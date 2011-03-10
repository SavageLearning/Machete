using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;

namespace Machete.Helpers
{
    public class Lookups
    {
        public static SelectListItem[] maritalstatus {get; private set;}
        public static SelectListItem[] gender { get; private set; } 

        public static void Initialize() {
            maritalstatus = init_maritalstatus();
            gender = init_gender();
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