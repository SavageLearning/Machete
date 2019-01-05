#region COPYRIGHT
// File:     WorkAssignmentService.cs
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
using System;
using System.Linq;

namespace Machete.Service
{
    public interface IWorkAssignmentService :IService<WorkAssignment>
    {        
        IQueryable<WorkAssignmentSummary> GetSummary(string search);
        bool Assign(WorkAssignment assignment, WorkerSignin signin, string user);
        bool Unassign(int? wsiid, int? waid, string user);
        dataTableResult<DTO.WorkAssignmentsList> GetIndexView(viewOptions o);
        void Save(WorkAssignment asmt, int? workerAssignedID, string user);
    }

    // Business logic for WorkAssignment record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkAssignmentService : ServiceBase<WorkAssignment>, IWorkAssignmentService
    {
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILookupRepository lRepo;
        private readonly IMapper map;
        //
        //
        public WorkAssignmentService(
            IWorkAssignmentRepository waRepo, 
            IWorkerRepository wRepo, 
            ILookupRepository lRepo, 
            IWorkerSigninRepository wsiRepo,
            IUnitOfWork unitOfWork,
            IMapper map
            ) : base(waRepo, unitOfWork)
        {
            this.waRepo = waRepo;
            this.unitOfWork = unitOfWork;
            this.wRepo = wRepo;
            this.lRepo = lRepo;
            this.wsiRepo = wsiRepo;
            this.map = map;
            this.logPrefix = "WorkAssignment";
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override WorkAssignment Get(int id)
        {
            var wa = waRepo.GetById(id);
            if (wa.workerAssignedID != null)
            {
                wa.workerAssigned = wRepo.GetById((int)wa.workerAssignedID);
            }         
            return wa;
        }
        public dataTableResult<DTO.WorkAssignmentsList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.WorkAssignmentsList>();
            IQueryable<WorkAssignment> q = waRepo.GetAllQ();
            //
            // 
            if (o.date != null) IndexViewBase.diffDays(o, ref q);
            if (o.typeofwork_grouping > 0) IndexViewBase.typeOfWork(o, ref q, lRepo);
            if (o.woid > 0) IndexViewBase.WOID(o, ref q);
            if (o.personID > 0) IndexViewBase.WID(o, ref q);
            if (o.status > 0) IndexViewBase.status(o, ref q);
            if (o.showPending == false) IndexViewBase.filterPending(o, ref q);
            if (!string.IsNullOrEmpty(o.wa_grouping)) IndexViewBase.waGrouping(o, ref q, lRepo);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q, lRepo);
            if (o.dwccardnum > 0)
            {
                var worker = wRepo.GetById((int)o.dwccardnum);
                IndexViewBase.filterOnSkill(o, q, lRepo, worker);
            }
            //Sort the Persons based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //e = e.ToList();
            result.filteredCount = q.Count();
            if (o.displayLength > 0)
            {
                result.query = q.ProjectTo<DTO.WorkAssignmentsList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            }
            else
            {
                result.query = q.ProjectTo<DTO.WorkAssignmentsList>(map.ConfigurationProvider)
                .AsEnumerable();
            }
            result.totalCount = waRepo.GetAllQ().Count();
           return result;
      }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IQueryable<WorkAssignmentSummary> GetSummary(string search)
        {
            IQueryable<WorkAssignment> query = waRepo.GetAllQ();
            if (!string.IsNullOrEmpty(search))
                IndexViewBase.filterOnDatePart(search, ref query);

            var sum_query = from wa in query //LINQ
                            group wa by new
                            {
                                dateSoW = wa.workOrder.dateTimeofWork.Date,                               
                                wa.workOrder.statusID
                            } into dayGroup
                            select new WorkAssignmentSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.statusID,
                                count = dayGroup.Count()
                            };

