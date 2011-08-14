using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Globalization;
using NLog;
using System.Data.Objects.SqlClient;
using System.Data.Objects;


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
        ServiceIndexView<WorkOrder> GetIndexView(
                        CultureInfo CI,
                        string search,
                        int? EmployerID,
                        int? status,
                        bool orderDescending,
                        int? displayStart,
                        int? displayLength,
                        string sortColName
            );
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

        public ServiceIndexView<WorkOrder> GetIndexView(
                        CultureInfo CI,
                        string search,
                        int? EmployerID,
                        int? status,
                        bool orderDescending,
                        int? displayStart,
                        int? displayLength,
                        string sortColName                        
            )
        {
            //Get all the records
            IQueryable<WorkOrder> filteredWO = workOrderRepository.GetAllQ();
            IQueryable<WorkOrder> orderedWO;
            bool isDateTime = false;
            //Search based on search-bar string 
            if (EmployerID != null) //EmployerID for WorkOrderIndex view
            {
                filteredWO = filteredWO
                    .Where(p => p.EmployerID.Equals((int)EmployerID));
            }
            if (status != null) //Work Order Status
            {
                filteredWO = filteredWO
                    .Where(p => p.status.Equals((int)status));
            }
            if (!string.IsNullOrEmpty(search))
            {
                DateTime parsedTime;
                if (isDateTime = DateTime.TryParse(search, out parsedTime))
                {
                    
                    filteredWO = filteredWO
                            .Where(p => 
                                EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false //||
                                //EntityFunctions.DiffMonths(p.dateupdated, parsedTime) == 0 ? true : false
                            );
                } else { 
                    filteredWO = filteredWO
                        .Where(p => SqlFunctions.StringConvert((decimal)p.ID).Contains(search) ||
                                    SqlFunctions.StringConvert((decimal)p.paperOrderNum).Contains(search) ||                                    
                                    p.contactName.Contains(search) ||
                                    p.workSiteAddress1.Contains(search) ||
                                    p.Updatedby.Contains(search)
                                    );
                }
            }
            //var counted = filteredWO.Count();
            //Sort the Persons based on column selection
            switch (sortColName)
            {
                //case "WOID": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
                case "status": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.status) : filteredWO.OrderBy(p => p.status); break;
                case "WAcount": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.workAssignments.Count) : filteredWO.OrderBy(p => p.workAssignments.Count); break;
                case "contactName": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.contactName) : filteredWO.OrderBy(p => p.contactName); break;
                case "workSiteAddress1": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.workSiteAddress1) : filteredWO.OrderBy(p => p.workSiteAddress1); break;
                case "updatedby": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.Updatedby) : filteredWO.OrderBy(p => p.Updatedby); break;
                case "WOID": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.paperOrderNum) : filteredWO.OrderBy(p => p.paperOrderNum); break;
                default: orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
            }

            if (displayLength != null && displayStart != null)
                orderedWO = orderedWO.Skip<WorkOrder>((int)displayStart).Take((int)displayLength);
            var filtered = filteredWO.Count();
            var total =  workOrderRepository.GetAllQ().Count();
            return new ServiceIndexView<WorkOrder> 
            { 
                query = orderedWO,
                filteredCount = filtered,
                totalCount = total
            };
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
            if (_workOrder.paperOrderNum == null) _workOrder.paperOrderNum  = _workOrder.ID;
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