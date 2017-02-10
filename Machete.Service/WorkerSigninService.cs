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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Machete.Service
{
    public interface IWorkerSigninService : ISigninService<WorkerSignin>
    {
        WorkerSignin GetSignin(int dwccardnum, DateTime date);
        int GetNextLotterySequence(DateTime date);
        bool moveDown(int id, string user);
        bool moveUp(int id, string user);
        dataTableResult<WorkerSigninList> GetIndexView(viewOptions o);
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
            IUnitOfWork uow,
            IMapper map)
            : base(repo, wRepo, iRepo, wrRepo, uow, map)
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
        /// This method returns the view data for the Worker Signin class.
        /// </summary>
        /// <param name="o">View options from DataTables</param>
        /// <returns>dataTableResult WorkerSigninList</returns>
        public dataTableResult<WorkerSigninList> GetIndexView(viewOptions o)
        {
            //
            var result = new dataTableResult<DTO.WorkerSigninList>();
            IQueryable<WorkerSignin> q = repo.GetAllQ();
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);                
            if (o.typeofwork_grouping != null) IndexViewBase.typeOfWork(o, ref q);
            IndexViewBase.waGrouping(o, ref q, wrRepo);
            if (o.dwccardnum > 0) IndexViewBase.dwccardnum(o, ref q);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = q.ProjectTo<DTO.WorkerSigninList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
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

            sfound = srepo.Select(s => new { s.dwccardnum, s.dateforsignin })
                        .Where(t => DbFunctions.TruncateTime(t.dateforsignin) == signin.dateforsignin.Date
                            && t.dwccardnum == signin.dwccardnum)
                        .Count();

            if (sfound == 0) Create(signin, user);
            else throw new InvalidOperationException("has already been signed in!");
        }

    }
}
