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

        public ActivityController(IActivityService employerService)
        {
            this.serv = employerService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        #region Index
        //
        // GET: /Activity/
        //
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
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allActivities = serv.GetAll();
            IEnumerable<Activity> filteredActivities;
            IEnumerable<Activity> sortedActivities;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredActivities = serv.GetAll();
                    //.Where(p => p.active.ToString().Contains(param.sSearch) ||
                    //            p.name.Contains(param.sSearch) ||
                    //            p.address1.Contains(param.sSearch) ||
                    //            p.phone.Contains(param.sSearch) ||
                    //            p.city.Contains(param.sSearch));
            }
            else
            {
                filteredActivities = allActivities;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Activity, string> orderingFunction = (p => 
                                                          //sortColIdx == 2 ? p.active.ToString() :
                                                          //sortColIdx == 3 ? p.name :
                                                          //sortColIdx == 4 ? p.address1 :
                                                          //sortColIdx == 5 ? p.city :
                                                          //sortColIdx == 6 ? p.phone :
                                                          p.dateupdated.ToBinary().ToString());

            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedActivities = filteredActivities.OrderBy(orderingFunction);
            else
                sortedActivities = filteredActivities.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayActivities = sortedActivities.Skip(param.iDisplayStart)
                                                .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayActivities
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
                             EID = Convert.ToString(p.ID),
                             recordid = Convert.ToString(p.ID),

                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allActivities.Count(),
                iTotalDisplayRecords = filteredActivities.Count(),
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
        #endregion

        #region Create
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
        public JsonResult Create(Activity employer, string userName)
        {
            UpdateModel(employer);
            Activity newActivity = serv.Create(employer, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newActivity),
                sNewLabel = _getTabLabel(newActivity),
                iNewID = newActivity.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit
        /// <summary>
        /// GET: /Activity/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher")]
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
        #endregion

        #region View
        /// <summary>
        /// GET: /Activity/View/ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator, Manager, Teacher")]
        //public ActionResult View(int id)
        //{
        //    ActivityViewModel _vm = new ActivityViewModel();
        //    _vm.employer = serv.Get(id);
        //    _vm.orders = serv.GetOrders(id);
        //    return View(_vm);
        //}
        #endregion

        #region Delete
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
        #endregion
    }
}
