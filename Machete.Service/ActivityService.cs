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
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Machete.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Machete.Data.Tenancy;

namespace Machete.Service
{
    public interface IActivityService : IService<Activity>
    {
        dataTableResult<ActivityList> GetIndexView(viewOptions o);
        void AssignList(int personID, List<int> actList, string user);
        void UnassignList(int personID, List<int> actList, string user);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityService : ServiceBase2<Activity>, IActivityService
    {
        private IActivitySigninService asServ;
        private TimeZoneInfo _clientTimeZoneInfo;
        public ActivityService(
            IDatabaseFactory db,
            IActivitySigninService asServ,
            ITenantService tenantService,
            IMapper map) : base(db, map)
        {
            this.asServ = asServ;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        public override Activity Create(Activity record, string user)
        {
            updateComputedFields(ref record);
            var result = base.Create(record, user);
            db.SaveChanges();
            return result;
        }

        public override void Save(Activity record, string user)
        {
            updateComputedFields(ref record);
            base.Save(record, user);
            db.SaveChanges();
        }

        private void updateComputedFields(ref Activity record)
        {
            record.nameEN = lookupTextByID(record.nameID, "EN");
            record.nameES = lookupTextByID(record.nameID, "ES");
            record.typeEN = lookupTextByID(record.typeID, "EN");
            record.typeES = lookupTextByID(record.typeID, "ES");
        }

        public dataTableResult<ActivityList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.ActivityList>();
            IQueryable<Activity> q = dbset.AsNoTracking();
            
            if (o.personID > 0 && o.attendedActivities == false)
                IndexViewBase.getUnassociated(o.personID, ref q, db);
            if (o.personID > 0 && o.attendedActivities == true)
                IndexViewBase.getAssociated(o.personID, ref q, db);

            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, _clientTimeZoneInfo ,ref q);

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
            foreach (int activityID in actList)
            {
                Activity act = dbset.Find(activityID);
                if (act == null) throw new Exception("Activity from list is null");
                int matches = db
                    .Set<ActivitySignin>().Count(az => az.activityID == activityID && az.personID == personID);

                if (matches == 0)
                {
                    asServ.CreateSignin(new ActivitySignin
                    {
                        activityID = activityID,
                        personID = personID,
                        dateforsignin = act.dateStart
                    }, user);
                }
            }
        }

        public void UnassignList(int personID, List<int> actList, string user)
        {
            foreach (int activityID in actList)
            {
                Activity act = dbset.Find(activityID);
                if (act == null) throw new Exception("Activity from list is null");

                ActivitySignin asi = db
                    .Set<ActivitySignin>().FirstOrDefault(az => az.activityID.Equals(activityID) && az.personID.Equals(personID));


                if (asi == null) throw new NullReferenceException("ActivitySignin.GetByPersonID returned null");
                asServ.Delete(asi.ID, user);
            }
        }
    }

}
