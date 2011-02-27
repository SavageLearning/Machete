using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;

namespace MemberAdmin1.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        //
        // GET: /Role/
        public ActionResult Index()
        {
            List<String> roles = Roles.GetAllRoles().ToList();
            return View(roles);
        }

        //
        // GET: /Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Role/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Role/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Roles.CreateRole(collection["roleName"]);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData.ModelState.AddModelError("_FORM", ex.Message);
                return View();
            }
        }

        //
        // GET: /Role/Delete/5
        public ActionResult Delete(String id)
        {
            ViewData["Role"] = id;
            return View();
        }

        //
        // POST: /Role/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(String id, FormCollection collection)
        {
            try
            {
                ViewData["Role"] = id;
                Roles.DeleteRole(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData.ModelState.AddModelError("ERROR", ex.Message);
                return View();
            }
        }
    }

    public class RoleItem
    {
        public String Role { get; set; }

        public RoleItem()
        {
        }

        public RoleItem(String role)
        {
            Role = role;
        }
    }
}

