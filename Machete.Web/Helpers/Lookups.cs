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

namespace Machete.Web.Helpers
{
    public class Lookups
    {
        #region definitions
        private static SelectList maritalstatusesEN { get; set; }
        private static SelectList maritalstatusesES { get; set; }
        public static int maritalstatusDefault { get; private set; }
        private static SelectList gendersEN { get; set; }
        private static SelectList gendersES { get; set; }
        public static int genderDefault { get; private set; }
        private static SelectList woStatusesEN { get; set; }
        private static SelectList woStatusesES { get; set; }
        public static int woStatusDefault { get; private set; }
        private static SelectList racesEN { get; set; }
        private static SelectList racesES { get; set; }
        public static int raceDefault { get; private set; }
        private static SelectList languagesEN { get; set; }
        private static SelectList languagesES { get; set; }
        public static int languageDefault { get; private set; }
        private static SelectList neighborhoodsEN { get; set; }
        private static SelectList neighborhoodsES { get; set; }
        public static int neighborhoodDefault { get; private set; }
        private static SelectList incomesEN { get; set; }
        private static SelectList incomesES { get; set; }
        public static int incomeDefault { get; private set; }
        private static SelectList emplrReferencesEN { get; set; }
        private static SelectList emplrReferencesES { get; set; }
        public static int emplrreferenceDefault { get; private set; }
        private static SelectList typesOfWorkEN { get; set; }
        private static SelectList typesOfWorkES { get; set; }
        public static int typesOfWorkDefault { get; private set; }
        private static SelectList transportmethodsEN { get; set; }
        private static SelectList transportmethodsES { get; set; }
        public static int transportmethodDefault { get; private set; }
        private static SelectList countriesoforiginEN { get; set; }
        private static SelectList countriesoforiginES { get; set; }
        public static int countryoforiginDefault { get; private set; }
        private static IEnumerable<LookupNumber> hoursNum { get; set; }
        public static int hoursDefault { get; private set; }
        private static IEnumerable<LookupNumber> daysNum { get; set; }
        public static int daysDefault { get; private set; }
        private static IEnumerable<LookupNumber> skillLevelNum { get; set; }
        public static int skillLevelDefault { get; private set; }

