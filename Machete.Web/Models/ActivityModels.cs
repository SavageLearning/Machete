using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Models
{
    public class ActivityViewModel
    {
        public CultureInfo CI { get; set; }
        public int authenticated { get; set; }
    }
}