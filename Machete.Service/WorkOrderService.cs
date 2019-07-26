#region COPYRIGHT
// File:     WorkOrderService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Service
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Machete.Data.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Machete.Service
{
    public interface IWorkOrderService : IService<WorkOrder>
    {
        IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly);
        IQueryable<WorkOrderSummary> GetSummary(string search);
        int CompleteActiveOrders(DateTime date, string user);
        dataTableResult<WOWASummary> CombinedSummary(string search,
            bool orderDescending,
            int displayStart,
            int displayLength);
        dataTableResult<DTO.WorkOrdersList> GetIndexView(viewOptions opt);
        void Save(WorkOrder workOrder, List<WorkerRequest> wrList, string user);
        WorkOrder Create(WorkOrder wo, List<WorkerRequest> wrList, string userName, ICollection<WorkAssignment> was = null);
        WorkOrder Create(WorkOrder wo, string userName, ICollection<WorkAssignment> was = null);
    }

    public class WorkOrderService : ServiceBase<WorkOrder>, IWorkOrderService
    {
        private readonly IWorkAssignmentService waServ;
        private readonly IWorkerRequestService wrServ;
        private readonly IWorkerService wServ;
        private readonly IMapper map;
        private readonly ILookupRepository lRepo;
        private readonly IConfigService cfg;
        private readonly ITransportProvidersService tpServ;
        private readonly TimeZoneInfo _clientTimeZoneInfo;
        private new readonly IWorkOrderRepository repo;

        /// <summary>
        /// Business logic object for WorkOrder record management. Contains logic specific
        /// to processing work orders, and not necessarily related to a web application.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="waServ">Work Assignment service</param>
        /// <param name="tpServ"></param>
        /// <param name="wrServ"></param>
        /// <param name="wServ"></param>
        /// <param name="lRepo"></param>
        /// <param name="uow">Unit of Work</param>
        /// <param name="map"></param>
        /// <param name="cfg"></param>
        /// <param name="tenantService"></param>
        public WorkOrderService(IWorkOrderRepository repo, 
                                IWorkAssignmentService waServ,
                                ITransportProvidersService tpServ,
                                IWorkerRequestService wrServ,
                                IWorkerService wServ,
                                ILookupRepository lRepo,
                                IUnitOfWork uow,
                                IMapper map,
                                IConfigService cfg,
                                ITenantService tenantService
        ) : base(repo, uow)
        {
            this.repo = repo;
            this.waServ = waServ;
            this.wrServ = wrServ;
            this.wServ = wServ;
            this.map = map;
            this.lRepo = lRepo;
            this.cfg = cfg;
            this.tpServ = tpServ;
            this.logPrefix = "WorkOrder";
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        /// <summary>
        /// Retrieve active orders for a given day. Active and assigned OR all active
        /// </summary>
        /// <param name="date">filter for the date</param>
        /// <param name="assignedOnly">filter to only orders with fully assigned jobs</param>
        /// <returns>WorkOrders associated with a given date that are active</returns>
        public IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly)
        {
            var matching = repo.GetActiveOrders(date);
//                .Where(wo => wo.statusID == WorkOrder.iActive
//                          && wo.dateTimeofWork.Date == date.Date).ToList();

            if (!assignedOnly) return matching;
            
            var result = new List<WorkOrder>();

            foreach (WorkOrder wo in matching)
            {
                // WO must have at least one WA to be marked active
                if (wo.workAssignments.Count == 0) continue;

                var nullAssignments = 0;

                foreach (var wa in wo.workAssignments)
                {
                    if (wa.workerAssignedID == null) nullAssignments++;
                }

                if (nullAssignments > 0) continue;
                
                result.Add(wo);
            }

            return result;
        }
        /// <summary>
        /// Complete active orders - change all WO status for a given date to complete
        /// </summary>
        /// <param name="date">Date to change WO status</param>
        /// <param name="user">User performing action</param>
        /// <returns>Count of completed work orders</returns>
        public int CompleteActiveOrders(DateTime date, string user)
        {
            IEnumerable<WorkOrder> list = this.GetActiveOrders(date, true);
            int count = 0;
            foreach (WorkOrder wo in list)
            {
                var order = this.Get(wo.ID);
                order.statusID = WorkOrder.iCompleted;
                this.Save(order, user);
                count++;
            }
            return count;
        }
        /// <summary>
        /// Retrieve index view of work orders
        /// </summary>
        /// <param name="vo">viewOptions object</param>
        /// <returns>Table of work orders</returns>
        public dataTableResult<DTO.WorkOrdersList> GetIndexView(viewOptions o)
        {
            //Get all the records
            var result = new dataTableResult<DTO.WorkOrdersList>();
            IQueryable<WorkOrder> q = repo.GetAllQ();
            //
            if (o.EmployerID != null) IndexViewBase.filterEmployer(o, ref q);
            if (o.employerGuid != null) IndexViewBase.filterEmployerByGuid(o, ref q);
            if (o.status != null) IndexViewBase.filterStatus(o, ref q);
            if (o.onlineSource == true) IndexViewBase.filterOnlineSource(o, ref q);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            // TODO restore CultureInfo
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, /*o.CI.TwoLetterISOLanguageName*/"en", ref q);
            //
            result.filteredCount = q.Count();
            result.query = q.ProjectTo<DTO.WorkOrdersList>(map.ConfigurationProvider)
            .Skip(o.displayStart)
            .Take(o.displayLength)
            .AsEnumerable();
                
            result.totalCount = repo.GetAllQ().Count();
            return result;
        }

        /// <summary>
        /// Retrieve WO summary results - count of work orders with each status type for each date
        /// </summary>
        /// <param name="search">Search string criteria</param>
        /// <returns>Work Order summary results</returns>
        public IQueryable<WorkOrderSummary> GetSummary(string search)
        {
            var workOrders = repo.GetAllQ();
            
            IQueryable<WorkOrder> query;
            if (string.IsNullOrEmpty(search)) query = workOrders;
            else query = IndexViewBase.filterDateTimeOfWork(workOrders, search);
            
            var group_query = from wo in query
                            group wo by new { 
                                dateSoW = TimeZoneInfo.ConvertTimeFromUtc(wo.dateTimeofWork, _clientTimeZoneInfo).Date,                                              
                                wo.statusID
                            } into dayGroup
                            select new WorkOrderSummary
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.statusID,
                                count = dayGroup.Count()
                            };

            return group_query;
        }

        /// <summary>
        /// A method to create a WorkOrder, along with associated WorkAssignments and WorkerRequests, in the database. 
        /// </summary>
        /// <param name="workOrder">The work order to be created.</param>
        /// <param name="workerRequestList">A list of worker requests made by the employer.</param>
        /// <param name="username">User performing action</param>
        /// <param name="workAssignments">A collection representing the worker assignments for the work order.</param>
        /// <returns>The created WorkOrder.</returns>
        public WorkOrder Create(WorkOrder workOrder, List<WorkerRequest> workerRequestList, string username, ICollection<WorkAssignment> workAssignments = null)
        {
            workOrder.timeZoneOffset = Convert.ToDouble(cfg.getConfig(Cfg.TimeZoneDifferenceFromPacific));
            updateComputedValues(ref workOrder);
            workOrder.createdByUser(username);
            var createdWorkOrder = repo.Add(workOrder);
            createdWorkOrder.workerRequestsDDD = new Collection<WorkerRequest>();
            if (workerRequestList != null)
            {
                foreach (var workerRequest in workerRequestList)
                {
                    workerRequest.workOrder = createdWorkOrder;
                    workerRequest.workerRequested = wServ.Get(workerRequest.WorkerID);
                    workerRequest.updatedByUser(username);
                    workerRequest.createdByUser(username);
                    createdWorkOrder.workerRequestsDDD.Add(workerRequest);
                }
            }
            uow.SaveChanges();
            
            if (createdWorkOrder.paperOrderNum == null || createdWorkOrder.paperOrderNum == 0)
            {
                createdWorkOrder.paperOrderNum = createdWorkOrder.ID;
            }
            if (workAssignments != null)
            {
                foreach (var workAssignment in workAssignments)
                {
                    workAssignment.ID = default(int); //so EF Core will save the record; otherwise IDENTITY_INSERT fails
                    workAssignment.workOrderID = createdWorkOrder.ID;
                    workAssignment.workOrder = createdWorkOrder;
                    waServ.Create(workAssignment, username);
                }
            }

            uow.SaveChanges();

            _log(workOrder.ID, username, "WorkOrder created");
            return createdWorkOrder;
        }

        public WorkOrder Create(WorkOrder wo, string user, ICollection<WorkAssignment> was = null)
        {
            return Create(wo, null, user, was);
        }

        private void updateComputedValues(ref WorkOrder record)
        {
            var lookup = lRepo.GetById(record.statusID);
            var transportProvider = tpServ.Get(record.transportProviderID);

            record.statusES = lookup.text_ES;
            record.statusEN = lookup.text_EN;
            record.transportMethodEN = transportProvider.text_EN;
            record.transportMethodES = transportProvider.text_ES;
        }

        public void Save(WorkOrder workOrder, List<WorkerRequest> wrList, string user)
        {
            // Stale requests to remove
            foreach (var rem in workOrder.workerRequestsDDD.Except(wrList, new WorkerRequestComparer()).ToArray())
            {
                var request = wrServ.GetByID(workOrder.ID, rem.WorkerID);
                wrServ.Delete(request.ID, user);
                workOrder.workerRequestsDDD.Remove(rem);
            }

            // New requests to add
            foreach (var add in wrList.Except(workOrder.workerRequestsDDD, new WorkerRequestComparer()))
            {
                add.workOrder = workOrder;
                add.workerRequested = wServ.Get(add.WorkerID);
                add.updatedByUser(user);
                add.createdByUser(user);
                workOrder.workerRequestsDDD.Add(add);
            }

            Save(workOrder, user);
        }

        public override void Save(WorkOrder workOrder, string user)
        {
            updateComputedValues(ref workOrder);
            if (workOrder.paperOrderNum == null)
            {
                workOrder.paperOrderNum = workOrder.ID;
            }
            base.Save(workOrder, user);
        }
        /// <summary>
        /// Provide combined summary of WO/WA status
        /// </summary>
        /// <param name="search">Search text criteria</param>
        /// <param name="orderDescending">Flag indicating whether results are sorted in descending order</param>
        /// <param name="displayStart">Record to start displaying (used for pagination)</param>
        /// <param name="displayLength">Number of records to display</param>
        /// <returns>WO/WA Summary table of status counts for a given day</returns>
        public dataTableResult<WOWASummary> CombinedSummary(string search, 
            bool orderDescending,
            int displayStart,
            int displayLength)
        {
            var result = new dataTableResult<WOWASummary>();
            //pulling from DB here because the joins grind it to a halt
            // TODO: investigate how to do a left join - results only appear when there are WA assigned to WO
            var woResult = GetSummary(search).AsEnumerable();
            var waResult = waServ.GetSummary(search).AsEnumerable();
            
            var q = woResult.Join(waResult,
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

            // Sort results on date (depending on orderDescending input parameter)
                if (orderDescending)
                    q = q.OrderByDescending(p => p.date);
                else
                    q = q.OrderBy(p => p.date);

                result.filteredCount = q.Count();
                result.query = q.Skip<WOWASummary>((int)displayStart).Take((int)displayLength);
                result.totalCount = repo.GetAllQ().Count();
                return result;
        }

        /// <summary>
        /// Log messages
        /// </summary>
        /// <param name="int">Work Order ID</param>
        /// <param name="user">User performing action</param>
        /// <param name="msg">Logging message</param>
        /// <returns>N/A</returns>
        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            //levent.Properties["username"] = user.Substring(0, 24); // Note: there is a bug in the logger that appends the username to itself causing logs generated by users with more than 25 characters to crash the code. Truncated this string to temporarily fix that bug.
            nlog.Log(levent);
        }        
    }

    /// <summary>
    /// Summary object of WO/WA status on a given date
    /// </summary>
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
