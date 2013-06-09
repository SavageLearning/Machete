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
using Machete.Web.ViewModel;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EmailController : MacheteController
    {
        private readonly IEmailService serv;
        private readonly ILookupCache lcache;
        private CultureInfo CI;

        public EmailController(IEmailService eServ, ILookupCache lc)
        {
            this.serv = eServ;
            lcache = lc;
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
                             relatedTo = _getRelatedObject(p),
                             subject = p.subject,
                             status = lcache.textByID(p.statusID, CI.TwoLetterISOLanguageName),
                             transmitAttempts = p.transmitAttempts.ToString(),
                             lastAttempt = p.lastAttempt.ToString(),
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby
                         }
            },
            JsonRequestBehavior.AllowGet);
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
        private string _getRelatedObject(Email email)
        {
            if (email.isJoinedToWorkOrder)
            {
                return "WO: " + serv .GetAssociatedWorkOrderFor(email).paperOrderNum.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// GET: /Email/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            var emailview = Mapper.Map<Email, EmailView>(new Email());
            return PartialView("Create", emailview);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Create(EmailView emailview, string userName)
        {
            UpdateModel(emailview);
            var email = Mapper.Map<EmailView, Email>(emailview);
            Email newEmail = serv.Create(email, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newEmail),
                sNewLabel = _getTabLabel(newEmail),
                iNewID = newEmail.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Duplicate(int id, string userName)
        {
            Email e = serv.Get(id);
            Email duplicate = e;
            duplicate.statusID = Email.iPending;
            duplicate.lastAttempt = null;
            duplicate.transmitAttempts = 0;

            serv.Create(duplicate, userName);
            return Json(new
            {
                sNewRef = _getTabRef(duplicate),
                sNewLabel = _getTabLabel(duplicate),
                iNewID = duplicate.ID
            },
            JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// GET: /Email/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, string userName)
        {
            EmailView emailview;
            // lock on read for fail
            Email email = serv.GetExclusive(id, userName);
            if (email != null)
            {
                emailview = Mapper.Map<Email, EmailView>(email);
                return PartialView("Edit", emailview);
            }
            email = serv.Get(id);
            emailview = Mapper.Map<Email, EmailView>(email);
            return PartialView("View", emailview);

        }
        /// <summary>
        /// POST: /Email/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Edit(EmailView emailview, FormCollection collection, string userName)
        {
            UpdateModel(emailview);
            var email = Mapper.Map<EmailView, Email>(emailview);
            serv.Save(email, userName);
            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>s
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        [UserNameFilter]
        [Authorize(Roles="Administrator, Manager, Phonedesk")]
        public ActionResult ConfirmDialog(int woid, string userName)
        {
            var email = serv.GetLatestConfirmEmailBy(woid);
            if (email == null)
            {
                email = new Email();
                return PartialView("Create", email);
            }
            else
            {
                return Edit(email.ID, userName);
            }
        }
    }
}