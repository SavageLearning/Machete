using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.ViewModel;
using System.Web.Routing;
using Machete.Web.Models;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class ActivityController : MacheteController
    {
        private readonly IActivityService serv;
        private System.Globalization.CultureInfo CI;

        public ActivityController(IActivityService aServ)
        {
            this.serv = aServ;
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
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            dataTableResult<Activity> list = serv.GetIndexView(new viewOptions
            {
                search = param.sSearch,
                CI = CI,
                sortColName = param.sortColName(),
                displayStart = param.iDisplayStart,
                displayLength = param.iDisplayLength,
                attendedActivities = param.attendedActivities,
                personID = param.personID ?? 0,
                orderDescending = param.sSortDir_0 == "asc" ? false : true,
            });
            //return what's left to datatables
            var result = from p in list.query
                         select new
                         {
                             tabref = _getTabRef(p),
                             tablabel = _getTabLabel(p),
                             name = LookupCache.byID(p.name, CI.TwoLetterISOLanguageName),
                             type = LookupCache.byID(p.type, CI.TwoLetterISOLanguageName),
                             count = p.Signins.Count(),
                             teacher = p.teacher,
                             dateStart = p.dateStart.ToString(),
                             dateEnd = p.dateEnd.ToString(),
                             AID = Convert.ToString(p.ID),
                             recordid = Convert.ToString(p.ID),
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        private string _getTabRef(Activity emp)
        {
            if (emp == null) return null;
            return "/Activity/Edit/" + Convert.ToString(emp.ID);
        }
        private string _getTabLabel(Activity emp)
        {
            if (emp == null) return null;
            return emp.dateStart.ToString() + " - " + 
                    LookupCache.byID(emp.name, CI.TwoLetterISOLanguageName) + " - " +
                    emp.teacher;
        }
        /// <summary>
        /// GET: /Activity/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Create()
        {
            var _model = new Activity();
            _model.dateStart = DateTime.Today;
            _model.dateEnd = DateTime.Today;
            //_model.city = "Seattle";
            //_model.state = "WA";
            //_model.blogparticipate = false;
            //_model.referredby = Lookups.emplrreferenceDefault;
            return PartialView("Create", _model);
        }
        /// <summary>
        /// POST: /Activity/Create
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Create(Activity activ, string userName)
        {
            UpdateModel(activ);
            Activity newActivity = serv.Create(activ, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newActivity),
                sNewLabel = _getTabLabel(newActivity),
                iNewID = newActivity.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GET: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator, Manager, Teacher")]
        public ActionResult Edit(int id)
        {
            Activity employer = serv.Get(id);
            return PartialView("Edit", employer);
        }
        /// <summary>
        /// POST: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Teacher")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Activity employer = serv.Get(id);
            UpdateModel(employer);
            serv.Save(employer, userName);
            return Json(new
            {
                jobSuccess = true
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
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
        //
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Assign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            serv.AssignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true//,
                //deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public JsonResult Unassign(int personID, List<int> actList, string userName)
        {
            if (actList == null) throw new Exception("Activity List is null");
            serv.UnassignList(personID, actList, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true//,
                //deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
