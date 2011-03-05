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
    public class WorkerViewModel
    {
        public Domain.Worker worker { get; set; }
        public Person person { get; set; }
        public byte RaceID { get; set; }
        public IEnumerable<SelectListItem> Race { get; set; }
    }
}
