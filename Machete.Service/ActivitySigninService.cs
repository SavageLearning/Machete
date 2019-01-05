#region COPYRIGHT
// File:     ActivitySigninService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivitySigninService : ISigninService<ActivitySignin>
    {
        dataTableResult<DTO.ActivitySigninList> GetIndexView(viewOptions o);
        Worker CreateSignin(ActivitySignin signin, string user);
        IQueryable<ActivitySignin> GetManyByPersonID(int actID, int perID);
        ActivitySignin GetByPersonID(int actID, int perID);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivitySigninService : SigninServiceBase<ActivitySignin>, IActivitySigninService
    {

        private readonly IPersonService pServ;
        private readonly IActivitySigninRepository asiRepo;
        public ActivitySigninService(
            IActivitySigninRepository asiRepo,
            IWorkerService wServ,
            IPersonService pServ,
            IImageService iServ,
            IWorkerRequestService wrServ,
            IUnitOfWork uow,
            IMapper map, 
            IConfigService cfg)
            : base(asiRepo, wServ, iServ, wrServ, uow, map, cfg)
        {
            this.logPrefix = "ActivitySignin";
            this.pServ = pServ;
            this.asiRepo = asiRepo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dataTableResult<DTO.ActivitySigninList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.ActivitySigninList>();
            IQueryable<ActivitySignin> q = repo.GetAllQ();
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);
            if (o.personID > 0) IndexViewBase.GetAssociated(o.personID, ref q);
            if (o.activityID != null) q = q.Where(p => p.activityID == o.activityID);
            //            
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            if (o.displayLength > 0)
            {
                result.query = q.ProjectTo<DTO.ActivitySigninList>(map.ConfigurationProvider)
                    .Skip(o.displayStart)
                    .Take(o.displayLength)
                    .AsEnumerable();
            }
            else
            {
                result.query = q.ProjectTo<DTO.ActivitySigninList>(map.ConfigurationProvider)
                    .AsEnumerable();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signin"></param>
        /// <param name="user"></param>
        public Worker CreateSignin(ActivitySignin s, string user)
        {
            Person p = null;
            Worker w = null;
            int count = 0;
            if (s.activityID == 0) throw new ArgumentOutOfRangeException("ActivitySignin's activityID is zero");
            //
            // Two possible keys to find person record: personID and dwccardnum
            // card signins will only have dwccardnums. AssignLists will only have personID.
            if (s.personID == null && s.dwccardnum == 0)
                throw new NullReferenceException("ActivitySignin's personID and dwccardnum are both null");
            if (s.personID < 1 && s.dwccardnum < 1)
                throw new NullReferenceException("ActivitySignin's personID and dwccardnum are invalid values");

            if (s.personID > 0)
            {
                p = pServ.Get((int)s.personID);
                w = p.Worker;
                if (w == null) throw new NullReferenceException("Worker object is null. A Worker record must exist to assign a person to activities.");
                if (w.dwccardnum == 0) throw new ArgumentOutOfRangeException("Membership ID in Worker record is zero.");
                s.dwccardnum = w.dwccardnum;
            }
            else //assuming dwccardnum pressent
            {
                //
                //TODO: GENERALIZE away from 5 digit dwccardnum
                w = wServ.GetByMemberID(s.dwccardnum);
                if (w == null) throw new NullReferenceException("card ID doesn't match a worker");
                p = w.Person;
                
                //
                if (w.isExpelled || w.isSanctioned) return w;
            }

            if (p == null) throw new NullReferenceException("Person record null");
            s.personID = p.ID;
            s.memberStatusID = p.Worker.memberStatusID;
            //
            //Search for duplicate signin for the same day
            count = repo.GetAllQ().Count(q => q.activityID == s.activityID &&
                                              q.personID == p.ID);
            if (count == 0)
            {
                s.timeZoneOffset = Convert.ToDouble(cfg.getConfig(Cfg.TimeZoneDifferenceFromPacific));
                Create(s, user);
            }
            return w;
        }

        public IQueryable<ActivitySignin> GetManyByPersonID(int actID, int perID)
        {
            return repo.GetManyQ(az => az.activityID == actID && az.personID == perID);
        }

        public ActivitySignin GetByPersonID(int actID, int perID)
        {
            return asiRepo.GetByPersonID(actID,perID);
        }
    }
}
