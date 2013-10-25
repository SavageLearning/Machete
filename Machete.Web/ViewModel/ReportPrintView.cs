using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Machete.Service;

namespace Machete.Web.ViewModel
{
    public class ReportPrintView<T>
    {
        public dataTableResult<T> report { get; set; }
    }
}