using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
        public ActionResult Http404(string url)
        {
            Response.StatusCode = 404;
            //Response.TrySkipIisCustomErrors = true;
            ViewData["url"] = url;
            return View();
        }


    }
}
