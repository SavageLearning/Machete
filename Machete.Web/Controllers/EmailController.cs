using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Web.Helpers;
using Machete.Service;
using System.Web.Routing;
using System.Globalization;
using AutoMapper;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EmailController : MacheteController
    {
        private readonly IEmailService serv;
        private CultureInfo CI;

        public EmailController(IEmailService eServ)
        {
            this.serv = eServ;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET: /Activity/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Email> list = serv.GetIndexView(vo);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = from p in list.query
                         select new
                         {
                             recordid = Convert.ToString(p.ID),
                             tabref = _getTabRef(p),
                             tablabel = _getTabLabel(p),
                             emailFrom = p.emailFrom,
                             emailTo = p.emailTo,
                             subject = p.subject,
                             transmitAttempts = p.transmitAttempts.ToString(),
                             lastAttempt = p.lastAttempt.ToString(),
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby
                         }
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }
        private string _getTabRef(Email email)
        {
            if (email == null) return null;
            return "/Email/Edit/" + Convert.ToString(email.ID);
        }
        private string _getTabLabel(Email email)
        {
            if (email == null) return null;
            return email.subject;
        }
        //
        // POST: /Email/Create

        [HttpPost]
        public ActionResult Create(Email email)
        {
            if (ModelState.IsValid)
            {
                //db.Emails.Add(email);
                //db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(email);
        }
        
        //
        // GET: /Email/Edit/5
 
        public ActionResult Edit(int id)
        {
            //Email email = db.Emails.Find(id);
            return View();
        }

        //
        // POST: /Email/Edit/5

        [HttpPost]
        public ActionResult Edit(Email email)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(email).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(email);
        }

        //
        // GET: /Email/Delete/5
 
        public ActionResult Delete(int id)
        {
            //Email email = db.Emails.Find(id);
            return View();
        }

        //
        // POST: /Email/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            //Email email = db.Emails.Find(id);
            //db.Emails.Remove(email);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}