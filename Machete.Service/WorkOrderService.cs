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
    public interface IWorkOrderService : IService<WorkOrder>
    {
        IEnumerable<WorkOrder> GetByEmployer(int id);
        IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly);
        IQueryable<WorkOrderSummary> GetSummary(string search);
        int CompleteActiveOrders(DateTime date, string user);
        dTableList<WOWASummary> CombinedSummary(string search,
            bool orderDescending,
            int displayStart,
            int displayLength);
        dTableList<WorkOrder> GetIndexView(viewOptions opt);
    }

    // Business logic for WorkOrder record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkOrderService : ServiceBase<WorkOrder>, IWorkOrderService
    {
        private readonly IWorkAssignmentService waServ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="unitOfWork"></param>
        public WorkOrderService(IWorkOrderRepository repo, 
                                IWorkAssignmentService waServ,
                                IUnitOfWork unitOfWork) : base(repo, unitOfWork)
        {
            this.waServ = waServ;
            this.logPrefix = "WorkOrder";
        }

        /// <summary>
        /// Get all orders for a specific Employer, or all orders if null
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public IEnumerable<WorkOrder> GetByEmployer(int id)
        {
             return repo.GetMany(w => w.EmployerID == id);
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
            IQueryable<WorkOrder> query = repo.GetAllQ();
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
                var order = this.Get(wo.ID);
                order.status = WorkOrder.iCompleted;
                this.Save(order, user);
                count++;
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">viewOptions object</param>
        /// <returns></returns>
        public dTableList<WorkOrder> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<WorkOrder> q = repo.GetAllQ();
            //
            if (o.EmployerID != null) IndexViewBase.filterEmployer(o, ref q);
            if (o.status != null) IndexViewBase.filterStatus(o, ref q);
            if (!string.IsNullOrEmpty(o.search)) IndexViewBase.search(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //
            q = q.Skip<WorkOrder>((int)o.displayStart).Take((int)o.displayLength);
            var filtered = q.Count();
            var total =  repo.GetAllQ().Count();
            return new dTableList<WorkOrder> 
            { 
                query = q,
                filteredCount = filtered,
                totalCount = total
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<WorkOrderSummary> GetSummary(string search)
        {
            IQueryable<WorkOrder> query;
            if (!string.IsNullOrEmpty(search)) 
                query = IndexViewBase.filterDateTimeOfWork(repo.GetAllQ(), search);            
            else query = repo.GetAllQ();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override WorkOrder Create(WorkOrder workOrder, string user)
        {
            WorkOrder wo;
            workOrder.createdby(user);
            wo = repo.Add(workOrder);
            wo.workerRequests = new Collection<WorkerRequest>();
            uow.Commit();
            if (wo.paperOrderNum == null) wo.paperOrderNum = wo.ID;
            uow.Commit();
            _log(workOrder.ID, user, "WorkOrder created");
            return wo;
        }
        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            nlog.Log(levent);
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
    public class WOWASummary
    {
        public DateTime? date { get; set; }
        public string weekday { get; set; }
        public int? pending_wo { get; set; }
        public int? pending_wa { get; set; }
        public int? active_wo { get; set; }
        public int? active_wa { get; set; }
        public int? completed_wo { get; set; }
        public int? completed_wa { get; set; }
        public int? cancelled_wo { get; set; }
        public int? cancelled_wa { get; set; }
        public int? expired_wo { get; set; }
        public int? expired_wa { get; set; }
    }
}