using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
//using Machete.Helpers;
using Machete.Web.Helpers;
using NLog;
using Machete.Domain;
using Machete.Data;
//using Microsoft.Reporting.WebForms;
using Machete.Web.Models;
//using System.Data.Objects.SqlClient;
//using System.Data.Objects;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
   [ElmahHandleError]
    public class WorkerSigninController : MacheteController
    {
        private readonly IWorkerSigninService _serv;
        private readonly IWorkerService _wServ;
        //private Logger log = LogManager.GetCurrentClassLogger();
        private System.Globalization.CultureInfo CI;
        //private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerSigninController", "");
        public WorkerSigninController(IWorkerSigninService workerSigninService, IWorkerService workerService)
        {
            this._serv = workerSigninService;
            this._wServ = workerService;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        //
        // GET: /WorkerSignin/
        // Initial page creation
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index()
        {        
            //WorkerSigninViewModel _model = new WorkerSigninViewModel();
            //if (ViewBag.dateforsignin == null) _model.dateforsignin = DateTime.Today;
            //else _model.dateforsignin = ViewBag.dateforsignin;
            return View();//_model);
        }
        //
        // POST: /WorkerSignin/Index -- records a signin
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, DateTime dateforsignin)
        {
            
            var _signin = new WorkerSignin();
            bool memberInactive = false;
            bool memberSanctioned = false;
            bool memberExpelled = false;
            bool memberExpired = false;
            // Tthe card just swiped
            _signin.dwccardnum = dwccardnum;
            _signin.dateforsignin = dateforsignin;
            //
            //
            _serv.CreateWorkerSignin(_signin, this.User.Identity.Name);

            Worker worker = _wServ.GetWorkerByNum(dwccardnum);
            //Get picture from checkin, show with next view
            Image checkin_image = _serv.getImage(dwccardnum);           
            string imageRef = "/Content/images/NO-IMAGE-AVAILABLE.jpg";
            if (checkin_image != null)
            {
                imageRef = "/Image/GetImage/" + checkin_image.ID;
            }

            
            if (worker != null)
            {
                memberInactive = worker.memberStatus == LookupCache.getSingleEN("memberstatus", "Inactive") ? true : false;
                memberSanctioned = worker.memberStatus == LookupCache.getSingleEN("memberstatus", "Sanctioned") ? true : false;
                memberExpelled = worker.memberStatus == LookupCache.getSingleEN("memberstatus", "Expelled") ? true : false;            
            }
            if (worker.memberexpirationdate < DateTime.Now)            
                memberExpired = true;            

            return Json(new
            {
                memberExpired = memberExpired,
                memberInactive = memberInactive,
                memberSanctioned = memberSanctioned,
                memberExpelled = memberExpelled,
                imageRef = imageRef,
                expirationDate = worker.memberexpirationdate
            },
            JsonRequestBehavior.AllowGet);
        }

        //
        // Lottery post
        //
        // POST: /WorkOrder/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult lotterySignin(int lotterycardnum, DateTime lotterysignindate, string user)
        {
            //var workOrder = woServ.GetWorkOrder(id);
            string rtnstatus;
            var wsi = _serv.GetWorkerSignin(lotterycardnum, lotterysignindate);
            if (wsi != null)
            {
                if (wsi.lottery_timestamp == null)
                {
                    wsi.lottery_timestamp = DateTime.Now;
                    _serv.SaveWorkerSignin();
                }                
                rtnstatus = "OK";

            }
            else
            {
                rtnstatus = "NO_WSI_REC";
            }
            var _signin = new WorkerSignin();
            return Json(new
            {
                status = rtnstatus,
                memberID = lotterycardnum
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /WorkerSignin/Delete/5
        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult Delete(int id)
        {
            _serv.DeleteWorkerSignin(id);            
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            ServiceIndexView<WorkerSigninView> was = _serv.GetIndexView(new DispatchOptions {
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
                typeofwork_grouping = param.typeofwork_grouping
            });

            //return what's left to datatables
            var result = from p in was.query
                         select new {  WSIID = p.ID,
                                       recordid = p.ID.ToString(),
                                       dwccardnum = p.dwccardnum,
                                       fullname = p.fullname,
                                       firstname1 = p.firstname1,
                                       firstname2 = p.firstname2,
                                       lastname1 = p.lastname1,
                                       lastname2 = p.lastname2, 
                                       dateforsignin = p.dateforsignin,
                                       dateforsigninstring = p.dateforsignin.ToShortDateString(),
                                       WAID = p.waid ?? 0,
                                       memberStatus = LookupCache.byID(p.memberStatus, CI.TwoLetterISOLanguageName),
                                       memberInactive = p.memberStatus == LookupCache.getSingleEN("memberstatus", "Inactive") ? true : false,
                                       memberSanctioned = p.memberStatus == LookupCache.getSingleEN("memberstatus", "Sanctioned") ? true : false,
                                       memberExpired = p.memberStatus == LookupCache.getSingleEN("memberstatus", "Expired") ? true : false,
                                       memberExpelled = p.memberStatus == LookupCache.getSingleEN("memberstatus", "Expelled") ? true : false,
                                       imageID = p.imageID,
                                       expirationDate = p.expirationDate.ToShortDateString(),
                                       skills = _getSkillCodes(p.englishlevel, p.skill1, p.skill2, p.skill3)
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
        private string _getSkillCodes(int eng, int? sk1, int? sk2, int? sk3)
        {
            string rtnstr = "E" + eng + " ";
            if (sk1 !=null) {
                var lookup = LookupCache.getBySkillID((int)sk1);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (sk2 != null)
            {
                var lookup = LookupCache.getBySkillID((int)sk2);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
            }
            if (sk3 != null)
            {
                var lookup = LookupCache.getBySkillID((int)sk3);
                rtnstr = rtnstr + lookup.ltrCode + lookup.level;
            }
            return rtnstr;
        }
    }
}
