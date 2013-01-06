#region COPYRIGHT
// File:     Lookups.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
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
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Service;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Globalization;

namespace Machete.Web.Helpers
{
    public class LookupGroup
    {
        public int defaultId { get; set; }
        public string name { get; set; }
        protected static IEnumerable<Lookup> DbCache { get; set; }
        private static List<LookupGroup> groups = new List<LookupGroup>();

        public LookupGroup(string name)
        {
            if (name == null) throw new Exception("LookupGroup name is null");
            this.name = name;            
            //this.defaultId = getDefaultID();
            groups.Add(this);
        }

        public static void Initialize(IEnumerable<Lookup> cache) 
        {
            DbCache = cache;
            foreach (var a in groups)
            {
                a.defaultId = a.getDefaultID();
            }

        }

        /// <summary>
        /// Gets the default ID for the group
        /// </summary>
        /// <returns></returns>
        public int getDefaultID()
        {            
            //
            // TODO: remove DbCache == null hack; needed for unit tests b/c of Lookup.blah
            if (DbCache == null) return 0;
            int count;
            count = DbCache.Where(s => s.selected == true &&
                                           s.category == this.name)
                                .Count();
            if (count > 0) return DbCache.Where(s => s.selected == true &&
                                           s.category == this.name).SingleOrDefault().ID;
            return count;
        }
        /// <summary>
        /// Get the SelectList for the group
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public virtual SelectList get(string locale)
        {
            string field;
            SelectList list;
            if (locale == "es") field = "text_ES";
            else field = "text_EN";
            if (DbCache == null) DbCache = LookupCache.getCache();

            list = new SelectList(DbCache.Where(s => s.category == this.name),
                                    "ID",
                                    field,
                                    getDefaultID());
            if (list == null) throw new ArgumentNullException("Get returned no lookups");
            return list;
        }
    }

    public class LookupSkill : LookupGroup
    {
        public LookupSkill(string name) : base(name) { }
        /// <summary>
        /// get the List of skills
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="specializedOnly">only return specialized entries</param>
        /// <returns></returns>
        public List<SelectListItemEx> get(string locale, bool specializedOnly)
        {
            IEnumerable<Lookup> prelist = DbCache.ToList().Where(s => s.category == this.name);
            Func<Lookup, string> textFunc;
            if (prelist == null) throw new ArgumentNullException("No skills returned");
            if (specializedOnly)
            {
                //TODO: Not scalable on i18n. Kludge.
                textFunc = (ll => "[" + ll.ltrCode + ll.level + "] " + (locale == "es" ? ll.text_ES : ll.text_EN));
                prelist = prelist.Where(s => s.speciality == true).OrderBy(s => textFunc(s)); //LINQ & FUNC
            }
            else
            {
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
    }

    public class Lookups
    {
        //public enum categoriesfoo
        //{
        //    maritalstatus = "maritalstatus",
        //    race = "race",
        //    language = "",
        //    neighborhood = "language",
        //    gender = "gender",
        //    transportmethod = "transportmethod",
        //    countryoforigin = "countryoforigin",
        //    activityName = "activityName",
        //    activityType = "activityType",
        //    eventtype = "eventtype",
        //    woStatus = "woStatus",
        //    emplrreference = "emplrreference",
        //    typesOfWork = "typesOfWork",
        //    memberStatus = "memberStatus",
        //    skill = "skill"
        //}
        public static LookupGroup maritalstatus = new LookupGroup("maritalstatus");
        public static LookupGroup race = new LookupGroup("race");
        public static LookupGroup language = new LookupGroup("language");
        public static LookupGroup neighborhood = new LookupGroup("neighborhood");
        public static LookupGroup income = new LookupGroup("income");
        public static LookupGroup gender = new LookupGroup("gender");
        public static LookupGroup transportmethod = new LookupGroup("transportmethod");
        public static LookupGroup countryoforigin = new LookupGroup("countryoforigin");
        public static LookupGroup activityName = new LookupGroup("activityName");
        public static LookupGroup activityType = new LookupGroup("activityType");
        public static LookupGroup eventtype = new LookupGroup("eventtype");
        public static LookupGroup woStatus = new LookupGroup("orderstatus");
        public static LookupGroup emplrreference = new LookupGroup("emplrreference");
        public static LookupGroup typesOfWork = new LookupGroup("worktype");
        public static LookupGroup memberStatus = new LookupGroup("memberstatus");
        public static LookupSkill skill = new LookupSkill("skill");
        public static int hoursDefault { get { return 5; } }
        public static int daysDefault { get { return 1;  } }
        public static int skillLevelDefault { get { return 1; } }
        public static double hourlyWageDefault { get { return 12; } }
        public static SelectList hours() { return hoursNum; }
        public static SelectList days() { return daysNum; }
        public static SelectList skillLevels() { return skillLevelNum; }
        public static SelectList configCategories() { return categories; }
        private static IEnumerable<Lookup> DbCache { get; set; }
        private static SelectList hoursNum { get; set; }
        private static SelectList daysNum { get; set; }
        private static SelectList categories { get; set; }
        private static SelectList skillLevelNum { get; set; }
        private static List<SelectListItem> yesnoEN { get; set; }
        private static List<SelectListItem> yesnoES { get; set; }
        //
        // Initialize once to prevent re-querying DB
        //
        //public static void Initialize(IEnumerable<Lookup> cache)
        public static void Initialize()
        {
            DbCache = LookupCache.getCache();
            LookupGroup.Initialize(DbCache);
            hoursNum = new SelectList(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }
                .Select(x => new LookupNumber { Value = x, Text = x }),
                "Value", "Text", "7"
                );
            daysNum = new SelectList(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" }
                .Select(x => new LookupNumber { Value = x, Text = x }),
                "Value", "Text", "7"
                );            
            skillLevelNum = new SelectList(new[] { "0", "1", "2", "3", "4" }
                .Select(x => new SelectListItem { Value = x, Text = x }),
                "Value", "Text", "0");
            categories = new SelectList(new[] {"maritalstatus","race","neighborhood","gender",
                "transportmethod","countryoforigin","activityName","activityType","eventtype",
                "orderstatus","emplrreference","worktype","memberStatus","skill"}
                .Select(x => new SelectListItem { Value = x, Text = x}),
                "Value", "Text", "activityName");

            yesnoEN = new List<SelectListItem>();
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoEN.Add(new SelectListItem() { Selected = false, Text = "Yes", Value = "true" });
            yesnoES = new List<SelectListItem>();
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "No", Value = "false" });
            yesnoES.Add(new SelectListItem() { Selected = false, Text = "Sí", Value = "true" });
        }
        //TODO: Lookups.yesno needs to use resource files, not hardcoded values
        public static List<SelectListItem> yesno(CultureInfo CI)
        {
            if (CI.TwoLetterISOLanguageName == "es") return yesnoES;
            return yesnoEN;  //defaults to English
        }

        public static IEnumerable<string> getTeachers()
        {
            return Roles.GetUsersInRole("Teacher").AsEnumerable();
        }
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