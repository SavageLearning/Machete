#region COPYRIGHT
// File:     LookupCache.cs
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
using System.Runtime.Caching;

namespace Machete.Service
{

    public static class LookupCache
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
        public static void Dispose()
        {
            DB = null;
            DbCache = null;
            cache = null;
        }
        //
        //
        private static void FillCache()
        {
            IEnumerable<Lookup> lookups = DB.Lookups.AsNoTracking().ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            //TODO: Put LookupCache expire time in config file
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(1));
            CacheItem wCacheItem = new CacheItem("lookupCache", lookups);
            cache.Set(wCacheItem, policy);
            //
            #region WORKERS
            Worker.iActive = getSingleEN("memberstatus", "Active");
            Worker.iSanctioned = getSingleEN("memberstatus", "Sanctioned");
            Worker.iExpelled = getSingleEN("memberstatus", "Expelled");
            Worker.iExpired = getSingleEN("memberstatus", "Expired");
            Worker.iInactive = getSingleEN("memberstatus", "Inactive");
            //
            Worker.iDWC = getSingleEN("worktype", "(DWC) Day Worker Center");//TODO: Remove Casa specific configuration. needs real abstraction on iDWC / iHHH.
            Worker.iHHH = getSingleEN("worktype", "(HHH) Household Helpers");//TODO: Remove Casa specific configuration. needs real abstraction on iDWC / iHHH.
            #endregion  
            //
            #region WORKORDERS
            //
            WorkOrder.iActive = getSingleEN("orderstatus", "Active");
            WorkOrder.iPending = getSingleEN("orderstatus", "Pending");
            WorkOrder.iCompleted = getSingleEN("orderstatus", "Completed");
            WorkOrder.iCancelled = getSingleEN("orderstatus", "Cancelled");
            WorkOrder.iExpired = getSingleEN("orderstatus", "Expired");
            #endregion
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
        public static IEnumerable<Lookup> getCache()
        {
            CacheItem wCacheItem = cache.GetCacheItem("lookupCache");
            if (wCacheItem == null)
            {
                FillCache();
                wCacheItem = cache.GetCacheItem("lookupCache");
            }
            return wCacheItem.Value as IEnumerable<Lookup>;
        }
        //
        //
        public static void Refresh(MacheteContext db)
        {
            FillCache(db);
        }
        //
        //
        public static bool isSpecialized(int skillid)
        {
            return getCache().Single(s => s.ID == skillid).speciality;
        }
        public static Lookup getBySkillID(int skillid)
        {
            return getCache().Single(s => s.ID == skillid);
        }
        //
        // Get the Id string for a given lookup number
        public static string byID(int ID, string locale)
        {
            Lookup record;
            try
            {
                record = getCache().Single(s => s.ID == ID);
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to find Lookup record " + ID);
            }
            if (locale == "es")
            {
                return record.text_ES;
            }
            return record.text_EN; ;  //defaults to English
        }
        //
        // Get the ID number for a given lookup string
        public static int getSingleEN(string category, string text)
        {
            int rtnint = 0;
            try
            {
                rtnint = getCache().Single(s => s.category == category && s.text_EN == text).ID;
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to Lookup Category: " + category + ", text: " + text);
            }
            return rtnint;
        }
        //
        //
        public static IEnumerable<int> getSkillsByWorkType(int worktypeID)
        {
            try
            {
                return getCache().Where(l => l.typeOfWorkID == worktypeID).Select(l => l.ID);
            }
            catch {
                throw new MacheteIntegrityException("getSkillsByWorkType throws exception for worktype ID:" +worktypeID);
            }

        }
    }
}
