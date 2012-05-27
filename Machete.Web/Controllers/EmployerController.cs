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
    public class EmployerController : MacheteController
    {
        private readonly IEmployerService serv;
        private System.Globalization.CultureInfo CI;

        public EmployerController(IEmployerService employerService)
        {
            this.serv = employerService;
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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET: /Employer/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            IEnumerable<Employer> list = serv.GetIndexView(new viewOptions
            {
                search = param.sSearch,
                sortColName = param.sortColName(),
                displayStart = param.iDisplayStart,
                displayLength = param.iDisplayLength,
                orderDescending = param.sSortDir_0 == "asc" ? false : true,
            });

            //return what's left to datatables
            var result = from p in list
                         select new { tabref = _getTabRef(p),
                                      tablabel = _getTabLabel(p),
                                      active = Convert.ToString(p.active),
                                      EID = Convert.ToString(p.ID),
                                      recordid = Convert.ToString(p.ID),
                                      name = p.name, 
                                      address1 = p.address1, 
                                      city = p.city, 
                                      phone =  p.phone, 
                                      dateupdated = Convert.ToString(p.dateupdated),
                                      Updatedby = p.Updatedby 
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = serv.TotalCount(),
                iTotalDisplayRecords = list.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        private string _getTabRef(Employer emp)
        {
            if (emp == null) return null;
            return "/Employer/Edit/" + Convert.ToString(emp.ID);
        }
        private string _getTabLabel(Employer emp)
        {
            if (emp == null) return null;
            return emp.name;
        }
        /// <summary>
        /// GET: /Employer/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var _model = new Employer();
            _model.active = true;
            _model.city = "Seattle";
            _model.state = "WA";
            _model.blogparticipate = false;
            _model.referredby = Lookups.emplrreferenceDefault;
            return PartialView("Create", _model);
        }
        /// <summary>
        /// POST: /Employer/Create
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Create(Employer employer, string userName)
        {
            UpdateModel(employer);
            Employer newEmployer = serv.Create(employer, userName);                          

            return Json(new
            {
                sNewRef = _getTabRef(newEmployer),
                sNewLabel = _getTabLabel(newEmployer),
                iNewID = newEmployer.ID,
                jobSuccess = true
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GET: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            Employer employer = serv.Get(id);
            return PartialView("Edit", employer);
        }
        /// <summary>
        /// POST: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Employer employer = serv.Get(id);
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
    }
}
