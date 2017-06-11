using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class ReportsV2Controller : MacheteController
    {
        // GET: ReportsV2
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Index()
        {
            return View();
        }
    }
}