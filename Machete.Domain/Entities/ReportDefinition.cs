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
            idString = "reportdef";
        }
        public string name          { get; set; }
        public string description   { get; set; }
        public string sqlquery      { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }

    }
}
