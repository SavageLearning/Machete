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
using Machete.Service.Helpers;


namespace Machete.Service
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetWorkOrders();
        IEnumerable<WorkOrder> GetWorkOrders(int? byEmployer);
        IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly);
        IQueryable<WorkOrderSummary> GetSummary(string search);
        int CompleteActiveOrders(DateTime date, string user);
        dTableList<WOWASummary> CombinedSummary(string search,
            bool orderDescending,
            int displayStart,
            int displayLength);
        WorkOrder GetWorkOrder(int id);
        WorkOrder CreateWorkOrder(WorkOrder workOrder, string user);
        void DeleteWorkOrder(int id, string user);
        void SaveWorkOrder(WorkOrder workOrder, string user);
        dTableList<WorkOrder> GetIndexView(woViewOptions opt);

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
        //
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
        /// Gets all orders
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetWorkOrders()
        {
            return GetWorkOrders(null);
        }
        /// <summary>
        /// Get all orders for a specific Employer, or all orders if null
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetWorkOrders(int? empID)
        {
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
        /// <summary>
        /// Gets active orders for a given day. Active and assigned OR all active
        /// </summary>
        /// <param name="date">filter for the date</param>
        /// <param name="assignedOnly">filter to only orders with fully assigned jobs</param>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly)
        {
            // I will rot in hell for hardcoding this value -- matches the Lookups table 
            // for active orderstatus
            IQueryable<WorkOrder> query = woRepo.GetAllQ();
                            query = query.Where(wo => wo.status == WorkOrder.iActive && 
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CompleteActiveOrders(DateTime date, string user)
        {
            IEnumerable<WorkOrder> list = this.GetActiveOrders(date, true);
            int count = 0;
            foreach (WorkOrder wo in list)
            {
                var order = this.GetWorkOrder(wo.ID);
                order.status = WorkOrder.iCompleted;
                this.SaveWorkOrder(order, user);
                count++;
            }
            return count;
        }

        #region GetIndexView
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">woViewOptions object</param>
        /// <returns></returns>
        public dTableList<WorkOrder> GetIndexView(woViewOptions o)
        {
            #region FILTER
            //Get all the records
            IQueryable<WorkOrder> filteredWO = woRepo.GetAllQ();
            IQueryable<WorkOrder> orderedWO;
            bool isDateTime = false;
            //
            //WHERE based on search-bar string 
            //
            if (o.EmployerID != null)
                filteredWO = filteredWO.Where(p => p.EmployerID.Equals((int)o.EmployerID)); //EmployerID for WorkOrderIndex view

            if (o.status != null)
                filteredWO = filteredWO.Where(p => p.status.Equals((int)o.status)); //Work Order Status

            if (!string.IsNullOrEmpty(o.search))
            {
                //Using DateTime.TryParse as determiner of date/string
                DateTime parsedTime;
                if (isDateTime = DateTime.TryParse(o.search, out parsedTime))
                {
                    if (isMonthSpecific.IsMatch(o.search))  //Regex for month/year
                        filteredWO = filteredWO.Where(p => EntityFunctions.DiffMonths(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isDaySpecific.IsMatch(o.search))  //Regex for day/month/year
                        filteredWO = filteredWO.Where(p =>EntityFunctions.DiffDays(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                    if (isTimeSpecific.IsMatch(o.search)) //Regex for day/month/year time
                        filteredWO = filteredWO.Where(p => EntityFunctions.DiffHours(p.dateTimeofWork, parsedTime) == 0 ? true : false);
                } else { 
                    filteredWO = filteredWO
                        .Where(p => SqlFunctions.StringConvert((decimal)p.ID).Contains(o.search) ||
                                    SqlFunctions.StringConvert((decimal)p.paperOrderNum).Contains(o.search) ||
                                    p.contactName.Contains(o.search) ||
                                    p.workSiteAddress1.Contains(o.search) ||
                                    p.Updatedby.Contains(o.search));
                }
            }
            #endregion 
            //
            //ORDER BY based on column selection
            #region ORDERBY
            switch (o.sortColName)
            {
                //case "WOID": orderedWO = orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
                case "status": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.status) : filteredWO.OrderBy(p => p.status); break;
                case "transportMethod": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.transportMethodID) : filteredWO.OrderBy(p => p.transportMethodID); break;
                case "WAcount": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.workAssignments.Count) : filteredWO.OrderBy(p => p.workAssignments.Count); break;
                case "contactName": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.contactName) : filteredWO.OrderBy(p => p.contactName); break;
                case "workSiteAddress1": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.workSiteAddress1) : filteredWO.OrderBy(p => p.workSiteAddress1); break;
                case "updatedby": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.Updatedby) : filteredWO.OrderBy(p => p.Updatedby); break;
                case "WOID": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.paperOrderNum) : filteredWO.OrderBy(p => p.paperOrderNum); break;
                case "dateupdated": orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.dateupdated) : filteredWO.OrderBy(p => p.dateupdated); break;
                default: orderedWO = o.orderDescending ? filteredWO.OrderByDescending(p => p.dateTimeofWork) : filteredWO.OrderBy(p => p.dateTimeofWork); break;
            }
            #endregion
            //
            //SKIP & TAKE for display
            //if (displayLength != 0 && displayStart != 0)
            orderedWO = orderedWO.Skip<WorkOrder>((int)o.displayStart).Take((int)o.displayLength);
            var filtered = filteredWO.Count();
            var total =  woRepo.GetAllQ().Count();
            return new dTableList<WorkOrder> 
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns></returns>
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
        public dTableList<WOWASummary> CombinedSummary(string search, 
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
                pending_wo = g.Where(c => c.status == WorkOrder.iPending).Sum(d => d.wo_count),
                pending_wa = g.Where(c => c.status == WorkOrder.iPending).Sum(d => d.wa_count),
                active_wo = g.Where(c => c.status == WorkOrder.iActive).Sum(d => d.wo_count),
                active_wa = g.Where(c => c.status == WorkOrder.iActive).Sum(d => d.wa_count),
                completed_wo = g.Where(c => c.status == WorkOrder.iCompleted).Sum(d => d.wo_count),
                completed_wa = g.Where(c => c.status == WorkOrder.iCompleted).Sum(d => d.wa_count),
                cancelled_wo = g.Where(c => c.status == WorkOrder.iCancelled).Sum(d => d.wo_count),
                cancelled_wa = g.Where(c => c.status == WorkOrder.iCancelled).Sum(d => d.wa_count),
                expired_wo = g.Where(c => c.status == WorkOrder.iExpired).Sum(d => d.wo_count),
                expired_wa = g.Where(c => c.status == WorkOrder.iExpired).Sum(d => d.wa_count)
            });

                if (orderDescending)
                    result = result.OrderByDescending(p => p.date);
                else
                    result = result.OrderBy(p => p.date);

                //Limit results to the display length and offset
                var displayedSummary = result.Skip(displayStart)
                                                    .Take(displayLength);
            return new dTableList<WOWASummary> {
                query = displayedSummary,
                filteredCount = result.Count(),
                totalCount = displayedSummary.Count()
                
            };
        }
    }
}