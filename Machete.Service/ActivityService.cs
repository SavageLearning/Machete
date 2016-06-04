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
        dataTableResult<Activity> GetIndexView(viewOptions o);
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
        public ActivityService(IActivityRepository repo,
            IActivitySigninService asServ,
            LookupCache lc,
            IUnitOfWork uow) : base(repo, uow)
        {
            this.logPrefix = "Activity";
            this.lcache = lc;
            this.asServ = asServ;
        }

        public dataTableResult<Activity> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Activity>();
            IQueryable<Activity> q = repo.GetAllQ();
            IEnumerable<Activity> e;
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

            e = q.AsEnumerable();
            if (!string.IsNullOrEmpty(o.sSearch))
                IndexViewBase.search(o, ref e, lcache);

            IndexViewBase.sortOnColName(o.sortColName, 
                                        o.orderDescending, 
                                        o.CI.TwoLetterISOLanguageName, 
                                        ref e,
                                        lcache);
            result.filteredCount = e.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = e.Skip(o.displayStart).Take(o.displayLength);
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
