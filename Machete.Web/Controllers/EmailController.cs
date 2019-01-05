using System;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Email = Machete.Domain.Email;

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
            serv = eServ;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
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
            dataTableResult<Email> list = serv.GetIndexView(vo);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = from p in list.query
                         select new
                         {
                             recordid = Convert.ToString(p.ID),
                             //relatedTo = _getRelatedTo(p),
                             tabref = _getTabRef(p),
                             tablabel = _getTabLabel(p),
                             p.emailFrom,
                             p.emailTo,
                             p.subject,
                             status = def.byID(p.statusID),
                             transmitAttempts = p.transmitAttempts.ToString(),
                             lastAttempt = p.lastAttempt.ToString(),
                             dateupdated = Convert.ToString(p.dateupdated),
                             p.updatedby,
                             hasAttachment = string.IsNullOrEmpty(p.attachment) ?  Shared.False : Shared.True
                         }
            });
        }

        private string _getTabRef(Email email)
        {
            if (email == null) return null;
            return "/Email/Edit/" + Convert.ToString(email.ID);
        }
        private string _getTabLabel(Email email)
        {
            return email == null ? null : email.subject;
        }
        /// <summary>
        /// GET: /Email/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            var e = new Email();
            var ev = map.Map<Email, EmailView>(e);
            ev.status = def.byID(e.statusID);
            ev.templates = def.getEmailTemplates();
            ev.def = def;
            ViewBag.EmailStatuses = def.getSelectList(LCategory.emailstatus);
            return PartialView("Create", ev);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailview"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public async Task<JsonResult> Create(EmailView emailview, string userName)
        {
            Email newEmail;
            if (await TryUpdateModelAsync(emailview)) {
            var email = map.Map<EmailView, Email>(emailview);
            if (emailview.attachment != null)
            {
                email.attachment = HttpUtility.UrlDecode(emailview.attachment);
                email.attachmentContentType = MediaTypeNames.Text.Html;
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
            });
        } else {
            return Json(new { jobSuccess = false });
            }
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Duplicate(int id, int? woid, string userName)
        {
            var duplicate = serv.Duplicate(id, woid, userName);
            return Json(new
            {
                sNewRef = _getTabRef(duplicate),
                sNewLabel = _getTabLabel(duplicate),
                iNewID = duplicate.ID
            });
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
            var email = serv.GetExclusive(id, userName);
            if (email != null)
            {
                pvType = "Edit";
            }
            else
            {
                pvType = "View";
                email = serv.Get(id);
            }
            ev = map.Map<Email, EmailView>(email);
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
            var newemail = map.Map(emailview, email);
            if (emailview.attachment != null)
            {
                newemail.attachment = HttpUtility.HtmlDecode(emailview.attachment);
                newemail.attachmentContentType = MediaTypeNames.Text.Html;
            }
            serv.Save(newemail, userName);
            return Json(new
            {
                jobSuccess = true
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>s
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
            });
        }

        [UserNameFilter]
        [Authorize(Roles="Administrator, Manager, Phonedesk")]
        public ActionResult ConfirmDialog(int woid, string userName)
        {
            // get most recent email for a work order
            var email = serv.GetLatestConfirmEmailBy(woid);
            if (email == null)
            {
                var ev = map.Map<Email, EmailView>(new Email());
                ev.def = def;
                var wo = serv.GetAssociatedWorkOrderFor(woid);
                ev.status = def.byID(ev.statusID);
                ev.templates = def.getEmailTemplates();
                ev.woid = woid;
                ev.emailTo = wo.Employer.email;
                ev.subject = string.Format(Emails.defaultSubject, def.getConfig("OrganizationName"),wo.paperOrderNum);
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
                    ev = map.Map<Email, EmailView>(lockedemail);
                    ev.def = def;
                    ev.status = def.byID(email.statusID);
                    ev.templates = def.getEmailTemplates();
                    ev.woid = woid;
                    return PartialView("EditDialog", ev);
                }
                lockedemail = serv.Get(email.ID);
                ev = map.Map<Email, EmailView>(lockedemail);
                ev.status = def.byID(email.statusID);
                ev.templates = def.getEmailTemplates();
                ev.woid = woid;
                return PartialView("ViewDialog", ev);
            }
        }
    }
}