            return sum_query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waid"></param>
        /// <param name="wsiid"></param>
        /// <returns></returns>
        public bool Assign(WorkAssignment asmt, WorkerSignin signin, string user)
        {
            int wid;
            //Assignments must be explicitly unassigned first; throws exception if either record is in assigned state
            if (signin == null) throw new NullReferenceException("WorkerSignin is null");
            if (asmt == null) throw new NullReferenceException("WorkAssignment is null");
            //
            // validate state of WSI and WA records
            assignCheckWSI_WAID_must_be_null(signin);
            assignCheckWSI_WID_cannot_be_null(signin);
            assignCheckWA_WSIID_must_be_null(asmt);
            assignCheckWA_legitimate_orphan(asmt, signin);
            wid = assignCheckWSI_cardnumber_match(signin);
            //
            // Link signin with work assignment
            signin.WorkAssignmentID = asmt.ID;
            asmt.workerSigninID = signin.ID;
            asmt.workerAssignedID = wid;
            //
            // update timestamps and save
            asmt.updatedByUser(user);
            signin.updatedByUser(user);
            unitOfWork.Commit();
            log(asmt.ID, user, "WSIID:" + signin.ID + " Assign successful");
            return true;
        }
        /// <summary>
        /// WorkerSignin's WAID must be null for an assignment
        /// </summary>
        /// <param name="wsi"></param>
        private static void assignCheckWSI_WAID_must_be_null(WorkerSignin wsi)
        {
            if (wsi.WorkAssignmentID != null)
                throw new MacheteDispatchException(
                    "WorkerSignin already associated with WorkAssignment ID" +
                    wsi.WorkAssignmentID);
        }
        /// <summary>
        /// WorkerSignin's WID must not be null for an assignment
        /// </summary>
        /// <param name="wsi"></param>
        private static void assignCheckWSI_WID_cannot_be_null(WorkerSignin wsi)
        {
            if (wsi.WorkerID == null)
                throw new MacheteIntegrityException(
                    "WorkerSignin key " + wsi.dwccardnum.ToString() +
                    "is not associated with a Worker record. " +
                    "Machete cannot assign a worker when no Worker record exists.");
        }
        /// <summary>
        /// WorkAssignment's WSIID must be null for an assignment
        /// </summary>
        /// <param name="wa"></param>
        private static void assignCheckWA_WSIID_must_be_null(WorkAssignment wa)
        {
            //
            //WA.WSIID must not point to a signin
            if (wa.workerSigninID != null)
                throw new MacheteDispatchException(
                    "WorkAssignment already associated with WorkerSignin ID " +
                    wa.workerSigninID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wa"></param>
        /// <param name="wsi"></param>
        private static void assignCheckWA_legitimate_orphan(WorkAssignment wa, WorkerSignin wsi)
        {
            if (wa.workerSigninID == null &&              //WA.WSIID == NULL
                wa.workerAssignedID != null &&            //WA.WID != NULL
                // Orphan Assignment if first two tests pass
                // If orphan, can only assign to a WSI with the same WID. Otherwise, 
                // user needs to unassign first
                wa.workerAssignedID != wsi.WorkerID)   //WA.WID != WSI.WID
                throw new MacheteDispatchException(
                    "Orphaned WorkAssignment, associated with Worker ID " +
                    wa.workerAssignedID + "; Unassign first, the assign to new Worker");
        }
        /// <summary>
        /// Check that WorkerSignin's Worker ID matches Worker's ID returned from cardnumber get
        /// </summary>
        /// <param name="wsi"></param>
        /// <returns></returns>
        private int assignCheckWSI_cardnumber_match(WorkerSignin wsi)
        {
            Worker worker = wRepo.GetByMemberID(wsi.dwccardnum);
            if (worker == null) throw new NullReferenceException("Worker for key " + wsi.dwccardnum.ToString() + " is null");
            if (worker.ID != wsi.WorkerID) throw new MacheteIntegrityException("WorkerSignin's internal WorkerID and public worker ID don't match");
            return worker.ID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waid"></param>
        /// <param name="wsiid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Unassign(int? waid, int? wsiid, string user)
        {
            //UI lets user select either WSI or WA and click remove.
            //Unassign decides which handlers to call
            //
            // WorkerSignin but no WorkAssignment
            if (wsiid != null && waid == null) unassignWorkerSigninOnly((int)wsiid, user);
            //           
            // WorkAssignment bu not WorkerSignin
            if (waid != null && wsiid == null) unassignWorkAssignmentOnly((int)waid, user);
            // Both
            if (waid != null && wsiid != null) unassignBoth((int)waid, (int)wsiid, user);
            // call error
            if (waid == null && wsiid == null)
                throw new NullReferenceException("Signin and WorkAssignment are both null");
            return true;
        }
        private void unassignWorkAssignmentOnly(int waid, string user)
        {
            // Get assignment
            WorkAssignment wa = waRepo.GetById((int)waid);
            if (wa == null) throw new NullReferenceException("WAID " + waid.ToString() +
                "returned a null Work Assignment record");
            //
            // Starting with WA:
            // 1. Does WA.WSIID point to anything?
            if (wa.workerSigninID == null) // No
            {
                // clear any orphan assignment
                wa.workerAssignedID = null;
                unitOfWork.Commit();
                return;
            }
            //
            // 2. WA.WSIID points to something. Does it link back?
            if (matchWAWSI(wa, null))
            {
                // Unassign both
                WorkerSignin wsi = wsiRepo.GetById((int)wa.workerSigninID);
                unassignBoth(wa, wsi, user);
                return;
            }
            //
            // 3. If points to something, but doesn't link bach, does something
            //    match it's link?
            WorkerSignin linkedWSI = wsiRepo.GetById((int)wa.workerSigninID);
            if (linkedWSI.WorkAssignmentID == null || matchWAWSI(null, linkedWSI))
            {
                //Something matches its link. My link to something assumed bad.
                wa.workerSigninID = null;
                unitOfWork.Commit();
                return;
            }
            else throw new MacheteIntegrityException("Unassign found chain of mislinked records, starting with WAID " + wa.ID.ToString());
        }
        private bool matchWAWSI(WorkAssignment wa, WorkerSignin wsi)
        {
            if (wa == null && wsi == null) throw new NullReferenceException("WorkAssignment and WorkerSignin objects both null.");
            if (wa == null)
            {
                // only have WSI and wsi.WAID is null. no match.
                if (wsi.WorkAssignmentID == null) return false;
                wa = waRepo.GetById((int)wsi.WorkAssignmentID);
                if (wa == null) throw new NullReferenceException("WorkAssignment GetById returned null");
            }
            if (wsi == null)
            {
                // only have WA and wa.WSIID is null. no match
                if (wa.workerSigninID == null) return false;
                wsi = wsiRepo.GetById((int)wa.workerSigninID);
                if (wsi == null) throw new NullReferenceException("WorkerSignin GetById returned null");
            }

            if (wa.workerSigninID == wsi.ID &&

                wsi.WorkAssignmentID == wa.ID &&

                wa.workerAssignedID == wsi.WorkerID) return true;
            else return false;
        }
        private void unassignWorkerSigninOnly(int wsiid, string user)
        {
            // get workersignin
            WorkerSignin wsi = wsiRepo.GetById(wsiid);
            if (wsi == null) throw new NullReferenceException("WSIID " + wsiid.ToString() +
                "returned a null Worker Signin record");
            //
            // Starting with WSI
            // 1. Does WSI.WAID point to anything?
            if (wsi.WorkAssignmentID == null) // No
                return; //nothing to clear
            //
            // 2. WSI.WAID points to something. Does something link back?
            if (matchWAWSI(null, wsi)) // yes
            {
                //Unassign both
                WorkAssignment wa = waRepo.GetById((int)wsi.WorkAssignmentID);
                unassignBoth(wa, wsi, user);
                return;
            }
            //
            // 3. If points to something, but something doesn't link back,
            //    does something match it's link?
            //if (wsi.WorkAssignmentID == null) throw new MacheteIntegrityException("Unassign called on non-assigned WorkerSignin");
            WorkAssignment linkedWA = waRepo.GetById((int)wsi.WorkAssignmentID);
            if (matchWAWSI(linkedWA, null))
            {
                //Something matches its link. My link to something assumed bad. 
                wsi.WorkAssignmentID = null;
                unitOfWork.Commit();
                return;
            }
            else throw new MacheteIntegrityException("Unassign found chain of mislinked records, starting with WSIID " + wsi.ID.ToString());
        }
        private void unassignBoth(int waid, int wsiid, string user)
        {
            WorkAssignment wa = waRepo.GetById(waid);
            WorkerSignin wsi = wsiRepo.GetById(wsiid);
            if (matchWAWSI(wa, wsi))
            {
                unassignBoth(wa, wsi, user);
            }
            else throw new Exception("The Worker and the Assignment do not match");
        }
        private void unassignBoth(WorkAssignment asmt, WorkerSignin signin, string user)
        {
            //Have both assignment and signin. 
            if (signin == null || asmt == null) throw new NullReferenceException("Signin and WorkAssignment are both null");
            //Try unassign with WorkerSignin only 
            signin.WorkAssignmentID = null;
            asmt.workerSigninID = null;
            asmt.workerAssignedID = null;
            asmt.updatedByUser(user);
            signin.updatedByUser(user);
            unitOfWork.Commit();
            log(asmt.ID, user, "WSIID:" + signin.ID + " Unassign successful");
        }
        public override WorkAssignment Create(WorkAssignment record, string user)
        {
            if (record.workOrder == null) throw new ArgumentNullException("workOrder object is null");
            record.workOrder.waPseudoIDCounter++;
            record.pseudoID = record.workOrder.waPseudoIDCounter;
            updateComputedValues(ref record);
            return base.Create(record, user);
        }

        public void Save(WorkAssignment wa, int? workerAssignedID, string user)
        {
            //check if workerAssigned changed; if so, Unassign
            int? origWorker = wa.workerAssignedID;
            if (workerAssignedID != origWorker)
                Unassign(wa.ID, wa.workerSigninID, user);
            //Save will link workerAssigned to Assignment record
            // if changed from orphan assignment
            if (workerAssignedID != null)
            {
                wa.workerAssigned = wRepo.GetById((int)wa.workerAssignedID);
            }
            wa.updatedByUser(user);
            updateComputedValues(ref wa);
            log(wa.ID, user, "WorkAssignment edited");
            unitOfWork.Commit();
        }

        public override void Save(WorkAssignment asmt, string user)
        {
            Save(asmt, null, user);
        }

        private void updateComputedValues(ref WorkAssignment record)
        {
            record.skillEN = lRepo.GetById(record.skillID).text_EN;
            record.skillES = lRepo.GetById(record.skillID).text_ES;
            record.minEarnings = (record.days * record.surcharge) + (record.hourlyWage * record.hours * record.days);
            record.maxEarnings = record.hourRange == null ? 0 : (record.days * record.surcharge) + (record.hourlyWage * (int)record.hourRange * record.days);
            var recordPseudoID = record.pseudoID ?? 0;
            var paperOrderNum = record.workOrder.paperOrderNum ?? 0;
            record.fullWAID = string.Format("{0,5:D5}-{1,2:D2}", paperOrderNum, recordPseudoID);
        }
    }
}