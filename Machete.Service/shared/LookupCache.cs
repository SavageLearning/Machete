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
using Machete.Data.Infrastructure;
using Machete.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Machete.Service
{
    public interface ILookupCache
    {
        void Dispose();
        IEnumerable<Lookup> getCache();
        void Refresh();
        bool isSpecialized(int skillid);
        Lookup getByID(int id);
        string textByID(int ID, string locale);
        int getByKeys(string category, string key);
        IEnumerable<int> getSkillsByWorkType(int worktypeID);
        IEnumerable<string> getTeachers();
    }


    public class LookupCache : ILookupCache
    {
        private IDatabaseFactory DB { get; set; }
        private CacheItem DbCache { get; set; }
        private ObjectCache cache;
        //
        //
        public LookupCache(IDatabaseFactory db)
        {
            cache = MemoryCache.Default;
            DB = db;
            FillCache(); //commented out related to moq'ing
        }
        public void Dispose()
        {
            DB = null;
            DbCache = null;
            cache = null;
        }
        //
        private void FillCache()
        {
            IEnumerable<Lookup> lookups = DB.Get().Lookups.ToList();
            var teacherID = DB.Get().Roles.First(r => r.Name == "Teacher").Id;
            IEnumerable<string> teachers = DB.Get().Users.Where(u => u.Roles.Any(r => r.RoleId == teacherID))
                .Select(x => x.UserName).Distinct().ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            //TODO: Put LookupCache expire time in config file
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5));
            var lookupItem = new CacheItem("lookupCache", lookups);
            var teacherItem = new CacheItem("teacherCache", teachers);
            cache.Set(lookupItem, policy);
            cache.Set(teacherItem, policy);

            #region WORKERS
            Worker.iActive = getByKeys(LCategory.memberstatus, LMemberStatus.Active);
            Worker.iSanctioned = getByKeys(LCategory.memberstatus, LMemberStatus.Sanctioned);
            Worker.iExpelled = getByKeys(LCategory.memberstatus, LMemberStatus.Expelled);
            Worker.iExpired = getByKeys(LCategory.memberstatus, LMemberStatus.Expired);
            Worker.iInactive = getByKeys(LCategory.memberstatus, LMemberStatus.Inactive);
            //
            #endregion  
            #region WORKORDERS
            WorkOrder.iActive = getByKeys(LCategory.orderstatus, LOrderStatus.Active);
            WorkOrder.iPending = getByKeys(LCategory.orderstatus, LOrderStatus.Pending);
            WorkOrder.iCompleted = getByKeys(LCategory.orderstatus, LOrderStatus.Completed);
            WorkOrder.iCancelled = getByKeys(LCategory.orderstatus, LOrderStatus.Cancelled);
            WorkOrder.iExpired = getByKeys(LCategory.orderstatus, LOrderStatus.Expired);
            #endregion
            #region EMAILS
            Email.iReadyToSend = getByKeys(LCategory.emailstatus, LEmailStatus.ReadyToSend);
            Email.iSent = getByKeys(LCategory.emailstatus, LEmailStatus.Sent);
            Email.iSending = getByKeys(LCategory.emailstatus, LEmailStatus.Sending);
            Email.iPending = getByKeys(LCategory.emailstatus, LEmailStatus.Pending);
            Email.iTransmitError = getByKeys(LCategory.emailstatus, LEmailStatus.TransmitError);
            #endregion
        }
        //
        //
        public IEnumerable<Lookup> getCache()
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
        public void Refresh()
        {
            FillCache();
        }
        //
        //
        public bool isSpecialized(int skillid)
        {
            return getCache().Single(s => s.ID == skillid).speciality;
        }
        public Lookup getByID(int id)
        {
            return getCache().Single(s => s.ID == id);
        }
        //
        /// <summary>
        /// Get the English or Spanish name for a given lookup number
        /// </summary>
        /// <param name="ID">ID of the record to look up</param>
        /// <param name="locale">Two Letter ISO Language Name</param>
        /// <returns></returns>
        public string textByID(int ID, string locale)
        {
            Lookup record;
            if (ID == 0) return null;
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
        public int getByKeys(string category, string key)
        {
            try
            {
                return getCache().Single(s => s.category == category && s.key == key).ID;
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to Lookup Category: " + category + ", text: " + key);
            }
        }
        //
        //
        public IEnumerable<int> getSkillsByWorkType(int worktypeID)
        {
            try
            {
                return getCache().Where(l => l.typeOfWorkID == worktypeID).Select(l => l.ID);
            }
            catch {
                throw new MacheteIntegrityException("getSkillsByWorkType throws exception for worktype ID:" +worktypeID);
            }
        }

        public IEnumerable<string> getTeachers()
        {
            CacheItem teachers = cache.GetCacheItem("teacherCache");
            if (teachers == null)
            {
                FillCache();
                teachers = cache.GetCacheItem("teacherCache");
            }
            return teachers.Value as IEnumerable<string>;
        }
    }
}
