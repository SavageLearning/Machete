using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Web.Helpers;
using Elmah;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
