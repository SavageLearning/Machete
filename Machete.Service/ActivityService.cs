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
    public class ActivityService : ServiceBase2<Activity>, IActivityService
    {
        private IActivitySigninService asServ;
        private readonly IMapper map;
        public ActivityService(IDatabaseFactory db,
            IActivitySigninService asServ,
            IMapper map) : base(db)
        {
            this.logPrefix = "Activity";
            this.map = map;
            this.asServ = asServ;
   
        }

        public override Activity Create(Activity record, string user)
        {
            updateComputedFields(ref record);
            var result = base.Create(record, user);
            db.SaveChanges();
            db.Dispose();
            return result;
        }

        public override void Save(Activity record, string user)
        {
            updateComputedFields(ref record);
            base.Save(record, user);
        }

        private void updateComputedFields(ref Activity record)
        {
            record.nameEN = lookupTextByID(record.nameID, "EN");
            record.nameES = lookupTextByID(record.nameID, "ES");
            record.typeEN = lookupTextByID(record.typeID, "EN");
            record.typeES = lookupTextByID(record.typeID, "ES");
        }

        public dataTableResult<DTO.ActivityList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.ActivityList>();
            IQueryable<Activity> q = dbset.AsNoTracking();
            

            if (o.personID > 0 && o.attendedActivities == false)
                IndexViewBase.getUnassociated(o.personID, ref q, db);
            if (o.personID > 0 && o.attendedActivities == true)
                IndexViewBase.getAssociated(o.personID, ref q, db);
            if (!o.authenticated)
            {
                if (o.date == null) o.date = DateTime.Now;
                IndexViewBase.unauthenticatedView((DateTime)o.date, ref q);
            }

            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.totalCount = TotalCount();
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
                Activity act = dbset.Find(aID);
                if (act == null) throw new Exception("Activity from list is null");
                int matches = db.Set<ActivitySignin>()
                    .Where(az => az.activityID == aID && az.personID == personID).Count();

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
                Activity act = dbset.Find(aID);
                if (act == null) throw new Exception("Activity from list is null");

                ActivitySignin asi = db.Set<ActivitySignin>()
                    .Where(az => az.activityID.Equals(aID) && az.personID.Equals(personID)).FirstOrDefault();


                if (asi == null) throw new NullReferenceException("ActivitySignin.GetByPersonID returned null");
                asServ.Delete(asi.ID, user);
            }
        }
    }
   
}
