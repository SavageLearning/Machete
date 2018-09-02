using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class ReportDefinition : Record
    {
        public ReportDefinition()
        {
        }
        public string name          { get; set; } // used in URLs, needs to be url-friendly, no spaces
        public string commonName    { get; set; } // used for dropdowns and titles
        public string title         { get; set; } // if null use commonName
        public string description   { get; set; }
        public string sqlquery      { get; set; }
        public string category      { get; set; }
        public string subcategory   { get; set; }
        public string inputsJson { get; set; } // Which search inputs to display for this report
        public string columnsJson { get; set; } // must match order in sqlquery
        //public bool softdelete { get; set; }
        //public string labelTextEN { get; set; }
    }
}
