using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;

namespace Machete.Service
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetWorkOrders();
        IEnumerable<WorkOrder> GetWorkOrders(int? byEmployer);
        IEnumerable<WorkOrderSummary> GetSummary();
        WorkOrder GetWorkOrder(int id);
        WorkOrder CreateWorkOrder(WorkOrder workOrder, string user);
        void DeleteWorkOrder(int id, string user);
        void SaveWorkOrder(WorkOrder workOrder, string user);
    }

    // Business logic for WorkOrder record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository workOrderRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkOrderService", "");
        private WorkOrder _workOrder;
        //
        public WorkOrderService(IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
        {
            this.workOrderRepository = workOrderRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WorkOrder> GetWorkOrders()
        {
            return GetWorkOrders(null);
        }
        public IEnumerable<WorkOrder> GetWorkOrders(int? empID)
        {
            //TODO Unit test this
            if (empID == null)
            {
                return workOrderRepository.GetAll();
            }
            else
            {
                return workOrderRepository.GetMany(w => w.EmployerID == empID);
            }
        }

        public WorkOrder GetWorkOrder(int id)
        {
            var workOrder = workOrderRepository.GetById(id);
            return workOrder;
        }

        public IEnumerable<WorkOrderSummary> GetSummary()
        {
            var sum_query = from wo in workOrderRepository.GetAll()
                            group wo by new { dateSoW = wo.dateTimeofWork.ToString("MM/dd/yyyy"),                                              
                                              wo.status} into dayGroup
                            select new WorkOrderSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            return sum_query;
        }

        public WorkOrder CreateWorkOrder(WorkOrder workOrder, string user)
        {
            workOrder.createdby(user);
            _workOrder = workOrderRepository.Add(workOrder);
            unitOfWork.Commit();
            _log(workOrder.ID, user, "WorkOrder created");
            return _workOrder;
        }

        public void DeleteWorkOrder(int id, string user)
        {
            var workOrder = workOrderRepository.GetById(id);
            workOrderRepository.Delete(workOrder);
            _log(id, user, "WorkOrder deleted");
            unitOfWork.Commit();
        }

        public void SaveWorkOrder(WorkOrder workOrder, string user)
        {
            workOrder.updatedby(user);
            _log(workOrder.ID, user, "WorkOrder edited");
            unitOfWork.Commit();
        }

        public void AddWorkerRequest(int id, int workerID, string user)
        {

        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
    }
}