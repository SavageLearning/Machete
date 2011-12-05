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
            //DbCache = DB.Lookups.AsNoTracking().ToList();
            DbCache = DB.Set<Lookup>().AsNoTracking().ToList();
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
        public static Lookup getBySkillID(int skillid)
        {
            return DbCache.Single(s => s.ID == skillid);
        }
        //
        // Get the Id string for a given lookup number
        //
        public static string byID(int ID, string locale)
        {
            Lookup record;
            try
            {
                record = DbCache.Single(s => s.ID == ID);
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
        //
        public static int getSingleEN(string category, string text)
        {
            int rtnint = 0;
            try
            {
                rtnint = DbCache.Single(s => s.category == category && s.text_EN == text).ID;
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to Lookup Category: " + category + ", text: " + text);
            }
            return rtnint;
        }
        //
        //
        //
        public static IEnumerable<int> getSkillsByWorkType(int worktypeID)
        {
            try
            {
                return DbCache.Where(l => l.typeOfWorkID == worktypeID).Select(l => l.ID );
            }
            catch {
                throw new MacheteIntegrityException("getSkillsByWorkType throws exception for worktype ID:" +worktypeID);
            }

        }
    }
}
