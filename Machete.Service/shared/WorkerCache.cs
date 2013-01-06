#region COPYRIGHT
// File:     WorkerCache.cs
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
using Machete.Data;
using Machete.Domain;
using System.Data.Entity;
using System.Runtime.Caching;

namespace Machete.Service
{
    public class WorkerCache
    {
        private static MacheteContext DB { get; set; }
        private static CacheItem DbCache { get; set; }        
        private static ObjectCache cache;
        //
        //
        public static void Initialize(MacheteContext db)
        {
            cache = MemoryCache.Default;
            DB = db;
            FillCache();            
        }
        //
        //
        private static void FillCache()
        {
            IQueryable<Worker> workersRaw = DB.Workers;
            IQueryable<Worker> workersNoTracking = workersRaw.AsNoTracking();
            IQueryable<Worker> workersIncluded = workersNoTracking.Include(p => p.Person);
            IEnumerable<Worker> workers = workersIncluded.ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5));
            CacheItem wCacheItem = new CacheItem("workerCache", workers);
            cache.Set(wCacheItem, policy);
            //DB.Dispose();
            ReactivateMembers(DB);
            ExpireMembers(DB);            
        }
        //
        //
        private static void FillCache(MacheteContext db)
        {
            DB = db;
            FillCache();
        }
        //
        //
        public static IEnumerable<Worker> getCache()
        {
            CacheItem wCacheItem = cache.GetCacheItem("workerCache");
            if (wCacheItem == null) 
            {
                FillCache();
                wCacheItem = cache.GetCacheItem("workerCache");
            }
            return wCacheItem.Value as IEnumerable<Worker>;
        }
        //
        //
        public static void Refresh(MacheteContext db)
        {
            FillCache(db);            
        }
        /// <summary>
        /// Expires active workers based on expiration date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record expired</returns>
        public static bool ExpireMembers(MacheteContext db)
        {
            bool rtn = false;
            IQueryable<Worker> list = db.Workers
                .Where(w => w.memberexpirationdate < DateTime.Now && 
                    w.memberStatus == Worker.iActive);
            //
            if (list.Count() > 0) rtn = true;
            //
            foreach (Worker wkr in list)
            {
                wkr.memberStatus = Worker.iExpired;                
            }
            db.SaveChanges();
            return rtn;
        }
        /// <summary>
        /// Reactivates sanctioned workers based on reactivation date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record reactivated</returns>
        public static bool ReactivateMembers(MacheteContext db) 
        {
            bool rtn = false;
            IQueryable<Worker> list = db.Workers
                .Where(w => w.memberReactivateDate != null &&
                    w.memberReactivateDate < DateTime.Now &&
                    w.memberStatus == Worker.iSanctioned);
            //
            if (list.Count() > 0) rtn = true;
            //
            foreach (Worker wkr in list)
            {
                wkr.memberStatus = Worker.iActive;
            }
            db.SaveChanges();
            return rtn;
        }
    }
}