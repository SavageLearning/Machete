using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service
{
    public interface IOnlineOrdersService
    {
        WorkOrder Get(int id);
        IEnumerable<WorkOrder> GetMany(Func<WorkOrder, bool> where);
        WorkOrder Create(WorkOrder order, string user);
    }

    public class OnlineOrdersService : IOnlineOrdersService
    {
        private readonly IMapper map;
        private readonly IWorkOrderService woserv;
        private readonly IWorkAssignmentService waserv;
        private readonly ITransportRuleService trServ;
        private readonly ITransportProvidersService tpServ;
        private readonly ILookupService lServ;


        public OnlineOrdersService(
            IWorkOrderService woServ,
            IWorkAssignmentService waServ,
            ITransportRuleService trServ,
            ITransportProvidersService tpServ,
            ILookupService lServ,
            IMapper map)
        {
            this.map = map;
            //this.eserv = eServ;
            this.woserv = woServ;
            this.waserv = waServ;
            this.trServ = trServ;
            this.tpServ = tpServ;
            this.lServ = lServ;
        }

        public WorkOrder Get(int id)
        {
            return woserv.Get(id);
        }

        public IEnumerable<WorkOrder> GetMany(Func<WorkOrder, bool> where)
        {
            return woserv.GetMany(where);
        }

        public WorkOrder Create(WorkOrder order, string user)
        {
            validateTransportRules(order);


            order.statusID = WorkOrder.iPending;
            var assignments = order.workAssignments;
            order.workAssignments = null;
            var entity = woserv.Create(order, null, user, assignments);
            return woserv.Get(entity.ID);
        }

        public bool validateTransportRules(WorkOrder order)
        {
            if (order.workAssignments == null)
                throw new MacheteValidationException("WorkAssignments can't be null");

            if (order.workAssignments.Count() == 0)
                throw new MacheteValidationException("WorkAssignments can't be empty");

            if (order.transportProviderID == 0)
                throw new MacheteValidationException("Transport pROVIDER can't be 0");

            var transMethod = tpServ.Get(order.transportProviderID);
            if (transMethod == null)
                throw new MacheteValidationException("Transport method lookup returned null");

            var trRules = trServ.GetMany(a => a.lookupKey == transMethod.key);
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
                var costRule = trRule.costRules.Where(c => wa.ID >= c.minWorker && wa.ID <= c.maxWorker).First();
                if (costRule == null)
                    throw new MacheteValidationException("No cost rule matching workAssignment ID");

                if (wa.transportCost != costRule.cost)
                    throw new MacheteValidationException("Unexpected transport cost from client");
            }

            return true;
        }

        public bool validateSkillsRules(WorkOrder order)
        {

            return true;
        }
    }
}
