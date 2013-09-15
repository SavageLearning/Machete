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
                             //relatedTo = _getRelatedTo(p),
                             tabref = _getTabRef(p),
                             tablabel = _getTabLabel(p),
                             emailFrom = p.emailFrom,
                             emailTo = p.emailTo,
                             subject = p.subject,
                             status = lcache.textByID(p.statusID, CI.TwoLetterISOLanguageName),
                             transmitAttempts = p.transmitAttempts.ToString(),
                             lastAttempt = p.lastAttempt.ToString(),
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby,
                             hasAttachment = string.IsNullOrEmpty(p.attachment) ? false : true
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
        //private string _getRelatedTo(Email email)
        //{
        //    if (email.WorkOrders.Count() > 0)
        //    {
        //     return "WO: "+ email.WorkOrders.First().paperOrderNum;
        //    }
        //    return string.Empty;
        //}

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
            Email newEmail;
            UpdateModel(emailview);
            var email = Mapper.Map<EmailView, Email>(emailview);

            email.attachment = Server.HtmlDecode(emailview.attachment);
            email.attachmentContentType = System.Net.Mime.MediaTypeNames.Text.Html;
            if (emailview.woid.HasValue)
            {
                newEmail = serv.CreateWithWorkorder(email, (int)emailview.woid, userName);
            }
            else
            {
                newEmail = serv.Create(email, userName);
            }

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
            //UpdateModel(emailview);
            var email = serv.Get(emailview.ID);
            var newemail = Mapper.Map<EmailView, Email>(emailview, email);
            newemail.attachment = Server.HtmlDecode(emailview.attachment);
            serv.Save(newemail, userName);
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
            // get most recent email for a work order
            var email = serv.GetLatestConfirmEmailBy(woid);
            if (email == null)
            {
                var emailview = Mapper.Map<Email, EmailView>(new Email());
                var wo = serv.GetAssociatedWorkOrderFor(woid);
                emailview.woid = woid;
                emailview.emailTo = wo.Employer.email;
                emailview.subject = string.Format("$$$Casa Latina work order {0} confirmation", wo.paperOrderNum);
                return PartialView("CreateDialog", emailview);
            }
            else
            {
                //
                // This block is almost an exact cut n paste of /Email/Edit GET call above
                // not abstracting it because it will change if the frontend is refactored with
                // a browser MVC framework (angularjs for example)
                EmailView emailview;
                // lock on read for fail
                var  lockedemail = serv.GetExclusive(email.ID, userName);
                if (lockedemail != null)
                {
                    emailview = Mapper.Map<Email, EmailView>(lockedemail);
                    emailview.woid = woid;
                    return PartialView("EditDialog", emailview);
                }
                lockedemail = serv.Get(email.ID);
                emailview = Mapper.Map<Email, EmailView>(lockedemail);
                emailview.woid = woid;
                return PartialView("ViewDialog", emailview);
            }
        }
    }
}