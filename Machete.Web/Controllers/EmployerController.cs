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
using AutoMapper;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class EmployerController : MacheteController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;
        private System.Globalization.CultureInfo CI;

        public EmployerController(IEmployerService employerService, IWorkOrderService workorderService)
        {
            this.serv = employerService;
            this.woServ = workorderService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
            ViewBag.idPrefix = "employer"; //TODO: integration with mUIExtension, feed idPrefix
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
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Employer> list = serv.GetIndexView(vo);
            //return what's left to datatables
            var result = from p in list.query select new 
            { 
                tabref = _getTabRef(p),
                tablabel = _getTabLabel(p),
                active = Convert.ToString(p.active),
                EID = Convert.ToString(p.ID),
                recordid = Convert.ToString(p.ID),
                name = p.name, 
                address1 = p.address1, 
                city = p.city, 
                phone =  p.phone, 
                dateupdated = Convert.ToString(p.dateupdated),
                Updatedby = p.Updatedby,
                onlineSource = p.onlineSource
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
            _model.referredby = Lookups.emplrreference.defaultId;
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

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult CreateCombined()
        {
            var model = new EmployerWoCombined();
            return PartialView(model);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult CreateCombined(EmployerWoCombined combined, string userName)
        {
            //UpdateModel(combined);
            //split the combined model into domain models
            Employer mappedEmployer = Mapper.Map<EmployerWoCombined, Employer>(combined);
            WorkOrder mappedWO = Mapper.Map<EmployerWoCombined, WorkOrder>(combined);
            mappedEmployer.onlineSource = true;
            mappedWO.onlineSource = true;
            //update domain
            Employer newEmployer = serv.Create(mappedEmployer, userName);
            mappedWO.EmployerID = newEmployer.ID;
            mappedWO.status = LookupCache.getCache().Where(a => a.category == "orderstatus" && a.text_EN == "Pending").Single().ID;
            WorkOrder newWO = woServ.Create(mappedWO, userName);
            //re-combine for display
            //EmployerWoCombined result = Mapper.Map<Employer, EmployerWoCombined>(newEmployer);
            //result = Mapper.Map<WorkOrder, EmployerWoCombined>(newWO, result);
            return Json(new
            {
                iEmployerID = newEmployer.ID,
                iWorkOrderID = newWO.ID,
                //EmployerWoConbined = result,
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
