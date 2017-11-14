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
        //WorkOrder Get(int id);
        IEnumerable<WorkOrder> GetAll();
        IEnumerable<WorkOrder> GetMany(Func<WorkOrder, bool> where);
        WorkOrder Create(WorkOrder order, string user);
    }

    public class OnlineOrdersService : IOnlineOrdersService
    {
        private readonly IMapper map;
        private readonly IEmployerService eserv;
        private readonly IWorkOrderService woserv;
        private readonly IWorkAssignmentService waserv;
        private readonly ITransportRuleService trServ;
        private readonly ILookupService lServ;


        public OnlineOrdersService(
            //IEmployerService eServ,
            IWorkOrderService woServ,
            IWorkAssignmentService waServ,
            ITransportRuleService trServ,
            ILookupService lServ,
            IMapper map)
        {
            this.map = map;
            //this.eserv = eServ;
            this.woserv = woServ;
            this.waserv = waServ;
            this.trServ = trServ;
            this.lServ = lServ;
        }

        public IEnumerable<WorkOrder> GetAll()
        {
            return woserv.GetAll();
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
            var entity = woserv.Create(order, user);
            foreach (var a in assignments)
            {
                a.workOrderID = entity.ID;
                a.workOrder = entity;
                waserv.Create(a, user);
            }
            return woserv.Get(entity.ID);
        }

        public bool validateTransportRules(WorkOrder order)
        {
            if (order.workAssignments == null)
                throw new MacheteNullObjectException("WorkAssignments can't be null");

            if (order.workAssignments.Count() == 0)
                throw new MacheteInvalidInputException("WorkAssignments can't be empty");

            if (order.transportMethodID == 0)
                throw new MacheteInvalidInputException("TransportMethod can't be 0");

            var transMethod = lServ.Get(order.transportMethodID);
            if (transMethod == null)
                throw new MacheteNullObjectException("Transport method lookup returned null");

            var trRules = trServ.GetMany(a => a.lookupKey == transMethod.key);
            if (trRules == null || trRules.Count() == 0)
                throw new MacheteNullObjectException("TransportMethod does not have rules associated with it");

            var trRule = trRules.Where(r => r.zipcodes.Contains(order.zipcode)).First();
            if (trRule == null)
                throw new MacheteNullObjectException("No rule matching order zipcode");

            foreach (var wa in order.workAssignments)
            {
                //
                // This assumes that the IDs are going to come in as 1,2,3,4...they're reset
                // later on down in the code
                var costRule = trRule.costRules.Where(c => wa.ID >= c.minWorker && wa.ID <= c.maxWorker).First();
                if (costRule == null)
                    throw new MacheteNullObjectException("No cost rule matching workAssignment ID");

                if (wa.transportCost != costRule.cost)
                    throw new MacheteInvalidInputException("Unexpected transport cost from client");
            }

            return true;
        }
    }
}
