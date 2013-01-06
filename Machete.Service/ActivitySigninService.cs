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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Objects;
using System.ComponentModel;

namespace Machete.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivitySigninService : ISigninService<ActivitySignin>
    {
        dataTableResult<asiView> GetIndexView(viewOptions o);
        Worker CreateSignin(ActivitySignin signin, string user);
        IQueryable<ActivitySignin> GetManyByPersonID(int actID, int perID);
        ActivitySignin GetByPersonID(int actID, int perID);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivitySigninService : SigninServiceBase<ActivitySignin>, IActivitySigninService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="wRepo"></param>
        /// <param name="pRepo"></param>
        /// <param name="iRepo"></param>
        /// <param name="wrRepo"></param>
        /// <param name="uow"></param>
        private readonly IPersonRepository pRepo;
        public ActivitySigninService(IActivitySigninRepository repo,
                                   IWorkerRepository wRepo,
                                   IPersonRepository pRepo,
                                   IImageRepository iRepo,
                                   IWorkerRequestRepository wrRepo,
                                   IUnitOfWork uow)
            : base(repo, wRepo, iRepo, wrRepo, uow)
        {
            this.logPrefix = "ActivitySignin";
            this.pRepo = pRepo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dataTableResult<asiView> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<asiView>();
            IQueryable<ActivitySignin> q = repo.GetAllQ();
            IEnumerable<ActivitySignin> e;
            IEnumerable<asiView> eSIV;
            //
            if (o.date != null) IndexViewBase.diffDays(o, ref q);
            // WHERE on activityID
            if (o.personID > 0)
                IndexViewBase.GetAssociated(o.personID, ref q);
            if (o.activityID != null)
                q = q.Where(p => p.activityID == o.activityID);
            //            
            e = q.ToList();
            if (!string.IsNullOrEmpty(o.sSearch))
                IndexViewBase.search(o, ref e);

            eSIV = e.Join(WorkerCache.getCache(),
                            s => s.dwccardnum,
                            w => w.dwccardnum,
                            (s, w) => new { s, w }
                            )
                    .Select(z => new asiView(z.w.Person, z.s));

            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, o.CI.TwoLetterISOLanguageName, ref eSIV);
            result.filteredCount = eSIV.Count();
            result.totalCount = repo.GetAllQ().Count();
            if ((int)o.displayLength >= 0)
                result.query = eSIV.Skip((int)o.displayStart).Take((int)o.displayLength);
            else
                result.query = eSIV;           
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
                p = pRepo.GetById((int)s.personID);
                w = p.Worker;
                if (w == null) throw new NullReferenceException("Worker object is null. A Worker record must exist to assign a person to activities.");
                if (w.dwccardnum == 0) throw new ArgumentOutOfRangeException("Membership ID in Worker record is zero.");
                s.dwccardnum = w.dwccardnum;
            }
            else //assuming dwccardnum pressent
            {
                //
                //TODO: GENERALIZE away from 5 digit dwccardnum
                w = wRepo.Get(ww => ww.dwccardnum == s.dwccardnum);
                if (w == null) throw new NullReferenceException("card ID doesn't match a worker");
                p = w.Person;
                
                //
                if (w.isExpelled || w.isSanctioned) return w;
            }

            if (p == null) throw new NullReferenceException("Person record null");
            s.personID = p.ID;
            //
            //Search for duplicate signin for the same day
            count = repo.GetAllQ().Count(q => q.activityID == s.activityID &&
                                              q.personID == p.ID);
            if (count == 0) Create(s, user);
            return w;
        }

        public IQueryable<ActivitySignin> GetManyByPersonID(int actID, int perID)
        {
            return repo.GetManyQ(az => az.activityID == actID && az.personID == perID);
        }

        public ActivitySignin GetByPersonID(int actID, int perID)
        {
            return repo.Get(az => az.activityID == actID && az.personID == perID);
        }
    }
}
