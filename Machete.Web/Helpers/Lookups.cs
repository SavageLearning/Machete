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
using System.Threading;

namespace Machete.Web.Helpers
{

    public static class Lookups
    {
        public static int hoursDefault { get { return 5; } }
        public static int daysDefault { get { return 1;  } }
        public static int skillLevelDefault { get { return 1; } }
        public static double hourlyWageDefault { get { return 12; } }
        public static SelectList hours() { return hoursNum; }
        public static SelectList days() { return daysNum; }
        public static SelectList skillLevels() { return skillLevelNum; }
        public static SelectList configCategories() { return categories; }
        private static SelectList hoursNum { get; set; }
        private static SelectList daysNum { get; set; }
        private static SelectList categories { get; set; }
        private static SelectList skillLevelNum { get; set; }
        private static List<SelectListItem> yesnoEN { get; set; }
        private static List<SelectListItem> yesnoES { get; set; }
        private static ILookupCache lcache;
        //
        // Initialize once to prevent re-querying DB
        //
        //public static void Initialize(IEnumerable<Lookup> cache)
        public static void Initialize(ILookupCache lc)
        {
            lcache = lc;
            hoursNum = new SelectList(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }
                .Select(x => new LookupNumber { Value = x, Text = x }),
                "Value", "Text", "7"
                );
            daysNum = new SelectList(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" }
                .Select(x => new LookupNumber { Value = x, Text = x }),
                "Value", "Text", "7"
                );            
            skillLevelNum = new SelectList(new[] { "0", "1", "2", "3" }
                .Select(x => new SelectListItem { Value = x, Text = x }),
                "Value", "Text", "0");
            categories = new SelectList(new[] {
                LCategory.maritalstatus, LCategory.race, LCategory.neighborhood, LCategory.gender,
                LCategory.transportmethod, LCategory.countryoforigin, LCategory.activityName, 
                LCategory.activityType, LCategory.eventtype, LCategory.orderstatus, LCategory.emplrreference, 
                LCategory.worktype, LCategory.memberstatus,LCategory.skill,LCategory.emailstatus, LCategory.emailTemplate}
                .Select(x => new SelectListItem { Value = x, Text = x}),
                "Value", "Text", LCategory.activityName);

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
        public static string byID(int ID)
        {
            return lcache.textByID(ID, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant());
        }
        public static string byID(int? ID)
        {
            return ID == null ? null : byID((int)ID);
        }
        //
        // create multi-lingual yes/no strings
        public static string getBool(bool val)
        {
            if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant() == "es")
            {
                if (val) return "sí"; 
                else return "no";
            }
            if (val) return "yes";
            else return "no";
        }
        public static string getBool(bool? val)
        {            
            return getBool(val ?? false);
        }

        /// <summary>
        /// Gets the default ID for the group
        /// </summary>
        /// <returns></returns>
        public static int getDefaultID(string type)
        {
            int count;
            count = lcache.getCache()
                .Where(s => s.selected == true &&
                            s.category == type)
                .Count();
            if (count > 0)
            {
                return lcache.getCache()
                            .Where(s => s.selected == true &&
                                        s.category == type)
                            .SingleOrDefault().ID;
            }
            return count;
        }
        /// <summary>
        /// Get the SelectList for the group
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static SelectList getSelectList(string type)
        {
            var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
            string field;
            SelectList list;
            if (locale == "ES") field = "text_ES";
            else field = "text_EN";
            list = new SelectList(lcache.getCache().Where(s => s.category == type),
                                    "ID",
                                    field,
                                    getDefaultID(type));
            if (list == null) throw new ArgumentNullException("Get returned no lookups");
            return list;
        }

        public static List<SelectListItemEmail> getEmailTemplates()
        {
            var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
            IEnumerable<Lookup> prelist = lcache.getCache()
                                         .Where(s => s.category == LCategory.emailTemplate);
            return new List<SelectListItemEmail>(prelist
                .Select(x => new SelectListItemEmail
                {
                    Selected = x.selected,
                    Value = Convert.ToString(x.ID),
                    Text = locale == "es" ? x.text_ES : x.text_EN,
                    template = x.emailTemplate
                }));
        }

        /// <summary>
        /// get the List of skills. used in Worker.cshtml & WorkAssignment.cshtml
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="specializedOnly">only return specialized entries</param>
        /// <returns></returns>
        public static List<SelectListItemEx> getSkill(bool specializedOnly)
        {
            var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();
            IEnumerable<Lookup> prelist = lcache.getCache()
                                                     .Where(s => s.category == LCategory.skill);
            Func<Lookup, string> textFunc; //anon function
            if (prelist == null) throw new ArgumentNullException("No skills returned");
            if (specializedOnly)
            {
                //TODO: Selection of ES/EN not scalable on i18n. Kludge.
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
    public class LookupNumber
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}