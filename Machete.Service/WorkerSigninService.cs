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
using System;
using System.Linq;
using Machete.Data.Tenancy;

// ReSharper disable ReplaceWithSingleCallToCount

namespace Machete.Service
{
    public interface IWorkerSigninService : ISigninService<WorkerSignin>
    {
        void moveDown(int id, string user);
        void moveUp(int id, string user);
        dataTableResult<DTO.WorkerSigninList> GetIndexView(viewOptions o);
        WorkerSignin CreateSignin(int dwccardnum, DateTime dateforsignin, string user);
    }

    public class WorkerSigninService : SigninServiceBase<WorkerSignin>, IWorkerSigninService
    {
        private TimeZoneInfo ClientTimeZoneInfo { get; }

        public WorkerSigninService(
            IWorkerSigninRepository repo, 
            IWorkerService wServ,
            IImageService iServ,
            IWorkerRequestService wrServ,
            IUnitOfWork uow,
            IMapper map,
            IConfigService cfg,
            ITenantService tenantService
        ) : base(repo, wServ, iServ, wrServ, uow, map, cfg)
        {
            logPrefix = "WorkerSignin";
            ClientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        /// <summary>
        /// This method moves a worker down in numerical order in
        /// the daily ('lottery') list, and moves
        /// the proceeding (next) set member into their spot
        /// </summary>
        /// <param name="id">The Worker ID of the entry to be moved.</param>
        /// <param name="user">The username of the person making the request.</param>
        /// <returns>bool</returns>
        public void moveDown(int id, string user)
        {
            WorkerSignin wsiDown = repo.GetById(id); // 4
            DateTime date = wsiDown.dateforsignin;

            int nextID = (wsiDown.lottery_sequence ?? 0) + 1; // 5

            if (nextID == 1) return;
            //this can't happen with current GUI settings (10/10/2013)

            var firstOrDefault = repo.GetAllQ()
                .Where(up => up.lottery_sequence == nextID
                             && up.dateforsignin.Date == date.Date)
                .Select(g => g.ID).FirstOrDefault();
            WorkerSignin wsiUp = repo.GetById(firstOrDefault); //up.ID = 5

            int? spotInQuestion = wsiDown.lottery_sequence; // 4
            wsiDown.lottery_sequence = wsiUp.lottery_sequence; // 5
            wsiUp.lottery_sequence = spotInQuestion; // 4

            Save(wsiUp, user);
            Save(wsiDown, user);
        }

        /// <summary>
        /// This method moves a worker up in numerical order in
        /// the daily ('lottery') list, and moves
        /// the preceding (prior) set member into their spot
        /// </summary>
        /// <param name="id">The Worker ID of the entry to be moved.</param>
        /// <param name="user">The username of the person making the request.</param>
        /// <returns>bool</returns>
        public void moveUp(int id, string user)
        {
            WorkerSignin wsiUp = repo.GetById(id); // 4
            DateTime date = wsiUp.dateforsignin;

            int prevID = (wsiUp.lottery_sequence ?? 0) - 1; // 3

            if (prevID < 1) return;

            var firstOrDefault = repo.GetAllQ()
                .Where(up => up.lottery_sequence == prevID
                             && up.dateforsignin.Date == date.Date)
                .Select(g => g.ID).FirstOrDefault();
            WorkerSignin wsiDown = repo.GetById(
                                    firstOrDefault
                                 ); //down.lotSq = 3

            int? spotInQuestion = wsiDown.lottery_sequence; // 3
            wsiDown.lottery_sequence = wsiUp.lottery_sequence; // 4
            wsiUp.lottery_sequence = spotInQuestion; // 3

            Save(wsiUp, user);
            Save(wsiDown, user);
        }

        /// <summary>
        /// This method returns the view data for the Worker Signin class.
        /// </summary>
        /// <param name="o">View options from DataTables</param>
        /// <returns>dataTableResult WorkerSigninList</returns>
        public dataTableResult<DTO.WorkerSigninList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.WorkerSigninList>();
            IQueryable<WorkerSignin> q = repo.GetAllQ();

            var unused = q.Count();

            if (o.date != null)
            {
                var requestedDate = o.date.Value.DateBasedOn(ClientTimeZoneInfo); // 12:00:00 AM client time
                var endOfDay = requestedDate.AddDays(1).AddMilliseconds(-1);
                
                var qDates = q.Select(the => new { the.ID, the.dateforsignin }).ToList();
                
                var qIDs = qDates.Where(the => 
                    the.dateforsignin.DateTimeFrom(ClientTimeZoneInfo) >= requestedDate
                 && the.dateforsignin.DateTimeFrom(ClientTimeZoneInfo) <= endOfDay)
                    .Select(the => the.ID);
                
                q = q.Where(wsi => qIDs.Contains(wsi.ID));
                var blah = q.Count();
            }

            if (o.typeofwork_grouping != null) IndexViewBase.typeOfWork(o, ref q);
            IndexViewBase.waGrouping(o, ref q, wrServ);
            if (o.dwccardnum > 0) IndexViewBase.dwccardnum(o, ref q);
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            if (o.displayLength > 0)
            {
                result.query = q.ProjectTo<DTO.WorkerSigninList>(map.ConfigurationProvider)
                    .Skip(o.displayStart)
                    .Take(o.displayLength)
                    .AsEnumerable();
            }
            else
            {
                result.query = q.ProjectTo<DTO.WorkerSigninList>(map.ConfigurationProvider)
                    .AsEnumerable();
            }
            return result;

        }

        /// <summary>
        /// This method creates a worker signin entry. Must be implemented with try/catch.
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public virtual WorkerSignin CreateSignin(int dwccardnum, DateTime dateforsignin, string user)
        {
            //Search for worker with matching card number
            var worker = wServ.GetAll().FirstOrDefault(d => d.dwccardnum == dwccardnum);
            if (worker == null) throw new NullReferenceException("Card ID doesn't match a worker!");

            var workerSignins = repo.GetAllQ();

            var clientDate = dateforsignin.DateBasedOn(ClientTimeZoneInfo); // 12:00:00 AM client time
            var endOfClientDay = clientDate.AddDays(1).AddMilliseconds(-1);

            var foo = workerSignins.Select(the => new {the.ID, the.dwccardnum, the.dateforsignin});

            var qID = foo.FirstOrDefault(the => 
                the.dwccardnum == dwccardnum
                    && the.dateforsignin.DateTimeFrom(ClientTimeZoneInfo) >= clientDate
                    && the.dateforsignin.DateTimeFrom(ClientTimeZoneInfo) <= endOfClientDay
            )?.ID;
            if (qID != null) return workerSignins.FirstOrDefault(wsi => wsi.ID == qID);

            var signin = new WorkerSignin();
            signin.WorkerID = worker.ID;
            signin.dwccardnum = dwccardnum;
            signin.dateforsignin = dateforsignin; // the client has spoken, we have universalized, let it in!
            signin.memberStatusID = worker.memberStatusID;
            signin.lottery_sequence =
                workerSignins.Where(p => p.dateforsignin.Date == dateforsignin.Date).Count() + 1;
            return Create(signin, user);
        }
    }
}
