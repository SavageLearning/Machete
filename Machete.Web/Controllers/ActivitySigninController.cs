using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Domain;
using Machete.Data;
using Machete.Web.Models;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ActivitySigninController : MacheteController
    {
        private readonly IActivitySigninService serv;
        private readonly IWorkerService wServ;
        private System.Globalization.CultureInfo CI;

        public ActivitySigninController(IActivitySigninService serv, 
                                 IWorkerService wServ)
        {
            this.serv = serv;
            this.wServ = wServ;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwccardnum"></param>
        /// <param name="activityID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, int activityID)
        {
            var _asi = new ActivitySignin();
            // Tthe card just swiped
            _asi.dateforsignin = DateTime.Now;
            _asi.activityID = activityID;
            _asi.dwccardnum = dwccardnum;            
            //
            //
            Worker w = serv.CreateSignin(_asi, this.User.Identity.Name);
            //Get picture from checkin, show with next view
            Image checkin_image = serv.getImage(dwccardnum);
            string imageRef = "/Content/images/NO-IMAGE-AVAILABLE.jpg";
            if (checkin_image != null)
            {
                imageRef = "/Image/GetImage/" + checkin_image.ID;
            }

            return Json(new
            {
                memberExpired = w.isExpired,
                memberInactive = w.isInactive,
                memberSanctioned = w.isSanctioned,
                memberExpelled = w.isExpelled,
                imageRef = imageRef,
                expirationDate = w.memberexpirationdate
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            dataTableResult<asiView> was = serv.GetIndexView(new viewOptions
            {
                CI = CI,
                search = param.sSearch,
                date = param.todaysdate == null ? null : (DateTime?)DateTime.Parse(param.todaysdate),
                dwccardnum = Convert.ToInt32(param.dwccardnum),
                woid = Convert.ToInt32(param.searchColName("WOID")),
                orderDescending = param.sSortDir_0 == "asc" ? false : true,
                displayStart = param.iDisplayStart,
                displayLength = param.iDisplayLength,
                sortColName = param.sortColName(),
                wa_grouping = param.wa_grouping,
                typeofwork_grouping = param.typeofwork_grouping,
                activityID = param.activityID,
                personID = param.personID ?? 0
            });

            //return what's left to datatables
            var result = from p in was.query
                         select new
                         {
                             WSIID = p.ID,
                             recordid = p.ID.ToString(),
                             dwccardnum = p.dwccardnum,
                             fullname = p.fullname,
                             firstname1 = p.firstname1,
                             firstname2 = p.firstname2,
                             lastname1 = p.lastname1,
                             lastname2 = p.lastname2,
                             dateforsignin = p.dateforsignin,
                             dateforsigninstring = p.dateforsignin.ToShortDateString(),
                             memberStatus = LookupCache.byID(p.memberStatus, CI.TwoLetterISOLanguageName),
                             memberInactive = p.w.isInactive,
                             memberSanctioned = p.w.isSanctioned,
                             memberExpired = p.w.isExpired,
                             memberExpelled = p.w.isExpelled,
                             imageID = p.imageID,
                             expirationDate = p.expirationDate.ToShortDateString(),
                             //type = LookupCache.byID(p.type, CI.TwoLetterISOLanguageName),
                             //activityName = LookupCache.byID(p.name, CI.TwoLetterISOLanguageName),
                             //teacher = p.teacher,
                             //dateStart = p.dateStart.ToString(),
                             //dateEnd = p.dateEnd.ToString()
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
