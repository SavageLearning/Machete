#region COPYRIGHT
// File:     ActivitySigninController.cs
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

using System;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Machete.Api.ViewModel;
using ActivitySigninListVM = Machete.Api.ViewModel.ActivitySigninListVM;
using System.Collections.Generic;

namespace Machete.Api.Controllers
{
    public class ActivitySigninController : MacheteApiController<ActivitySignin, ActivitySigninVM, ActivitySigninListVM>
    {
        private readonly IActivitySigninService serv;

        public ActivitySigninController(
            IActivitySigninService serv,
            IMapper map
        ) : base(serv, map)
        { }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Check-in, Teacher")]
        public new ActionResult<IEnumerable<ActivitySigninListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams
            )
        {
            return base.Get(apiRequestParams);
        }

        [HttpPost, Authorize(Roles = "Manager, Administrator, Check-in, Teacher")]
        public ActionResult Post(int dwccardnum, int activityID)
        {
            var _asi = new ActivitySignin();
            var utcTime = DateTime.UtcNow;

            _asi.dateforsignin = utcTime;
            _asi.activityID = activityID;
            _asi.dwccardnum = dwccardnum;

            //Get picture from checkin, show with next view
            string imageRef = serv.getImageRef(dwccardnum);

            Worker w;
            try
            {
                w = serv.CreateSignin(_asi, UserEmail);
            }
            catch (NullReferenceException)
            {
                return StatusCode(500, new Exception("Failed to Create Signin record"));
            }
            return Ok(new
            {
                memberExpired = w.isExpired,
                memberInactive = w.isInactive,
                memberSanctioned = w.isSanctioned,
                memberExpelled = w.isExpelled,
                imageRef,
                expirationDate = w.memberexpirationdate
            });
        }

        [HttpDelete, Authorize(Roles = "Administrator, Manager, Check-in, Teacher")]
        public ActionResult<ActivitySigninVM> Delete(int id, string userName)
        {
            return base.Delete(id);
        }
    }
}
