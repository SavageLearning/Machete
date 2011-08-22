using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Domain;

namespace Machete.Service
{
    public class LookupCache
    {
        private static MacheteContext DB { get; set; }
        private static IEnumerable<Lookup> DbCache { get; set; }
        public static void Initialize(MacheteContext db)
        {
            DB = db;
            FillCache();
        }
        private static void FillCache()
        {
            //DbCache = DB.Lookups.OrderBy(s => s.category).ThenBy(s => s.sortorder).ToList();
            DbCache = DB.Lookups.ToList();
            DB.Dispose();
        }
        public static IEnumerable<Lookup> getCache()
        {
            return DbCache;
        }
        public static bool isSpecialized(int skillid)
        {
            return DbCache.Single(s => s.ID == skillid).speciality;
        }
        public static Lookup getByID(int skillid)
        {
            return DbCache.Single(s => s.ID == skillid);
        }
    }
}
