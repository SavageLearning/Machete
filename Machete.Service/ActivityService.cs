#region COPYRIGHT
// File:     ActivityService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
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
    public interface IActivityService : IService<Activity>
    {
        dataTableResult<DTO.ActivityList> GetIndexView(viewOptions o);
        void AssignList(int personID, List<int> actList, string user);
        void UnassignList(int personID, List<int> actList, string user);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityService : ServiceBase<Activity>, IActivityService
    {
        private IActivitySigninService asServ;
        private LookupCache lcache;
        private readonly IMapper map;
        public ActivityService(IActivityRepository repo,
            IActivitySigninService asServ,
            LookupCache lc,
            IUnitOfWork uow,
            IMapper map) : base(repo, uow)
        {
            this.logPrefix = "Activity";
            this.lcache = lc;
            this.map = map;
            this.asServ = asServ;
        }

        public override Activity Create(Activity record, string user)
        {
            record.nameEN = lcache.textByID(record.nameID, "EN");
            record.nameES = lcache.textByID(record.nameID, "ES");
            record.typeEN = lcache.textByID(record.typeID, "EN");
            record.typeES = lcache.textByID(record.typeID, "ES");
            return base.Create(record, user);
        }

        public override void Save(Activity record, string user)
        {
            record.nameEN = lcache.textByID(record.nameID, "EN");
            record.nameES = lcache.textByID(record.nameID, "ES");
            record.typeEN = lcache.textByID(record.typeID, "EN");
            record.typeES = lcache.textByID(record.typeID, "ES");
            base.Save(record, user);
        }

        public dataTableResult<DTO.ActivityList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.ActivityList>();
            IQueryable<Activity> q = repo.GetAllQ();
            var asRepo = (IActivitySigninRepository)asServ.GetRepo();

            if (o.personID > 0 && o.attendedActivities == false)
                IndexViewBase.getUnassociated(o.personID, ref q, repo, asRepo);
            if (o.personID > 0 && o.attendedActivities == true)
                IndexViewBase.getAssociated(o.personID, ref q, asRepo);
            if (!o.authenticated)
            {
                if (o.date == null) o.date = DateTime.Now;
                IndexViewBase.unauthenticatedView((DateTime)o.date, ref q);
            }

            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = q.ProjectTo<DTO.ActivityList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            return result;
        }

        public void AssignList(int personID, List<int> actList, string user)
        {
            foreach (int aID in actList)
            {
                Activity act = repo.GetById(aID);
                if (act == null) throw new Exception("Activity from list is null");
                int matches = asServ.GetManyByPersonID(aID, personID).Count();

                if (matches == 0)
                {
                    asServ.CreateSignin(new ActivitySignin
                    {
                        activityID = aID,
                        personID = personID,
                        dateforsignin = act.dateStart
                    }, user);
                }
            }
        }

        public void UnassignList(int personID, List<int> actList, string user)
        {
            foreach (int aID in actList)
            {
                Activity act = repo.GetById(aID);
                if (act == null) throw new Exception("Activity from list is null");
                ActivitySignin asi = asServ.GetByPersonID(aID, personID);
                if (asi == null) throw new NullReferenceException("ActivitySignin.GetByPersonID returned null");
                asServ.Delete(asi.ID, user);
            }
        }
    }
   
}
