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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Globalization;
using NLog;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace Machete.Service
{
    public interface IWorkOrderService : IService<WorkOrder>
    {
        IEnumerable<WorkOrder> GetByEmployer(int id);
        IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly);
        IQueryable<WorkOrderSummary> GetSummary(string search);
        IQueryable<WorkOrderSummary> GetSummary();
        int CompleteActiveOrders(DateTime date, string user);
        dataTableResult<WOWASummary> CombinedSummary(string search,
            bool orderDescending,
            int displayStart,
            int displayLength);
        dataTableResult<WorkOrder> GetIndexView(viewOptions opt);
    }

    // Business logic for WorkOrder record management
    // This class should contain all of the business logic that would exist even for a non-web app
    public class WorkOrderService : ServiceBase<WorkOrder>, IWorkOrderService
    {
        private readonly IWorkAssignmentService waServ;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repo">Work Order repository</param>
        /// <param name="waServ">Work Assignment service</param>
        /// <param name="unitOfWork">Unit of Work</param>
        public WorkOrderService(IWorkOrderRepository repo, 
                                IWorkAssignmentService waServ,
                                IUnitOfWork unitOfWork) : base(repo, unitOfWork)
        {
            this.waServ = waServ;
            this.logPrefix = "WorkOrder"; // Initialize log prefix
        }

        /// <summary>
        /// Retrieve all worker orders for a specific Employer, or all work orders if null
        /// </summary>
        /// <param name="id">Employer ID</param>
        /// <returns>WorkOrders associated with employer</returns>
        public IEnumerable<WorkOrder> GetByEmployer(int id)
        {
            // TODO: investigate what happens if ID = null (should return all WO)
            // Retrieve work orders for given employer
            return repo.GetMany(wo => wo.EmployerID == id);
        }

        /// <summary>
        /// Retrieve active orders for a given day. Active and assigned OR all active
        /// </summary>
        /// <param name="date">filter for the date</param>
        /// <param name="assignedOnly">filter to only orders with fully assigned jobs</param>
        /// <returns>WorkOrders associated with a given date that are active</returns>
        public IEnumerable<WorkOrder> GetActiveOrders(DateTime date, bool assignedOnly)
        {
            IQueryable<WorkOrder> query = repo.GetAllQ();
            query = query.Where(wo => wo.status == WorkOrder.iActive && 
                            DbFunctions.DiffDays(wo.dateTimeofWork, date) == 0 ? true : false)
                    .AsQueryable();
            List<WorkOrder> list = query.ToList();
            List<WorkOrder> final = list.ToList();
            if (!assignedOnly)
            {
                return final;
            }
            int waCounter = 0;
            foreach (WorkOrder wo in list)
            {
                waCounter = 0;
                foreach (WorkAssignment wa in wo.workAssignments)
                {
                    waCounter++;
                    if (wa.workerAssignedID == null)
                    {
                        final.Remove(wo);
                        break;
                    }
                }
                if (waCounter == 0) // WO must have at least one WA to be completed
                {
                    final.Remove(wo);
                }
            }
            return final;
        }
        /// <summary>
        /// Complete active orders - change all WO status for a given date to complete
        /// </summary>
        /// <param name="date">Date to change WO status</param>
        /// <param name="user">User performing action</param>
        /// <returns>Count of completed work orders</returns>
        public int CompleteActiveOrders(DateTime date, string user)
        {
            // Retrieve list of active orders for given date
            IEnumerable<WorkOrder> woRecords = this.GetActiveOrders(date, true);

            // Count of active orders that were changed to completed
            int count = 0;

            // TODO: should there be logic to check if WO is unassigned or orphaned?

            // Iterate through list of active work orders
            foreach (WorkOrder wo in woRecords)
            {
                // Retrieve work order
                var order = this.Get(wo.ID);

                // Change work order state
                order.status = WorkOrder.iCompleted;
                
                // Save work order
                this.Save(order, user);
                
                // Increment counter
                count++;
            }

            return count;
        }

        /// <summary>
        /// Retrieve index view of work orders
        /// </summary>
        /// <param name="vo">viewOptions object</param>
        /// <returns>Table of work orders</returns>
        public dataTableResult<WorkOrder> GetIndexView(viewOptions vo)
        {
            // Initialize return table
            var result = new dataTableResult<WorkOrder>();

            // Retrieve all the records
            IQueryable<WorkOrder> woRecords = repo.GetAllQ();

            // Filter by employerID - if viewOption set
            if (vo.EmployerID != null)
            {
                IndexViewBase.filterEmployer(vo, ref woRecords);
            }

            // Filter by status - if viewOption set
            if (vo.status != null)
            {
                IndexViewBase.filterStatus(vo, ref woRecords);
            }

            // Filter by status - if viewOption set
            if (vo.onlineSource == true)
            {
                IndexViewBase.filterOnlineSource(vo, ref woRecords);
            }

            // Filter by status - if viewOption set
            if (!string.IsNullOrEmpty(vo.sSearch))
            {
                IndexViewBase.search(vo, ref woRecords);
            }

            // Sort results based on viewOptions
            IndexViewBase.sortOnColName(vo.sortColName, vo.orderDescending, ref woRecords);

            // Set results filtered count
            result.filteredCount = woRecords.Count();

            // Return query - based on displayStart & number of records to display
            result.query = woRecords.Skip<WorkOrder>((int)vo.displayStart).Take((int)vo.displayLength);

            // Set results total count
            result.totalCount = repo.GetAllQ().Count();

            return result;
        }

        /// <summary>
        /// Retrieve WO summary results - count of work orders with each status type for each date
        /// </summary>
        /// <returns>Work Order summary results</returns>
        public IQueryable<WorkOrderSummary> GetSummary()
        {
            // Call GetSummary() without search string
            return GetSummary(null);
        }

        /// <summary>
        /// Retrieve WO summary results - count of work orders with each status type for each date
        /// </summary>
        /// <param name="search">Search string criteria</param>
        /// <returns>Work Order summary results</returns>
        public IQueryable<WorkOrderSummary> GetSummary(string search)
        {
            IQueryable<WorkOrder> woResults;

            // Use search string if provided to retrieve all work orders on given date/time
            if (!string.IsNullOrEmpty(search))
            {
                woResults = IndexViewBase.filterDateTimeOfWork(repo.GetAllQ(), search);
            }
            else
            {
                woResults = repo.GetAllQ();
            }

            // Group work orders by date / status
            var group_query = from wo in woResults
                            group wo by new { 
                                dateSoW = DbFunctions.TruncateTime(wo.dateTimeofWork), // Group by date (not time)                                             
                                wo.status // Group by status
                            } into dayGroup
                            select new WorkOrderSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            // Return summary results (count of work orders with each status type for each date)
            return group_query;
        }

        /// <summary>
        /// Create Work Order
        /// </summary>
        /// <param name="workOrder">Work order to create</param>
        /// <param name="user">User performing action</param>
        /// <returns>Work Order object</returns>
        public override WorkOrder Create(WorkOrder workOrder, string user)
        {
            WorkOrder wo;
            workOrder.createdby(user); // Set timestamps
            wo = repo.Add(workOrder);
            // TODO: investigate why worker requests collection is added to wo - there is a similar collection of wa added to wo
            wo.workerRequests = new Collection<WorkerRequest>();
            uow.Commit();
            
            // Initialize PaperOrdernum to WO ID if not set
            if (wo.paperOrderNum == null)
            {
                wo.paperOrderNum = wo.ID;
                uow.Commit();
            }

            // Log results
            _log(workOrder.ID, user, "WorkOrder created");

            return wo;
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

            IEnumerable<WorkOrderSummary> woResult;
            IEnumerable<WorkAssignmentSummary> waResult;
            IEnumerable<WOWASummary> wowaResult;
            dataTableResult<WOWASummary> result = new dataTableResult<WOWASummary>();

            //pulling from DB here because the joins grind it to a halt
            // TODO: investigate how to do a left join - results only appear when there are WA assigned to WO
            woResult = GetSummary(search).AsEnumerable();
            waResult = waServ.GetSummary(search).AsEnumerable();
            wowaResult = woResult.Join(waResult,
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
            {
                wowaResult = wowaResult.OrderByDescending(p => p.date);
            }
            else
            {
                wowaResult = wowaResult.OrderBy(p => p.date);
            }

            result.filteredCount = wowaResult.Count();
            result.query = wowaResult.Skip<WOWASummary>((int)displayStart).Take((int)displayLength);
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
            levent.Properties["username"] = user;
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