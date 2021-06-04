using System;
using System.Linq;
using Machete.Service;
using Machete.Service.Infrastructure;
using Machete.Service.Tenancy;
using Machete.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Machete.Service.BackgroundServices
{
    public interface IWorkerActions
    {
        bool Execute();
        ExpireMembersResults ExpireMembers(MacheteContext context);
    }
    public class WorkerActions : IWorkerActions
    {
        private readonly ILogger<IWorkerActions> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ITenantService _tenantService;
        private IDatabaseFactory _dbFactory;

        public WorkerActions(
            ILogger<IWorkerActions> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public bool Execute()
        {
            var didExecute = false;

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _tenantService = (ITenantService)scope.ServiceProvider.GetRequiredService(typeof(ITenantService));
                    _dbFactory = (IDatabaseFactory)scope.ServiceProvider.GetRequiredService(typeof(IDatabaseFactory));

                    var tenants = _tenantService.GetAllTenants();

                    foreach (Tenant tenant in tenants)
                    {
                        var context = _dbFactory.Get(tenant);
                        var expireMembersRes = ExpireMembers(context);
                        didExecute = expireMembersRes.Executed;

                        _logger.LogWarning($"Updated {expireMembersRes.RecordsUpdated} record(s) in tenant -{tenant.Name}-");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to update expired members " + ex.Message + ex.InnerException);
            }

            return didExecute;
        }

        /// <summary>
        /// Expires active workers based on expiration date for a given tenant db
        /// </summary>
        /// <param name="context"></param>
        public ExpireMembersResults ExpireMembers(MacheteContext context)
        {
            var results = new ExpireMembersResults();
            results.Executed = false;
            results.RecordsUpdated = 0;

            var expiredLookUp = context.Lookups.SingleOrDefault(row => row.category.Equals(LCategory.memberstatus) && row.key.Equals(LMemberStatus.Expired));
            var activeLookUp = context.Lookups.SingleOrDefault(row => row.category.Equals(LCategory.memberstatus) && row.key.Equals(LMemberStatus.Active));

            var list = context.Workers
                .Where(w =>
                    w.memberexpirationdate < DateTime.Now &&
                    w.memberStatusID == activeLookUp.ID)
                .ToList();

            if (list.Any())
            {

                foreach (Worker wkr in list)
                {
                    wkr.memberStatusID = expiredLookUp.ID;
                    wkr.memberStatusEN = expiredLookUp.text_EN;
                    wkr.memberStatusES = expiredLookUp.text_ES;
                    wkr.updatedby = "ExpirationBot";
                }

                results.RecordsUpdated = context.SaveChanges();
                results.Executed = true;
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
