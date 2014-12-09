#region COPYRIGHT
// File:     WorkerSigninService.cs
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
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace Machete.Service
{
    public interface IWorkerSigninService : ISigninService<WorkerSignin>
    {
        WorkerSignin GetSignin(int dwccardnum, DateTime date);
        int GetNextLotterySequence(DateTime date);
        bool clearLottery(int id, string user);
        bool moveDown(int id, string user);
        bool moveUp(int id, string user);
        bool sequenceLottery(DateTime date, string user);
        bool listDuplicate(DateTime date, string user);
        string signinDuplicate(DateTime date, string user);
        dataTableResult<wsiView> GetIndexView(viewOptions o);
        void CreateSignin(WorkerSignin signin, string user);
    }

    public class WorkerSigninService : SigninServiceBase<WorkerSignin>, IWorkerSigninService
    {
        //
        public WorkerSigninService(
            IWorkerSigninRepository repo, 
            IWorkerRepository wRepo,
            IImageRepository iRepo,
            IWorkerRequestRepository wrRepo,
            IWorkerCache wc, 
            IUnitOfWork uow)
            : base(repo, wRepo, iRepo, wrRepo, wc, uow)
        {
            this.logPrefix = "WorkerSignin";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwccardnum"></param>
        /// <param name="date"></param>
        /// <returns>WorkerSignin</returns>
        public WorkerSignin GetSignin(int dwccardnum, DateTime date)
        {
            return repo.GetManyQ().FirstOrDefault(r => r.dwccardnum == dwccardnum &&
                            DbFunctions.DiffDays(r.dateforsignin, date) == 0 ? true : false);
        }
        /// <summary>
        /// This method counts the current lottery (daily list) entries for the day and increments by one.
        /// </summary>
        /// <param name="date">The date for sequencing.</param>
        /// <returns>int</returns>
        public int GetNextLotterySequence(DateTime date)
        {
            return repo.GetAllQ().Where(p => p.lottery_timestamp != null && 
                                        DbFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                                 .Count() + 1;
        }

        /// <summary>
        /// This method moves a worker down in numerical order in
        /// the daily ('lottery') list, and moves
        /// the proceeding (next) set member into their spot
        /// </summary>
        /// <param name="id">The Worker ID of the entry to be moved.</param>
        /// <param name="user">The username of the person making the request.</param>
        /// <returns>bool</returns>
        public bool moveDown(int id, string user)
        {
            WorkerSignin wsiDown = repo.GetById(id); // 4
            DateTime date = wsiDown.dateforsignin;

            int nextID = (wsiDown.lottery_sequence ?? 0) + 1; // 5

            if (nextID == 1)
                return false;
            //this can't happen with current GUI settings (10/10/2013)

            WorkerSignin wsiUp = repo.GetById(
                                    repo.GetAllQ()
                                        .Where(up => up.lottery_sequence == nextID
                                                  && DbFunctions.DiffDays(up.dateforsignin, date) == 0)
                                        .Select(g => g.ID).FirstOrDefault()
                                 ); //up.ID = 5

            int? spotInQuestion = wsiDown.lottery_sequence; // 4
            DateTime? timeInQuestion = wsiDown.lottery_timestamp;
            wsiDown.lottery_sequence = wsiUp.lottery_sequence; // 5
            wsiDown.lottery_timestamp = wsiUp.lottery_timestamp;
            wsiUp.lottery_sequence = spotInQuestion; // 4
            wsiUp.lottery_timestamp = timeInQuestion; 

            Save(wsiUp, user);
            Save(wsiDown, user);

            return true;
        }

        /// <summary>
        /// This method moves a worker up in numerical order in
        /// the daily ('lottery') list, and moves
        /// the preceding (prior) set member into their spot
        /// </summary>
        /// <param name="id">The Worker ID of the entry to be moved.</param>
        /// <param name="user">The username of the person making the request.</param>
        /// <returns>bool</returns>
        public bool moveUp(int id, string user)
        {
            WorkerSignin wsiUp = repo.GetById(id); // 4
            DateTime date = wsiUp.dateforsignin;

            int prevID = (wsiUp.lottery_sequence ?? 0) - 1; // 3

            if (prevID < 1)
                return false;

            WorkerSignin wsiDown = repo.GetById(
                                    repo.GetAllQ()
                                        .Where(up => up.lottery_sequence == prevID
                                                  && DbFunctions.DiffDays(up.dateforsignin, date) == 0)
                                        .Select(g => g.ID).FirstOrDefault()
                                 ); //down.lotSq = 3

            int? spotInQuestion = wsiDown.lottery_sequence; // 3
            DateTime? timeInQuestion = wsiDown.lottery_timestamp;
            wsiDown.lottery_sequence = wsiUp.lottery_sequence; // 4
            wsiDown.lottery_timestamp = wsiUp.lottery_timestamp;
            wsiUp.lottery_sequence = spotInQuestion; // 3
            wsiUp.lottery_timestamp = timeInQuestion; // down.lotSq = 4

            Save(wsiUp, user);
            Save(wsiDown, user);

            return true;
        }

        /// <summary>
        /// This method clears an entry from the daily ('lottery') list
        /// along with the timestamp and then resequences the list.
        /// </summary>
        /// <param name="id">The worker ID of the worker to be cleared from the lottery.</param>
        /// <param name="user">The usename of the user making the request.</param>
        /// <returns>bool</returns>
        public bool clearLottery(int id, string user)
        {
            WorkerSignin wsi = repo.GetById(id);
            DateTime date = wsi.dateforsignin;
            wsi.lottery_sequence = null;
            wsi.lottery_timestamp = null;
            Save(wsi, user);
            if (date != null)
                sequenceLottery(date, user);
            return true;
        }
        /// <summary>
        /// A method that resequences the ordinal lottery numbers.
        /// </summary>
        /// <param name="date">Date to resequence numbers</param>
        /// <param name="user">Username of the user making the request.</param>
        /// <returns>bool</returns>
        public bool sequenceLottery(DateTime date, string user)
        {
            IEnumerable<WorkerSignin> signins;
            int i = 0;
            // select and sort by timestamp
            signins = repo.GetManyQ().Where(p => p.lottery_timestamp != null &&
                                           DbFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                                    .OrderBy(p => p.lottery_timestamp)
                                    .AsEnumerable();
            // reset sequence number
            foreach (WorkerSignin wsi in signins)
            {
                i++;
                wsi.lottery_sequence = i;                
            }
            // no username logging as of now
            uow.Commit();
            return true;
        }

        /// <summary>
        /// This method Duplicates the Daily ('lottery') list for the previous day, 
        /// minus workers who did not sign in for the current day and 
        /// workers who were assigned to a job for the previous day
        /// </summary>
        /// <param name="date">The date for which the list should be duplicated.</param>
        /// <param name="user">The username of the user making the request.</param>
        /// <returns>bool</returns>
        public bool listDuplicate(DateTime date, string user)
        {
            int i = 0;
            DateTime yesterday = new DateTime(date.Year, date.Month, (date.Day - 1), date.Hour, date.Minute, date.Second);
            IEnumerable<WorkerSignin> todayListSignins;
            IEnumerable<WorkerSignin> yesterdaySignins;
            IEnumerable<WorkerSignin> todayDupeSignins;
            IEnumerable<WorkerSignin> dupes;

            // Get today's signins. Make sure that anyone who already has been assigned a lottery number
            // gets a new one in the same order.
            todayListSignins = repo.GetAllQ()
                .Where(p => p.lottery_sequence != null
                         && DbFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                .OrderBy(p => p.lottery_sequence)
                .AsEnumerable();
            // reset sequence number
            foreach (WorkerSignin wsi in todayListSignins)
            {
                i++;
                wsi.lottery_sequence = i;
                wsi.lottery_timestamp = DateTime.Now;
                wsi.Updatedby = user;
            }

            //get yesterday's lottery/list members, 
            //as long as they weren't dispatched
            yesterdaySignins = repo.GetAllQ()
                .Where(p => p.lottery_sequence != null 
                         && p.WorkAssignmentID == null
                         && DbFunctions.DiffDays(p.dateforsignin, yesterday) == 0 ? true : false)
                .OrderBy(p => p.lottery_sequence)
                .AsEnumerable();

            //gets any signin for today that "should" be on the duplicate lottery/list
            //i.e., they are not currently on the list, were on the list yesterday,
            //were not dispatched yesterday and have not been dispatched today
            todayDupeSignins = repo.GetManyQ() //with tracing
                .Where(p => p.WorkAssignmentID == null
                         && p.lottery_sequence == null
                         && DbFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                .OrderBy(o => o.dateforsignin)
                .AsEnumerable();

            //thank you Jimmy
            dupes = todayDupeSignins
                .Join(yesterdaySignins, to => to.WorkerID, ye => ye.WorkerID, (to, ye) => new { to, seq = ye.lottery_sequence })
                .Select(g => g.to )
                .OrderBy(o => o.lottery_sequence);

            //good god that works
            foreach (WorkerSignin wsi in dupes)
            {
                i++;
                wsi.lottery_sequence = i;
                wsi.lottery_timestamp = DateTime.Now;
                wsi.Updatedby = user;
                Save(wsi, user);
            }
            
            //uow.Commit();
            return true;
        }


        /// <summary>
        /// This method Duplicates the Daily ('lottery') list for the previous day, 
        /// minus workers who did not sign in for the current day and 
        /// workers who were assigned to a job for the previous day
        /// </summary>
        /// <param name="date">The date for which the list should be duplicated.</param>
        /// <param name="user">The username of the user making the request.</param>
        /// <returns>string</returns>
        public string signinDuplicate(DateTime date, string user)
        {
            DateTime yesterday = date.AddDays(-1);
            IEnumerable<WorkerSignin> todayListSignins;
            IEnumerable<WorkerSignin> yesterdaySignins;
            StringBuilder iWasNotSignedIn = new StringBuilder();

            // Get today's signins. If anyone has already been signed in, this feature will hold those records.
            todayListSignins = repo.GetAllQ()
                .Where(p => DbFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                .OrderBy(p => p.dateforsignin)
                .AsEnumerable();

            //get yesterday's signins, 
            //as long as they weren't dispatched
            //and haven't already been signed in
            yesterdaySignins = repo.GetManyQ()
                .Where(p => p.WorkAssignmentID == null
                         && DbFunctions.DiffDays(p.dateforsignin, yesterday) == 0 ? true : false
                         && !todayListSignins.Select(t => t.WorkerID).Contains(p.WorkerID))
                .OrderBy(p => p.dateforsignin)
                .AsEnumerable()
                .ToList();

                //then, create signins for all of them.
            foreach (WorkerSignin wsi in yesterdaySignins)
            {
                //we're going to need some objects
                Worker oompaLoompa = wRepo.GetAllQ().FirstOrDefault(s => s.dwccardnum == wsi.dwccardnum);
                WorkerSignin dupadedoo = new WorkerSignin();
                // The card don't need no swipe
                dupadedoo.dwccardnum = wsi.dwccardnum;
                dupadedoo.dateforsignin = new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                dupadedoo.memberStatus = oompaLoompa.memberStatus;
                try
                {
                    // Let CreateSignin do the rest
                    CreateSignin(dupadedoo, user);
                }
                catch (InvalidOperationException eek)
                {
                    string addMeToTheList = oompaLoompa.dwccardnum.ToString() + " " + eek.Message + "\n";
                    iWasNotSignedIn.Append(addMeToTheList);
                }
            }
            if (iWasNotSignedIn.Length > 0) return iWasNotSignedIn.ToString();
            else return "Success!";
        }


        /// <summary>
        /// This method returns the view data for the Worker Signin class.
        /// </summary>
        /// <param name="o">View options from DataTables</param>
        /// <returns>dataTableResult wsiView</returns>
        public dataTableResult<wsiView> GetIndexView(viewOptions o)
        {
            //
            var result = new dataTableResult<wsiView>();
            IQueryable<WorkerSignin> q = repo.GetAllQ();
            IEnumerable<WorkerSignin> e;
            IEnumerable<wsiView> eSIV;
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);                
            //
            if (o.typeofwork_grouping != null)
                IndexViewBase.typeOfWork(o, ref q);
            // 
            // wa_grouping
            IndexViewBase.waGrouping(o, ref q, wrRepo);
            //
            // dwccardnum populated
            if (o.dwccardnum > 0) IndexViewBase.dwccardnum(o, ref q);
            e = q.ToList();
            if (!string.IsNullOrEmpty(o.sSearch))
                IndexViewBase.search(o, ref e, wcache.GetCache());
            var cache = wcache.GetCache();
            eSIV = e.Join(cache, 
                            s => s.dwccardnum, 
                            w => w.dwccardnum, 
                            (s, w) => new { s, w }
                            )
                    .Select(z => new wsiView( z.w.Person, z.s ));

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref eSIV);
            result.filteredCount = eSIV.Count();
            result.totalCount = repo.GetAllQ().Count();
            if ((int)o.displayLength >= 0)
                result.query = eSIV.Skip((int)o.displayStart).Take((int)o.displayLength);
            else
                result.query = eSIV;
            return result;

        }
        /// <summary>
        /// This method creates a worker signin entry. Must be implemented with try/catch.
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public virtual void CreateSignin(WorkerSignin signin, string user)
        {
            //Search for worker with matching card number
            Worker wfound;
            wfound = wRepo.GetAllQ().FirstOrDefault(s => s.dwccardnum == signin.dwccardnum);
            if (wfound != null)
            {
                signin.WorkerID = wfound.ID;
            }
            //Search for duplicate signin for the same day
            int sfound = 0;
            var srepo = repo.GetAllQ();
            
            try 
            { 
            sfound = srepo
                .Select(s => new { s.dwccardnum, s.dateforsignin })
                .Where(t => DbFunctions.TruncateTime(t.dateforsignin) == signin.dateforsignin.Date
                    && t.dwccardnum == signin.dwccardnum)
                .Count();
            }
            catch(NotSupportedException _nse)
            {
                Console.WriteLine(_nse.Message);
                // Must use the following in a Mocking environment:
                sfound = srepo
                    .Select(s => new { s.dwccardnum, s.dateforsignin })
                    .Where(t => t.dateforsignin.Date == signin.dateforsignin.Date
                        && t.dwccardnum == signin.dwccardnum)
                    .Count();
            }

            if (sfound == 0) Create(signin, user);
            else throw new InvalidOperationException("has already been signed in!");
        }
    }
}
