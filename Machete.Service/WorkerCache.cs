using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Domain;
using System.Data.Entity;

namespace Machete.Service
{
    public class WorkerCache
    {
        private static MacheteContext DB { get; set; }
        //private static readonly IDbSet<Worker> dbset;
        private static IEnumerable<Worker> DbCache { get; set; }

        public static void Initialize(MacheteContext db)
        {
            DB = db;
            //dbset = DB.Set<Worker>();
            FillCache();
        }
        private static void FillCache()
        {
            DbCache = DB.Workers.AsNoTracking().Include(p => p.Person).ToList();
            DB.Dispose();
        }
        public static IEnumerable<Worker> getCache()
        {
            return DbCache;
        }
        public static IEnumerable<Worker> getCache(Func<Worker, bool> where)
        {
            return DbCache;
        }
    }
}