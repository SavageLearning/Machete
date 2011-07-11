using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.Models;
using Machete.Domain;

namespace Machete.Web.ViewModel
{
    public class EmployerIndex
    {
        public Filter filter { get; set; }
        public Employer employer { get; set; }
        public IEnumerable<Employer> employers { get; set; }
    }
}