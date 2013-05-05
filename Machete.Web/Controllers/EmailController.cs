using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;

namespace Machete.Web.Controllers
{ 
    public class EmailController : Controller
    {
        private MacheteContext db = new MacheteContext();

        //
        // GET: /Email/

        public ViewResult Index()
        {
            return View(db.Emails.ToList());
        }

        //
        // GET: /Email/Details/5

        public ViewResult Details(int id)
        {
            Email email = db.Emails.Find(id);
            return View(email);
        }

        //
        // GET: /Email/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Email/Create

        [HttpPost]
        public ActionResult Create(Email email)
        {
            if (ModelState.IsValid)
            {
                db.Emails.Add(email);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(email);
        }
        
        //
        // GET: /Email/Edit/5
 
        public ActionResult Edit(int id)
        {
            Email email = db.Emails.Find(id);
            return View(email);
        }

        //
        // POST: /Email/Edit/5

        [HttpPost]
        public ActionResult Edit(Email email)
        {
            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(email);
        }

        //
        // GET: /Email/Delete/5
 
        public ActionResult Delete(int id)
        {
            Email email = db.Emails.Find(id);
            return View(email);
        }

        //
        // POST: /Email/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Email email = db.Emails.Find(id);
            db.Emails.Remove(email);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}