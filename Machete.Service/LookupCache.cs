using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Domain;
using System.Runtime.Caching;

namespace Machete.Service
{
    public class LookupCache
    {
        private static MacheteContext DB { get; set; }
        //private static IEnumerable<Lookup> DbCache { get; set; }
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
            IEnumerable<Lookup> lookups = DB.Lookups.AsNoTracking().ToList();
            //DbCache = DB.Set<Lookup>().AsNoTracking().ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(1));
            CacheItem wCacheItem = new CacheItem("lookupCache", lookups);
            cache.Set(wCacheItem, policy);
            //DB.Dispose();
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
