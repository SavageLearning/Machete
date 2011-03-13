using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;

namespace Machete.Helpers
{
    public class Lookups
    {
        public static SelectListItem[] maritalstatuses {get; private set;}
        public static SelectListItem[] genders { get; private set; }
        public static IEnumerable<Race> races { get; private set; }
        public static IEnumerable<Language> languages { get; private set; }
        public static IEnumerable<Neighborhood> neighborhoods { get; private set; }
        public static IEnumerable<Income> incomes { get; private set; }
        public static MacheteContext MacheteDB { get; set; }

        public static void Initialize() {

            MacheteDB = new MacheteContext(); 
            maritalstatuses = init_maritalstatus();
            genders = init_gender();
            races = MacheteDB.Races.ToList();
            languages = MacheteDB.Languages.ToList();
            neighborhoods = MacheteDB.Neighborhoods.ToList();
            incomes = MacheteDB.Incomes.ToList();
            
        }

        private static SelectListItem[] init_maritalstatus()
        {
            return new[]
            {
                new SelectListItem {Value = "S", Text = "Single", Selected=true},
                new SelectListItem {Value = "M", Text = "married"},
                new SelectListItem {Value = "D", Text = "Divorced"}
            };
        }
        private static SelectListItem[] init_gender()
        {
            return new[]
            {
                new SelectListItem {Value = "M", Text = "Male", Selected=true},
                new SelectListItem {Value = "F", Text = "Female"},
                new SelectListItem {Value = "T", Text = "Transgender"},
                new SelectListItem {Value = "O", Text = "Other"}
            };
        }
    }
}