        public static double hourlyWageDefault { get; private set; }
        //private static List<SelectListItemEx> skillEN { get; set; }
        //private static List<SelectListItemEx> skillES { get; set; }
        public static int skillDefault { get; private set; }
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
        public static void Initialize(IEnumerable<Lookup> cache)
        {
            DbCache = cache;
            maritalstatusesEN = get("maritalstatus", "en");
            maritalstatusesES = get("maritalstatus", "es");
            maritalstatusDefault = getDefaultID("maritalstatus");
            gendersEN = get("gender", "en");
            gendersES = get("gender", "es");
            genderDefault = getDefaultID("gender");
            woStatusesEN = get("orderstatus", "en");
            woStatusesES = get("orderstatus", "es");
            woStatusDefault = getDefaultID("orderstatus");
            racesEN = get("race", "en");
            racesES = get("race", "es");
            raceDefault = getDefaultID("race");
            languagesEN = get("language", "en");
            languagesES = get("language", "es");
            languageDefault = getDefaultID("language");
            neighborhoodsEN = get("neighborhood", "en");
            neighborhoodsES = get("neighborhood", "es");
            neighborhoodDefault = getDefaultID("neighborhood");
            incomesEN = get("income", "en");
            incomesES = get("income", "es");
            incomeDefault = getDefaultID("income");
            typesOfWorkEN = get("worktype", "en");
            typesOfWorkES = get("worktype", "es");
            typesOfWorkDefault = getDefaultID("worktype");
            emplrReferencesEN = get("emplrreference", "en");
            emplrReferencesES = get("emplrreference", "es");
            emplrreferenceDefault = getDefaultID("emplrreference");
            transportmethodsEN = get("transportmethod", "en");
            transportmethodsES = get("transportmethod", "es");
            transportmethodDefault = getDefaultID("transportmethod");
            countriesoforiginEN = get("countryoforigin", "en");
            countriesoforiginES = get("countryoforigin", "es");
            countryoforiginDefault = getDefaultID("countryoforigin");
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
            //skillEN = getEx("skill", "en");
            //skillES = getEx("skill", "es");
            skillDefault = getDefaultID("skill");
            yesnoEN = new List<SelectListItem>();
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "Yes", Value = "true" });
            yesnoES = new List<SelectListItem>();
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "Si", Value = "true" });
        }

        public static SelectList maritalstatus(string locale)
        {
            if (locale == "es") return maritalstatusesES;
            return maritalstatusesEN;  //defaults to English
        }

        public static SelectList gender(string locale)
        {
            if (locale == "es") return gendersES;
            return gendersEN;  //defaults to English
        }

        public static SelectList orderstatus(string locale)
        {
            if (locale == "es") return woStatusesES;
            return woStatusesEN;  //defaults to English
        }

        public static SelectList race(string locale)
        {
            if (locale == "es") return racesES;
            return racesEN;  //defaults to English
        }

        public static SelectList language(string locale)
        {
            if (locale == "es") return languagesES;
            return languagesEN;  //defaults to English
        }

        public static SelectList neighborhood(string locale)
        {
            if (locale == "es") return neighborhoodsES;
            return neighborhoodsEN;  //defaults to English
        }

        public static SelectList income(string locale)
        {
            if (locale == "es") return incomesES;
            return incomesEN;  //defaults to English
        }

        public static SelectList typeOfWork(string locale)
        {
            if (locale == "es") return typesOfWorkES;
            return typesOfWorkEN;  //defaults to English
        }

        public static SelectList emplrreference(string locale)
        {
            if (locale == "es") return emplrReferencesES;
            return emplrReferencesEN;  //defaults to English
        }

        public static SelectList transportmethod(string locale)
        {
            if (locale == "es") return transportmethodsES;
            return transportmethodsEN;  //defaults to English
        }
        public static SelectList countryoforigin(string locale)
        {
            if (locale == "es") return countriesoforiginES;
            return countriesoforiginEN;  //defaults to English
        }
        public static IEnumerable<LookupNumber> hours()
        {
            return hoursNum;  
        }
        public static IEnumerable<LookupNumber> days()
        {
            return daysNum;
        }
        public static IEnumerable<LookupNumber> skillLevels()
        {
            return skillLevelNum;
        }

        public static List<SelectListItemEx> skill(string locale, bool specializedOnly)
        {
            return getEx("skill", locale, specializedOnly);
            //if (locale == "es") return skillES;
            //return skillEN;  //defaults to English
        }

        public static List<SelectListItem> yesno(string locale)
        {
            if (locale == "es") return yesnoES;
            return yesnoEN;  //defaults to English
        }


        private static SelectList get(string category, string locale)
        {
            //TODO: throw and catch exception if Lookup returns nothing
            SelectList list;
            if (locale == "es")
            {

                list = new SelectList(DbCache.Where(s => s.category == category),
                                      "ID",
                                      "text_ES",
                                      getDefaultID(category));
            }
            else //Default to English
            {
                list = new SelectList(DbCache.Where(s => s.category == category),
                                      "ID",
                                      "text_EN",
                                      getDefaultID(category));
            }
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
            //TODO: throw and catch exception if Lookup returns nothing
            IEnumerable<Lookup> prelist = DbCache.ToList().Where(s => s.category == category);
            Func<Lookup, string> textFunc;
            List<SelectListItemEx> list;
            if (specializedOnly) {
                textFunc = (ll => "[" + ll.ltrCode + ll.level + "] " + (locale == "es" ? ll.text_ES : ll.text_EN));
                prelist = prelist.Where(s => s.speciality == true).OrderBy(s => textFunc(s));
            } else {
                textFunc = (ll => locale == "es" ? ll.text_ES : ll.text_EN);
                if (locale == "es") prelist = prelist.OrderBy(s => s.sortorder).ThenBy(s => s.text_ES);
                else prelist = prelist.OrderBy(s => s.sortorder).ThenBy(s => s.text_EN);
            }          
            list =  new List<SelectListItemEx>(prelist
                    .Select(x => new SelectListItemEx
                    { 
                        Selected = x.selected,
                        Value = Convert.ToString(x.ID),
                        Text = textFunc(x),
                        wage = Convert.ToString(x.wage),
                        minHour = Convert.ToString(x.minHour),
                        fixedJob = Convert.ToString(x.fixedJob)
                    }));
            return list;
        }

        //
        // Returns the default ID for a given category
        //
        public static int getDefaultID(string category)
        {
            //TODO Exception handling
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
        //
        public static int getSingleEN(string category, string text)
        {
            return LookupCache.getSingleEN(category, text);
        }
        //
        // Get the Id string for a given lookup number
        //
        public static string byID(int ID, string locale)
        {
            return LookupCache.byID(ID, locale);
        }
        //
        // create multi-lingual yes/no strings
        //
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