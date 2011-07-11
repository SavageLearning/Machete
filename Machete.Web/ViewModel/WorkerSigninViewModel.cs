using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//using Machete.Domain.Resources;
using Machete.Domain;
//using Machete.Data;
using Machete.Web.Resources;

namespace Machete.Web.ViewModel
{
    public class WorkerSigninViewModel
    {
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(WorkerSignins))]
        [Required(ErrorMessageResourceName = "dwccardnumrequired", ErrorMessageResourceType = typeof(WorkerSignins))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(WorkerSignins))]
        public int dwccardentry { get; set; }
        public DateTime dateforsignin { get; set; }
        public Image last_chkin_image { get; set; }
        public DateTime last_chkin_memberexpirationdate { get; set; }
        public bool memberexpired { get; set; }
        public IEnumerable<WorkerSigninView> workersignins { get; set; }
    }
}