using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Http404(string url)
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            ViewData["url"] = url;
            return View();
        }


    }
}
