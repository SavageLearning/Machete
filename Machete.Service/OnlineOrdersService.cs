using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IOnlineOrdersService : IService<WorkOrder>
    {
        Employer GetEmployer(string userSubject);
        dataTableResult<DTO.WorkOrdersList> GetIndexView(viewOptions opt);
    }

    public class OnlineOrdersService : ServiceBase2<WorkOrder>, IOnlineOrdersService
    {
        private readonly DbSet<TransportRule> trSet;
        private readonly DbSet<TransportProvider> tpSet;
        private readonly IWorkOrderService workOrderService;

        public OnlineOrdersService(IDatabaseFactory dbf, IWorkOrderService woServ, IMapper map) : base(dbf, map)
        {
            this.workOrderService = woServ;
            trSet = this.db.Set<TransportRule>();
            tpSet = this.db.Set<TransportProvider>();
        }

        public Employer GetEmployer(string userSubject) {
            return db.Set<Employer>().Where(e => e.onlineSigninID.Equals(userSubject)).SingleOrDefault();
        }

        public new WorkOrder Create(WorkOrder order, string user)
        {
            validateTransportRules(order);
            order.statusID = WorkOrder.iPending;
            var assignments = order.workAssignments;
            order.workAssignments = null;
            var entity = workOrderService.Create(order, null, user, assignments);
            return workOrderService.Get(entity.ID);
        }

        public dataTableResult<DTO.WorkOrdersList> GetIndexView(viewOptions opt) { return workOrderService.GetIndexView(opt); }

        public bool validateTransportRules(WorkOrder order)
        {
            if (order.workAssignments == null)
                throw new MacheteValidationException("WorkAssignments can't be null");

            if (order.workAssignments.Count() == 0)
                throw new MacheteValidationException("WorkAssignments can't be empty");

            if (order.transportProviderID == 0)
                throw new MacheteValidationException("Transport provider can't be 0");

            var transMethod = tpSet.Find(order.transportProviderID);
            if (transMethod == null)
                throw new MacheteValidationException("Transport method lookup returned null");

            var trRules = trSet.Where(a => a.lookupKey == transMethod.key).AsQueryable();
            if (trRules == null || trRules.Count() == 0)
                throw new MacheteValidationException("TransportMethod does not have rules associated with it");

            var trRule = trRules.Where(r => r.zipcodes.Contains(order.zipcode) || r.zipcodes.Contains("*")).First();
            if (trRule == null)
                throw new MacheteValidationException("No rule matching order zipcode");
            //
            // the code assumes that IDs sent from the client
            // are unique and sequential. They determine price and are prescribed for the pricing logic:
            var i = 1;
            foreach (var wa in order.workAssignments.OrderBy(a => a.ID))
            {
                if (wa.ID != i)
                    throw new MacheteValidationException("Work Assignment ID invalid");
                i++;
            }

            foreach (var wa in order.workAssignments)
            {
                //
                // This assumes that the IDs are going to come in as 1,2,3,4...they're reset
                // later on down in the code
                var costRule = trRule.costRules.First(c => wa.ID >= c.minWorker && wa.ID <= c.maxWorker);
                if (costRule == null)
                    throw new MacheteValidationException("No cost rule matching workAssignment ID");

                if (wa.transportCost != costRule.cost)
                    throw new MacheteValidationException("Unexpected transport cost from client");
            }

            return true;
        }
    }
}
