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
        
        //private static IEnumerable<Worker> DbCache { get; set; }
        private static CacheItem DbCache { get; set; }
        
        private static ObjectCache cache;

        public static void Initialize(MacheteContext db)
        {
            cache = MemoryCache.Default;
            DB = db;
            //dbset = DB.Set<Worker>();
            FillCache();
        }
        private static void FillCache()
        {
            IEnumerable<Worker> workers = DB.Workers.AsNoTracking().Include(p => p.Person).ToList();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5));
            CacheItem wCacheItem = new CacheItem("workerCache", workers);
            cache.Set(wCacheItem, policy);
            //DB.Dispose();
        }
        private static void FillCache(MacheteContext db)
        {
            DB = db;
            FillCache();
        }

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
        public static void Refresh(MacheteContext db)
        {
            FillCache(db);            
        }
        //public static IEnumerable<Worker> getCache(Func<Worker, bool> where)
        //{
        //    return DbCache;
        //}
    }
}