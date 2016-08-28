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
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Machete.Data
{
    public interface IWorkerCache
    {
        void Refresh();
        IEnumerable<Worker> GetCache();
    }

    public class WorkerCache : IWorkerCache
    {
        private Func<IDatabaseFactory> _factory { get; set; }
        private CacheItem DbCache { get; set; }        
        private ObjectCache cache;
        //
        //
        public WorkerCache(Func<IDatabaseFactory> factory)
        {
            cache = MemoryCache.Default;
            _factory = factory;
            FillCache();            
        }

        public void Dispose()
        {
            _factory = null;
            DbCache = null;
            cache = null;
        }
        //
        //
        private MacheteContext GetDB()
        {
            return _factory().Get();
        }
        //
        //
        private void FillCache()
        {
            IQueryable<Worker> workersRaw = GetDB().Workers;
            IQueryable<Worker> workersNoTracking = workersRaw.AsNoTracking();
            IQueryable<Worker> workersIncluded = workersNoTracking.Include(p => p.Person);
            IEnumerable<Worker> workers = workersIncluded.ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5));
            CacheItem wCacheItem = new CacheItem("workerCache", workers);
            cache.Set(wCacheItem, policy);
            ReactivateMembers();
            ExpireMembers();            
        }
        //
        //
        public IEnumerable<Worker> GetCache()
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
        public void Refresh()
        {
            FillCache();            
        }
        /// <summary>
        /// Expires active workers based on expiration date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record expired</returns>
        public bool ExpireMembers()
        {
            bool rtn = false;
            IQueryable<Worker> list = GetDB().Workers
                .Where(w => w.memberexpirationdate < DateTime.Now && 
                    w.memberStatus == Worker.iActive);
            //
            if (list.Count() > 0) rtn = true;
            //
            foreach (Worker wkr in list)
            {
                wkr.memberStatus = Worker.iExpired;                
            }
            GetDB().SaveChanges();
            return rtn;
        }
        /// <summary>
        /// Reactivates sanctioned workers based on reactivation date
        /// </summary>
        /// <param name="db"></param>
        /// <returns>true if at least one record reactivated</returns>
        public bool ReactivateMembers() 
        {
            bool rtn = false;
            IQueryable<Worker> list = GetDB().Workers
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
            try
            {
                GetDB().SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder str = new StringBuilder();
                var foo = (from eve in e.EntityValidationErrors
                           from error in eve.ValidationErrors
                           select error.ErrorMessage).Aggregate<string>((c,n) => c + ", [" + n + "]");
                          
                throw new Exception(foo, e);
            }
            return rtn;
        }
    }
}