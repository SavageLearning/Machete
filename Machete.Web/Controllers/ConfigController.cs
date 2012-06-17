using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using System.Web.Routing;
using System.Data.Entity.Infrastructure;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ConfigController : MacheteController
    {
        private readonly ILookupService serv;
        System.Globalization.CultureInfo CI;
        public ConfigController(ILookupService serv)
        {
            this.serv = serv;
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
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records            
            IEnumerable<Lookup> list = serv.GetIndexView(new viewOptions()
            {
                CI = CI, 
                category = param.category,
                search = param.sSearch,
                //status = string.IsNullOrEmpty(param.searchColName("status")) ? (int?)null : Convert.ToInt32(param.searchColName("status")),
                orderDescending = param.sSortDir_0 == "asc" ? false : true,
                displayStart = param.iDisplayStart,
                displayLength = param.iDisplayLength,
                sortColName = param.sortColName()
            });
            var result = from p in list
                         select new
                         {
                             tabref = "/Config/Edit/" + Convert.ToString(p.ID),
                             tablabel = p.category + ' ' + p.text_EN,
                             category = p.category,
                             selected = p.selected,
                             text_EN = p.text_EN,
                             text_ES = p.text_ES,
                             subcategory = p.subcategory,
                             level = p.level,
                             //wage = p.wage,
                             //minHour = p.minHour,
                             //fixedJob = p.fixedJob,
                             //sortorder = p.sortorder,
                             //typeOfWorkID = p.typeOfWorkID,
                             //specialtiy = p.speciality,
                             ltrCode = p.ltrCode,
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby,
                             recordid = Convert.ToString(p.ID)
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var _model = new Lookup();
            return PartialView(_model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create(Person person, string userName)
        {
            Lookup lookup = null;
            UpdateModel(lookup);
            lookup = serv.Create(lookup, userName);

            return Json(new
            {
                sNewRef = _getTabRef(lookup),
                sNewLabel = _getTabLabel(lookup),
                iNewID = (lookup == null ? 0 : lookup.ID)
            },
            JsonRequestBehavior.AllowGet);
        }
        private string _getTabRef(Lookup per)
        {
            if (per != null) return "/Lookup/Edit/" + Convert.ToString(per.ID);
            else return null;
        }
        private string _getTabLabel(Lookup per)
        {
            if (per != null) return per.text_EN;
            else return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            Lookup lookup = serv.Get(id);
            return PartialView(lookup);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id, string userName)
        {
            Lookup lookup = serv.Get(id);
            UpdateModel(lookup);
            serv.Save(lookup, userName);
            return Json(new
            {
                status = "OK"
            },
            JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult View(int id)
        {
            Lookup lookup = serv.Get(id);
            return View(lookup);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, string user)
        {
            serv.Delete(id, user);

            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
