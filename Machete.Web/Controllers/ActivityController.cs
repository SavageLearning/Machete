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
            var allA = serv.GetAll();
            IEnumerable<Activity> filteredA;
            IEnumerable<Activity> sortedA;

            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredA = serv.GetAll()
                    .Where(p => LookupCache.byID(p.name, CI.TwoLetterISOLanguageName).ContainsOIC(param.sSearch) ||
                                p.notes.ContainsOIC(param.sSearch) ||
                                p.teacher.ContainsOIC(param.sSearch) ||
                                LookupCache.byID(p.type, CI.TwoLetterISOLanguageName).ContainsOIC(param.sSearch) ||
                                p.dateStart.ToString().ContainsOIC(param.sSearch) ||
                                p.dateEnd.ToString().ContainsOIC(param.sSearch));
            }
            else
            {
                filteredA = allA;
            }
            //Sort the Persons based on column selection
            var sortDesc = param.sSortDir_0 == "asc" ? false : true;
            switch (param.sortColName())
            {
                case "name": 
                    sortedA = sortDesc ?  
                        filteredA.OrderByDescending(p => LookupCache.byID(p.name, CI.TwoLetterISOLanguageName)) :
                        filteredA.OrderBy(p => LookupCache.byID(p.name, CI.TwoLetterISOLanguageName));
                    break;
                case "type": 
                    sortedA = sortDesc ?  
                        filteredA.OrderByDescending(p => LookupCache.byID(p.type, CI.TwoLetterISOLanguageName)) :
                        filteredA.OrderBy(p => LookupCache.byID(p.type, CI.TwoLetterISOLanguageName));
                    break;
                case "count":
                    sortedA = sortDesc ?  
                        filteredA.OrderByDescending(p => p.Signins.Count()) :
                        filteredA.OrderBy(p => p.Signins.Count());
                    break;
                case "teacher":
                    sortedA = sortDesc ?
                        filteredA.OrderByDescending(p => p.teacher) :
                        filteredA.OrderBy(p => p.teacher);
                    break;
                case "dateStart":
                    sortedA = sortDesc ?
                        filteredA.OrderByDescending(p => p.dateStart) :
                        filteredA.OrderBy(p => p.dateStart);
                    break;
                case "dateEnd":
                    sortedA = sortDesc ?
                        filteredA.OrderByDescending(p => p.dateEnd) :
                        filteredA.OrderBy(p => p.dateEnd);
                    break;
                case "dateupdated":
                    sortedA = sortDesc ?
                        filteredA.OrderByDescending(p => p.dateupdated) :
                        filteredA.OrderBy(p => p.dateupdated);
                    break;
                default:
                    sortedA = sortDesc ? filteredA.OrderByDescending(p => p.dateStart) :
                        filteredA.OrderBy(p => p.dateStart);
                    break;
            }

            //Limit results to the display length and offset
            var displayA = sortedA.Skip(param.iDisplayStart)
                                  .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayA
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
                iTotalRecords = allA.Count(),
                iTotalDisplayRecords = filteredA.Count(),
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
