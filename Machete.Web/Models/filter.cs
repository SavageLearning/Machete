using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Models
{
    public class Filter
    {
        // Controller & Views assume that for every property, there is a showProperty
        // and that class will flip the show bit if the controller sets 
        private bool _inactive;

        public bool showInactive { get; set; } 
        public bool inactive 
        {
            get { return _inactive; }
            set
            {
                showInactive = true;
                _inactive = value;
            }
        }
        public bool showDateRange { get; set; }
        public DateTime dateRange { get; set; }
    }
}