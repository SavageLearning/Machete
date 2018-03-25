using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Controllers
{
    public class HirerAccountController : Controller
    {
        // GET: HirerAccount
        public void Index()
        {
            Response.RedirectPermanent("/V2/welcome");
        }
    }
}