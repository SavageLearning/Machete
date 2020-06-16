using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Data.Tenancy;
using Machete.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Machete.Service.BackgroundServices
{
    public interface IWorkerActions
    {
        void Execute();
        void SetContext(MacheteContext context);
        ExpireMembersResults ExpireMembers();
    }
    public class WorkerActions : IWorkerActions
    {
        private MacheteContext _context;
        MacheteContext Context { get => _context; set => _context = value; }
        LookupServiceHelper _lServiceHelper;
        private readonly ILogger<IWorkerActions> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ITenantService _tenantService;
        private IDatabaseFactory _dbFactory;
        public MacheteContext context;

        public WorkerActions(
            ILogger<IWorkerActions> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _lServiceHelper = new LookupServiceHelper();
        }

        public void SetContext(MacheteContext context)
        {
            _context = context;
            _lServiceHelper.setContext(_context);
        }

        public void Execute()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _tenantService = (ITenantService)scope.ServiceProvider.GetService(typeof(ITenantService));
                _dbFactory = (IDatabaseFactory)scope.ServiceProvider.GetService(typeof(IDatabaseFactory));

                var tenants = _tenantService.GetAllTenants();

                foreach (Tenant tenant in tenants)
                {
                    context = _dbFactory.Get(tenant);
                    SetContext(context);
                    var result = ExpireMembers();
                    _logger.LogWarning($"Updated {result.RecordsUpdated} record in tenant -{tenant.Name}-");
                }
            }
        }

        /// <summary>
        /// Expires active workers based on expiration date for a given tenant db
        /// </summary>
        /// <param name="context"></param>
        public ExpireMembersResults ExpireMembers()
        {
            var results = new ExpireMembersResults();

            var expiredLookUp = _lServiceHelper.GetByKey(LCategory.memberstatus, LMemberStatus.Expired);
            var activeLookUp = _lServiceHelper.GetByKey(LCategory.memberstatus, LMemberStatus.Active);

            IList<Worker> list = _context.Workers
                .Where(w =>
                    w.memberexpirationdate < DateTime.Now &&
                    w.memberStatusID == activeLookUp.ID)
                .ToList();

            if (list.Any())
            {
                try
                {
                    foreach (Worker wkr in list)
                    {
                        wkr.memberStatusID = expiredLookUp.ID;
                        wkr.memberStatusEN = expiredLookUp.text_EN;
                        wkr.memberStatusES = expiredLookUp.text_ES;
                        wkr.updatedby = "ExpirationBot";
                    }

                    results.RecordsUpdated = _context.SaveChanges();
                    results.Executed = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to update expired members " + ex.Message + ex.InnerException);

                }
            }

            return results;
        }
    }

    public struct ExpireMembersResults
    {
        public bool Executed { get; set; }
        public int RecordsUpdated { get; set; }
    }
}
