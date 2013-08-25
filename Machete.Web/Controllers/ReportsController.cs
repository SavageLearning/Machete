#region COPYRIGHT
// File:     ReportsController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

// how did this get so unbelievably huge?

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Web.Helpers;
using Elmah;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using NLog;
using Machete.Web.ViewModel;
using Machete.Web.Models;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Data.Objects;
using AutoMapper;
using System.Globalization;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ReportsController : MacheteController
    {
        // Initialize the following:
        // I am assuming for the moment that I'll only need what's in ReportService.cs
        private readonly IWorkerSigninRepository wsiRepo;
        private readonly IWorkAssignmentRepository waRepo;
        private readonly IWorkerRepository wRepo;
        // Oh, there's also this, which is the culture info setting:
        CultureInfo CI;

        //Dependency injection (see Global.asax.cs):
        // Again, this is copied from the service layer; Global.asax.cs does not seem to
        // provide configuration for the Controller level, so I'm assuming there's some
        // magic I haven't seen yet.
        public ReportsController(IWorkerSigninRepository wsiRepo,
                             IWorkAssignmentRepository waRepo,
                             IWorkerRepository wRepo)
        {
            this.wsiRepo = wsiRepo;
            this.waRepo = waRepo;
            this.wRepo = wRepo;
        }

        //Not entirely sure what we are initializing. The other controllers do not explain much.
        //However, they all have this.
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// MVC Controller for Machete Reports
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager")]
        #region Index 
        // A simple MVC index view
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region AjaxHandler
        /// <summary>
        /// Provides json grid of first report 
        /// This is temporary -- we will need
        /// multiple Ajax handlers for  the
        /// other reports
        /// </summary>
        /// <param name="param">contains paramters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxSummary(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //
            //pass filter parameters to service level
            dataTableResult<mwdViewData> mvd =
                woServ.CombinedSummary(param.sSearch,
                    Request["sSortDir_0"] == "asc" ? false : true,
                    param.iDisplayStart,
                    param.iDisplayLength);
            //
            //return what's left to datatables
            var result = from d in mvd.query
                         select new[] { System.String.Format("{0:MM/dd/yyyy}", d.date),
                                         d.date.ToString(),
                                         d.TotalSignins > 0 ? d.TotalSignins.ToString() : "0",
                                         d.totalDWCSignins > 0 ? d.totalDWCSignins.ToString(),
                                         d.totalHHHSignins.ToString(),
                                         d.dispatchedDWCSignins.ToString(),
                                         d.dispatchedHHHSignins.ToString(),
                                         d.totalHours.ToString(),
                                         d.totalIncome.ToString()
                         };
                                         p.weekday.ToString(),
                                         p.pending_wo > 0 ? p.pending_wo.ToString(): null,


            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = mvd.totalCount,
                iTotalDisplayRecords = mvd.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ajaxhandler
        /// <summary>
        /// Provides json grid of orders
        /// </summary>
        /// <param name="param">contains parameters for filtering</param>
        /// <returns>JsonResult for DataTables consumption</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = Mapper.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            //Get all the records
            dataTableResult<WorkOrder> dtr = woServ.GetIndexView(vo);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = dtr.totalCount,
                iTotalDisplayRecords = dtr.filteredCount,
                aaData = from p in dtr.query
                         select dtResponse(ref p, param.showOrdersWorkers)
            },
            JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
