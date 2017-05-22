using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class ReportDefinition : Domain.ReportDefinition
    {
        public IDefaults def { get; set; }
        public int id { get; set; }
        public object columns { get; set; }
    }
}