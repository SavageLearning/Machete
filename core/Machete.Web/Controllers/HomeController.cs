using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Machete.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Hirer"))
                {
                    return Redirect("/V2/Onlineorders");
                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Changes()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult About()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Issues()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Wiki()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Docs()
        {
            return PartialView();
        }
        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Reports()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return PartialView();
        }
    }
}
