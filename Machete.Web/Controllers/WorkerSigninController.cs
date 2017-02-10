#region COPYRIGHT
// File:     WorkerSigninController.cs
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
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerSigninController : MacheteController
    {
        private readonly IWorkerSigninService _serv;
        private readonly IWorkerService _wServ;
        private readonly LookupCache lcache;
        private readonly IMapper map;
        private readonly IDefaults def;
        private System.Globalization.CultureInfo CI;        
        public WorkerSigninController(IWorkerSigninService workerSigninService, 
                                      IWorkerService workerService, 
                                      LookupCache lc,
            IDefaults def,
            IMapper map)
        {
            this._serv = workerSigninService;
            this._wServ = workerService;
            this.lcache = lc;
            this.map = map;
            this.def = def;
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
            return View();
        }
        //
        // POST: /WorkerSignin/Index -- records a signin
        [HttpPost]
        [Authorize(Roles = "Manager, Administrator, Check-in")]
        public ActionResult Index(int dwccardnum, DateTime dateforsignin, string userName)
        {
            Worker worker = _wServ.GetWorkerByNum(dwccardnum);
            if (worker == null) throw new NullReferenceException("Card ID doesn't match a worker!");
            var _signin = new WorkerSignin();
            string result = "";
            // The card just swiped
            _signin.dwccardnum = dwccardnum;
            _signin.dateforsignin = new DateTime(dateforsignin.Year, dateforsignin.Month, dateforsignin.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            _signin.memberStatusID = worker.memberStatusID;

            if (_signin.lottery_timestamp == null)
            {
                _signin.lottery_sequence = _serv.GetNextLotterySequence(dateforsignin);
                _signin.lottery_timestamp = DateTime.Now;
                _serv.Save(_signin, userName);
            }  
            //
            //
            try
            {
                _serv.CreateSignin(_signin, this.User.Identity.Name);
            }
            catch(InvalidOperationException eek)
            {
                result = dwccardnum.ToString() + " " + eek.Message;
            }

            //Get picture from checkin, show with next view
            Image checkin_image = _serv.getImage(dwccardnum);           
            string imageRef = "/Content/images/NO-IMAGE-AVAILABLE.jpg";
            if (checkin_image != null)
            {
                imageRef = "/Image/GetImage/" + checkin_image.ID;
            }

            if (result.Length == 0)
                result = "Success!";

            return Json(new
            {
                // TODO2017: replace this with viewmodel 
                memberExpired = worker.isExpired,
                memberInactive = worker.isInactive,
                memberSanctioned = worker.isSanctioned,
                memberExpelled = worker.isExpelled,
                message = result,
                imageRef = imageRef,
                expirationDate = worker.memberexpirationdate
            },
            JsonRequestBehavior.AllowGet);
        }
            },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method invokes IWorkerSigninService.moveDown,
        /// which moves a worker down in numerical order in the daily 
        /// ('lottery') list,
        /// and moves the proceeding (next) set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveDown(int id, string userName)
        {
            _serv.moveDown(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK", 
                workerID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method invokes IWorkerSigninService.moveUp,
        /// which moves a worker up in numerical order in the 
        /// daily ('lottery') list,
        /// and moves the preceeding set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveUp(int id, string userName)
        {
            _serv.moveUp(id, userName);
            return Json(new
            {
                jobSuccess = true,
                status = "OK",
                workerID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        // GET: /WorkerSignin/Delete/5
       /// <summary>
       /// This method deletes a signin from the master Worker Signins list for the day.
       /// </summary>
       /// <param name="id">The Worker ID of the worker.</param>
       /// <param name="userName">The user performing the action.</param>
       /// <returns>Json (bool jobSuccess, string status, int deletedID)</returns>
        [UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public JsonResult Delete(int id, string userName)
        {
            var record = _serv.Get(id);
            if (record.lottery_sequence != null)
            {
                return Json(new
                    {
                        jobSuccess = false,
                        rtnMessage = "You cannot delete a signin that has already been added to the daily list. Remove the signin from the daily list and try again."
                    },
                    JsonRequestBehavior.AllowGet);
            }
            else
            { 
                _serv.Delete(id, userName);            
                return Json(new
                {
                    jobSuccess = true,
                    status = "OK",
                    deletedID = id
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Administrator, Manager, Check-in")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<Service.DTO.WorkerSigninList> was = _serv.GetIndexView(vo);
            var result = was.query
                .Select(
                    e => map.Map<DTO.WorkerSigninList, ViewModel.WorkerSigninList>(e)
                ).AsEnumerable();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = was.totalCount,
                iTotalDisplayRecords = was.filteredCount,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        //TODO: rework into model 
        //private string _getSkillCodes(int eng, int? sk1, int? sk2, int? sk3)
        //{
        //    string rtnstr = "E" + eng + " ";
        //    if (sk1 != null)
        //    {
        //        var lookup = lcache.getByID((int)sk1);
        //        rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
        //    }

        //    if (sk2 != null)
        //    {
        //        var lookup = lcache.getByID((int)sk2);
        //        rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
        //    }

        //    if (sk3 != null)
        //    {
        //        var lookup = lcache.getByID((int)sk3);
        //        rtnstr = rtnstr + lookup.ltrCode + lookup.level;
        //    }
        //    return rtnstr;
        //}
    }
}
