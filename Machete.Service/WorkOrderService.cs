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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;


namespace Machete.Service
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetWorkOrders();
        IEnumerable<WorkOrder> GetWorkOrders(int? byEmployer);
        IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly);
        IQueryable<WorkOrderSummary> GetSummary(string search);
        int CompleteActiveOrders(DateTime date, string user);
        ServiceIndexView<WOWASummary> CombinedSummary(string search,
            bool orderDescending,
            int displayStart,
            int displayLength);
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
                        int displayStart,
                        int displayLength,
                        string sortColName
            );
    }

    // Business logic for WorkOrder record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository woRepo;
        private readonly IWorkAssignmentService waServ;
        private readonly IUnitOfWork unitOfWork;
        private static Regex isTimeSpecific = new Regex(@"^\s*\d{1,2}[\/-_]\d{1,2}[\/-_]\d{2,4}\s+\d{1,2}:\d{1,2}");
        private static Regex isDaySpecific = new Regex(@"^\s*\d{1,2}\/\d{1,2}\/\d{2,4}");
        private static Regex isMonthSpecific = new Regex(@"^\s*\d{1,2}\/\d{4,4}");
        //private static Regex isYearSpecific = new Regex("(?<=%download%#)\\d+");
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkOrderService", "");
        private WorkOrder _workOrder;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="woRepo"></param>
        /// <param name="unitOfWork"></param>
        public WorkOrderService(IWorkOrderRepository woRepo, 
                               IWorkAssignmentService waServ,
                                IUnitOfWork unitOfWork)
        {
            this.woRepo = woRepo;
            this.waServ = waServ;
            this.unitOfWork = unitOfWork;            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetWorkOrders()
        {
            return GetWorkOrders(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetWorkOrders(int? empID)
        {
            //TODO Unit test this
            if (empID == null)
            {
                return woRepo.GetAll();
            }
            else
            {
                return woRepo.GetMany(w => w.EmployerID == empID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(int id)
        {
            var workOrder = woRepo.GetById(id);
            return workOrder;
        }

        public IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly)
        {
            //TODO should make statuses strongly typed (42 == active)
            // I will rot in hell for hardcoding this value -- matches the Lookups table 
            // for active orderstatus
            IQueryable<WorkOrder> query = woRepo.GetAllQ();
                            query = query.Where(wo => wo.status == 42 && 
                                           EntityFunctions.DiffDays(wo.dateTimeofWork, date) == 0 ? true : false)
                                    .AsQueryable();
            List<WorkOrder> list = query.ToList();
            List<WorkOrder> final = list.ToList();
            if (!assignedOnly) return final;
            foreach (WorkOrder wo in list)
            {
                foreach (WorkAssignment wa in wo.workAssignments)
                {
                    if (wa.workerAssignedID == null)
                    {
                        final.Remove(wo);
                        break;
                    }
                }
            }
            return final;
        }

        public int CompleteActiveOrders(DateTime date, string user)
        {
            IEnumerable<WorkOrder> list = this.GetActiveOrders(date, false);
            int count = 0;
            foreach (WorkOrder wo in list)
            {
                var order = this.GetWorkOrder(wo.ID);
                //TODO use strongly typed status here
                order.status = 44;
                this.SaveWorkOrder(order, user);
                count++;
            }
            return count;
        }

        #region GetIndexView
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CI"></param>
        /// <param name="search"></param>
        /// <param name="EmployerID"></param>
        /// <param name="status"></param>
        /// <param name="orderDescending"></param>
        /// <param name="displayStart"></param>
        /// <param name="displayLength"></param>
        /// <param name="sortColName"></param>
        /// <returns></returns>
        public ServiceIndexView<WorkOrder> GetIndexView(
                        CultureInfo CI,
                        string search,
                        int? EmployerID,
                        int? status,
                        bool orderDescending,
                        int displayStart,
                        int displayLength,
                        string sortColName                        
            )
        {
            //Get all the records
            IQueryable<WorkOrder> filteredWO = woRepo.GetAllQ();
            IQueryable<WorkOrder> orderedWO;
            bool isDateTime = false;
            //
            //WHERE based on search-bar string 
            //
            if (EmployerID != null)            
                filteredWO = filteredWO.Where(p => p.EmployerID.Equals((int)EmployerID)); //EmployerID for WorkOrderIndex view
            
            if (status != null) 
                filteredWO = filteredWO.Where(p => p.status.Equals((int)status)); //Work Order Status
            
            if (!string.IsNullOrEmpty(search))
            {
                //Using DateTime.TryParse as determiner of date/string
                DateTime parsedTime;
                if (isDateTime = DateTime.TryParse(search, out parsedTime))
                {
                    if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                        filteredWO = filteredWO.Where(p => EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                        filteredWO = filteredWO.Where(p =>EntityFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                        filteredWO = filteredWO.Where(p => EntityFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
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
            //
            //ORDER BY based on column selection
            //
            switch (sortColName)
            {
                //case "WOID": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
                case "status": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.status) : filteredWO.OrderBy(p => p.status); break;
                case "transportMethod": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.transportMethodID) : filteredWO.OrderBy(p => p.transportMethodID); break;
                case "WAcount": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.workAssignments.Count) : filteredWO.OrderBy(p => p.workAssignments.Count); break;
                case "contactName": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.contactName) : filteredWO.OrderBy(p => p.contactName); break;
                case "workSiteAddress1": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.workSiteAddress1) : filteredWO.OrderBy(p => p.workSiteAddress1); break;
                case "updatedby": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.Updatedby) : filteredWO.OrderBy(p => p.Updatedby); break;
                case "WOID": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.paperOrderNum) : filteredWO.OrderBy(p => p.paperOrderNum); break;
                case "dateupdated": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateupdated) : filteredWO.OrderBy(p => p.dateupdated); break;
                default: orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
            }
            //
            //SKIP & TAKE for display
            //
            //if (displayLength != 0 && displayStart != 0)
                orderedWO = orderedWO.Skip<WorkOrder>((int)displayStart).Take((int)displayLength);
            var filtered = filteredWO.Count();
            var total =  woRepo.GetAllQ().Count();
            return new ServiceIndexView<WorkOrder> 
            { 
                query = orderedWO,
                filteredCount = filtered,
                totalCount = total
            };
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<WorkOrderSummary> GetSummary(string search)
        {
            IQueryable<WorkOrder> query;
            if (!string.IsNullOrEmpty(search)) 
                query = filterDateTimeOfWork(woRepo.GetAllQ(), search);            
            else query = woRepo.GetAllQ();
            var group_query = from wo in query
                            group wo by new { 
                                dateSoW = EntityFunctions.TruncateTime(wo.dateTimeofWork),                                              
                                wo.status
                            } into dayGroup
                            select new WorkOrderSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            return group_query;
        }
        #region CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public WorkOrder CreateWorkOrder(WorkOrder workOrder, string user)
        {
            workOrder.createdby(user);
            _workOrder = woRepo.Add(workOrder);
            _workOrder.workerRequests = new Collection<WorkerRequest>();
            unitOfWork.Commit();
            if (_workOrder.paperOrderNum == null) _workOrder.paperOrderNum = _workOrder.ID;
            unitOfWork.Commit();
            _log(workOrder.ID, user, "WorkOrder created");
            return _workOrder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public void DeleteWorkOrder(int id, string user)
        {
            var workOrder = woRepo.GetById(id);
            woRepo.Delete(workOrder);
            _log(id, user, "WorkOrder deleted");
            unitOfWork.Commit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="user"></param>
        public void SaveWorkOrder(WorkOrder workOrder, string user)
        {
            workOrder.updatedby(user);
            _log(workOrder.ID, user, "WorkOrder edited");
            unitOfWork.Commit();
        }


        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
        #endregion
        ///
        ///
        private IQueryable<WorkOrder> filterDateTimeOfWork(IQueryable<WorkOrder> query, string search)
        {

            //Using DateTime.TryParse as determiner of date/string
            DateTime parsedTime;
            if (DateTime.TryParse(search, out parsedTime))
            {
                if (isMonthSpecific.IsMatch(search))  //Regex for month/year
                    return query.Where(p => EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isDaySpecific.IsMatch(search))  //Regex for day/month/year
                    return query.Where(p => EntityFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                if (isTimeSpecific.IsMatch(search)) //Regex for day/month/year time
                    return query.Where(p => EntityFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);                    
            }
            return query;                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ServiceIndexView<WOWASummary> CombinedSummary(string search, 
            bool orderDescending,
            int displayStart,
            int displayLength)
        {

            IEnumerable<WorkOrderSummary> woResult;
            IEnumerable<WorkAssignmentSummary> waResult;
            IEnumerable<WOWASummary> result;
            //pulling from DB here because the joins grind it to a halt
            woResult = GetSummary(search).ToList();
            waResult = waServ.GetSummary(search).ToList();
                result = woResult.Join(waResult,
                            wo => new { wo.date, wo.status },
                            wa => new { wa.date, wa.status },
                            (wo, wa) => new
                            {
                                wo.date,
                                wo.status,
                                wo_count = wo.count,
                                wa_count = wa.count
                            })
            .GroupBy(gb => gb.date)
            .Select(g => new WOWASummary
            {
                date = g.Key,
                weekday = Convert.ToDateTime(g.Key).ToString("dddd"),
                pending_wo = g.Where(c => c.status == 43).Sum(d => d.wo_count),
                pending_wa = g.Where(c => c.status == 43).Sum(d => d.wa_count),
                active_wo = g.Where(c => c.status == 42).Sum(d => d.wo_count),
                active_wa = g.Where(c => c.status == 42).Sum(d => d.wa_count),
                completed_wo = g.Where(c => c.status == 44).Sum(d => d.wo_count),
                completed_wa = g.Where(c => c.status == 44).Sum(d => d.wa_count),
                cancelled_wo = g.Where(c => c.status == 45).Sum(d => d.wo_count),
                cancelled_wa = g.Where(c => c.status == 45).Sum(d => d.wa_count),
                expired_wo = g.Where(c => c.status == 46).Sum(d => d.wo_count),
                expired_wa = g.Where(c => c.status == 46).Sum(d => d.wa_count)
            });

                if (orderDescending)
                    result = result.OrderByDescending(p => p.date);
                else
                    result = result.OrderBy(p => p.date);

                //Limit results to the display length and offset
                var displayedSummary = result.Skip(displayStart)
                                                    .Take(displayLength);
            return new ServiceIndexView<WOWASummary> {
                query = displayedSummary,
                filteredCount = result.Count(),
                totalCount = displayedSummary.Count()
                
            };
        }
    }
}