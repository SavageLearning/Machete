using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class ReportPrintView<T>
    {
        public IEnumerable<T> report { get; set; }
    }
}