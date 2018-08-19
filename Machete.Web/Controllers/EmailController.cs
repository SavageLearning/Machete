using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EmailController : MacheteController
    {
        private readonly IEmailService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private CultureInfo CI;

        public EmailController(
            IEmailService eServ, 
            IDefaults def,
            IMapper map
            )
        {
            this.serv = eServ;
            this.map = map;
            this.def = def;
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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
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
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Domain.Email> list = serv.GetIndexView(vo);
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
                             status = def.byID(p.statusID),
                             transmitAttempts = p.transmitAttempts.ToString(),
                             lastAttempt = p.lastAttempt.ToString(),
                             dateupdated = Convert.ToString(p.dateupdated),
                             updatedby = p.updatedby,
                             hasAttachment = string.IsNullOrEmpty(p.attachment) ?  Shared.False : Shared.True
                         }
            },
            JsonRequestBehavior.AllowGet);
        }

        private string _getTabRef(Domain.Email email)
        {
            if (email == null) return null;
            return "/Email/Edit/" + Convert.ToString(email.ID);
        }
        private string _getTabLabel(Domain.Email email)
        {
            if (email == null) return null;
            return email.subject;
        }
        /// <summary>
        /// GET: /Email/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            var e = new Domain.Email();
            var ev = map.Map<Domain.Email, EmailView>(e);
            ev.status = def.byID(e.statusID);
            ev.templates = def.getEmailTemplates();
            ev.def = def;
            ViewBag.EmailStatuses = def.getSelectList(Machete.Domain.LCategory.emailstatus);
            return PartialView("Create", ev);
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
            Domain.Email newEmail;
            UpdateModel(emailview);
            var email = map.Map<EmailView, Domain.Email>(emailview);
            if (emailview.attachment != null)
            {
                email.attachment = Server.HtmlDecode(emailview.attachment);
                email.attachmentContentType = System.Net.Mime.MediaTypeNames.Text.Html;
            }
            if (emailview.woid.HasValue)
            {
                newEmail = serv.Create(email, userName, emailview.woid);
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
        public ActionResult Duplicate(int id, int? woid, string userName)
        {
            Domain.Email duplicate = serv.Duplicate(id, woid, userName);
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
            EmailView ev;
            string pvType;
            // lock on read for fail
            Domain.Email email = serv.GetExclusive(id, userName);
            if (email != null)
            {
                pvType = "Edit";
            }
            else
            {
                pvType = "View";
                email = serv.Get(id);
            }
            ev = map.Map<Domain.Email, EmailView>(email);
            ev.def = def;
            ev.status = def.byID(email.statusID);
            ev.templates = def.getEmailTemplates();
            return PartialView(pvType, ev);

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
            var newemail = map.Map<EmailView, Domain.Email>(emailview, email);
            if (emailview.attachment != null)
            {
                newemail.attachment = Server.HtmlDecode(emailview.attachment);
                newemail.attachmentContentType = System.Net.Mime.MediaTypeNames.Text.Html;
            }
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
                var ev = map.Map<Domain.Email, EmailView>(new Domain.Email());
                ev.def = def;
                var wo = serv.GetAssociatedWorkOrderFor(woid);
                ev.status = def.byID(ev.statusID);
                ev.templates = def.getEmailTemplates();
                ev.woid = woid;
                ev.emailTo = wo.Employer.email;
                ev.subject = string.Format(Resources.Emails.defaultSubject, def.getConfig("OrganizationName"),wo.paperOrderNum);
                return PartialView("CreateDialog", ev);
            }
            else
            {
                //
                // This block is almost an exact cut n paste of /Email/Edit GET call above
                // not abstracting it because it will change if the frontend is refactored with
                // a browser MVC framework (angularjs for example)
                EmailView ev;
                // lock on read for fail
                var  lockedemail = serv.GetExclusive(email.ID, userName);
                if (lockedemail != null)
                {
                    ev = map.Map<Domain.Email, EmailView>(lockedemail);
                    ev.def = def;
                    ev.status = def.byID(email.statusID);
                    ev.templates = def.getEmailTemplates();
                    ev.woid = woid;
                    return PartialView("EditDialog", ev);
                }
                lockedemail = serv.Get(email.ID);
                ev = map.Map<Domain.Email, EmailView>(lockedemail);
                ev.status = def.byID(email.statusID);
                ev.templates = def.getEmailTemplates();
                ev.woid = woid;
                return PartialView("ViewDialog", ev);
            }
        }
    }
}