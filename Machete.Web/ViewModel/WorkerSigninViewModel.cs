using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Machete.Domain.Resources;
using Machete.Domain;
using Machete.Data;

namespace Machete.Web.ViewModel
{
    public class WorkerSigninViewModel
    {
        public int dwccardentry { get; set; }
        public DateTime dateforsignin { get; set; }
        public IEnumerable<WorkerSigninView> workersignins { get; set; }
    }
}