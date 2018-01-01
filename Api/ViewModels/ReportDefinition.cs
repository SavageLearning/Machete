using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api
{
    public class ReportDefinition : Domain.ReportDefinition
    {
        public int id { get; set; }
        public object columns { get; set; }
        public object inputs { get; set; }
    }
}