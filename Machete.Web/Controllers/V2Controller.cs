using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Controllers
{
    [AllowAnonymous]
    public class V2Controller : Controller
    {
        // GET: V2
        public ActionResult Index()
        {
            return View("Index","_angularLayout", new { });
        }
    }
}