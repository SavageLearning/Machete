using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Service;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Machete.Web.Helpers
{
    public class Lookups
    {
        #region declarations
        public static int maritalstatusDefault { get; private set; }
        public static int genderDefault { get; private set; }
        public static int woStatusDefault { get; private set; }
        public static int raceDefault { get; private set; }
        public static int languageDefault { get; private set; }
        public static int neighborhoodDefault { get; private set; }
        public static int incomeDefault { get; private set; }
        public static int emplrreferenceDefault { get; private set; }
        public static int typesOfWorkDefault { get; private set; }
        public static int eventtypeDefault { get; private set; }
        public static int memberStatusDefault { get; private set; }
        public static int transportmethodDefault { get; private set; }
        public static int countryoforiginDefault { get; private set; }        
        public static int hoursDefault { get; private set; }
        public static int daysDefault { get; private set; }
        public static int skillLevelDefault { get; private set; }
        public static int activityNameDefault { get; private set; }
        public static int activityTypeDefault { get; private set; }
        public static double hourlyWageDefault { get; private set; }
        public static int skillDefault { get; private set; }
        //
        public static SelectList maritalstatus(string locale) { return get("maritalstatus", locale); }
        public static SelectList gender(string locale) { return get("gender", locale); }
        public static SelectList orderstatus(string locale) { return get("orderstatus", locale); }
        public static SelectList race(string locale) { return get("race", locale); }
        public static SelectList language(string locale) { return get("language", locale); }
        public static SelectList neighborhood(string locale) { return get("neighborhood", locale); }
        public static SelectList income(string locale) { return get("income", locale); }
        public static SelectList typeOfWork(string locale) { return get("worktype", locale); }
        public static SelectList eventtype(string locale) { return get("eventtype", locale); }
        public static SelectList memberStatus(string locale) { return get("memberstatus", locale); }
        public static SelectList activityName(string locale) { return get("activityName", locale); }
        public static SelectList activityType(string locale) { return get("activityType", locale); }
        public static SelectList emplrreference(string locale) { return get("emplrreference", locale); }
        public static SelectList transportmethod(string locale) { return get("transportmethod", locale); }
        public static SelectList countryoforigin(string locale) { return get("countryoforigin", locale); }
        public static IEnumerable<LookupNumber> hours() { return hoursNum; }
        public static IEnumerable<LookupNumber> days() { return daysNum; }
        public static IEnumerable<LookupNumber> skillLevels() { return skillLevelNum; }
        public static List<SelectListItemEx> skill(string locale, bool specializedOnly)
        {
            return getEx("skill", locale, specializedOnly);
        }
        //
        private static IEnumerable<LookupNumber> hoursNum { get; set; }
        private static IEnumerable<LookupNumber> daysNum { get; set; }
        private static IEnumerable<LookupNumber> skillLevelNum { get; set; }
        private static List<SelectListItem> yesnoEN { get; set; }
        private static List<SelectListItem> yesnoES { get; set; }
        private static IQueryable<Lookup> DbCacheEN { get; set; }
        private static IQueryable<Lookup> DbCacheES { get; set; }
        private static IEnumerable<Lookup> DbCache { get; set; }
        private static Dictionary<string, SelectList> LookupLists { get; set; }
        #endregion
        //
        // Initialize once to prevent re-querying DB
        //
        //public static void Initialize(IEnumerable<Lookup> cache)
        public static void Initialize()
        {
            DbCache = LookupCache.getCache();
            maritalstatusDefault = getDefaultID("maritalstatus");
            genderDefault = getDefaultID("gender");
            woStatusDefault = getDefaultID("orderstatus");
            raceDefault = getDefaultID("race");
            languageDefault = getDefaultID("language");
            neighborhoodDefault = getDefaultID("neighborhood");
            incomeDefault = getDefaultID("income");
            typesOfWorkDefault = getDefaultID("worktype");
            eventtypeDefault = getDefaultID("eventtype");
            memberStatusDefault = getDefaultID("memberstatus"); 
            emplrreferenceDefault = getDefaultID("emplrreference");
            transportmethodDefault = getDefaultID("transportmethod");
            countryoforiginDefault = getDefaultID("countryoforigin");
            activityNameDefault = getDefaultID("activityName");
            activityTypeDefault = getDefaultID("activityType");
            //
            hourlyWageDefault = 12;
            hoursNum = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }
                .Select(x => new LookupNumber { Value = x, Text = x });
            hoursDefault = 5;
            daysNum = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" }
                .Select(x => new LookupNumber { Value = x, Text = x });
            daysDefault = 1;
            //
            skillLevelNum = new[] {"0", "1", "2", "3", "4" }
                .Select(x => new LookupNumber { Value = x, Text = x });
            skillLevelDefault = 1;         
            //
            skillDefault = getDefaultID("skill");

            yesnoEN = new List<SelectListItem>();
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "Yes", Value = "true" });
            yesnoES = new List<SelectListItem>();
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "Sí", Value = "true" });
        }


        //TODO: Lookups.yesno needs to use resource files, not hardcoded values
        public static List<SelectListItem> yesno(string locale)
        {
            if (locale == "es") return yesnoES;
            return yesnoEN;  //defaults to English
        }

        public static IEnumerable<string> getTeachers()
        {
            //SelectList list;

            //list = new SelectList(Roles.GetUsersInRole("Teacher"), "ID", "text", "Choose?!");
            //if (list == null) throw new ArgumentNullException("Get techers returned no teachers");
            return Roles.GetUsersInRole("Teacher").AsEnumerable();
        }

        private static SelectList get(string category, string locale)
        {
            string field;
            SelectList list;
            if (locale == "es") field = "text_ES";
            else field = "text_EN";
            
            list = new SelectList(DbCache.Where(s => s.category == category),
                                    "ID",
                                    field,
                                    getDefaultID(category));
            if (list == null) throw new ArgumentNullException("Get returned no lookups");
            return list;
        }
        /// <summary>
        /// get skills with extra information
        /// </summary>
        /// <param name="category">string that matches a category group</param>
        /// <param name="locale">two letter language identifier</param>
        /// <param name="specializedOnly">include only specialized skills</param>
        /// <returns>List of items for an MVC dropdown box</returns>
        private static List<SelectListItemEx> getEx(string category, string locale, bool specializedOnly)
        {            
            IEnumerable<Lookup> prelist = DbCache.ToList().Where(s => s.category == category);
            Func<Lookup, string> textFunc;
            if (prelist == null) throw new ArgumentNullException("No skills returned");
            if (specializedOnly) {
                textFunc = (ll => "[" + ll.ltrCode + ll.level + "] " + (locale == "es" ? ll.text_ES : ll.text_EN));
                prelist = prelist.Where(s => s.speciality == true).OrderBy(s => textFunc(s));
            } else {
                textFunc = (ll => locale == "es" ? ll.text_ES : ll.text_EN);
                if (locale == "es") prelist = prelist.OrderBy(s => s.sortorder).ThenBy(s => s.text_ES);
                else prelist = prelist.OrderBy(s => s.sortorder).ThenBy(s => s.text_EN);
            }
            return new List<SelectListItemEx>(prelist
                    .Select(x => new SelectListItemEx
                        { 
                            Selected = x.selected,
                            Value = Convert.ToString(x.ID),
                            Text = textFunc(x),
                            wage = Convert.ToString(x.wage),
                            minHour = Convert.ToString(x.minHour),
                            fixedJob = Convert.ToString(x.fixedJob)
                        }));
        }
        //
        // Returns the default ID for a given category
        public static int getDefaultID(string category)
        {
            if (category == null) throw new Exception("category label is null");
            int count;
            count = DbCache.Where(s => s.selected == true &&
                                           s.category == category)
                                .Count();
            if (count > 0) return DbCache.Where(s => s.selected == true &&
                                           s.category == category).SingleOrDefault().ID;
            return count; 

        }
        //
        // Get the ID number for a given lookup string
        //public static int getSingleEN(string category, string text)
        //{
        //    return LookupCache.getSingleEN(category, text);
        //}
        //
        // Get the Id string for a given lookup number
        public static string byID(int ID, string locale)
        {
            return LookupCache.byID(ID, locale);
        }
        public static string byID(int? ID, string locale)
        {
            return ID == null ? null : byID(ID, locale);
        }
        //
        // create multi-lingual yes/no strings
        public static string getBool(bool val, string locale)
        {
            if (locale == "es")
            {
                if (val) return "sí";
                else return "no";
            }
            if (val) return "yes";
            else return "no";
        }
        public static string getBool(bool? val, string locale)
        {            
            if (val == null) val = false;
            return getBool((bool)val, locale);
        }
    }
    public class LookupNumber
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}