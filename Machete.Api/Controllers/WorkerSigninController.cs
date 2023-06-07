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

using System;
using AutoMapper;
using Machete.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Machete.Domain;
using Machete.Api.ViewModel;
using System.Collections.Generic;
using WorkerSigninListVM = Machete.Api.ViewModel.WorkerSigninListVM;

namespace Machete.Api.Controllers
{
    public class WorkerSigninController : MacheteApiController<WorkerSignin, WorkerSigninVM, WorkerSigninListVM>
    {
        private new readonly IWorkerSigninService service;

        public WorkerSigninController(
            IWorkerSigninService serv,
            IMapper map) : base(serv, map)
        {
            service = serv;
        }

        // GET: /WorkerSignin/Delete/5
        /// <summary>
        /// This method deletes a signin from the master Worker Signins list for the day.
        /// </summary>
        /// <param name="id">The Worker ID of the worker.</param>
        /// <param name="userName">The user performing the action.</param>
        /// <returns>Json (bool jobSuccess, string status, int deletedID)</returns>
        [HttpDelete, Authorize(Roles = "Administrator, Manager, Check-in")]
        public new ActionResult Delete(int id)
        {
            var record = service.Get(id);
            if (record.WorkAssignmentID != null)
            {
                return StatusCode(400, new Exception("You cannot delete a signin that has been associated with an Assignment. Disassociate the sigin with the assignment first."));
            }
            return base.Delete(id);
        }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Check-in")]
        public new ActionResult<IEnumerable<WorkerSigninListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams
            )
        {
            return base.Get(apiRequestParams);
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
        [HttpPost, Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveDown(int id)
        {
            service.moveDown(id, UserEmail);
            return Ok();
        }

        /// <summary>
        /// This method invokes IWorkerSigninService.moveUp,
        /// which moves a worker up in numerical order in the
        /// daily ('lottery') list,
        /// and moves the prece    eding set member into their spot.
        /// </summary>
        /// <param name="id">The Worker ID of the person to be moved down.</param>
        /// <param name="userName">The username of the person making the request.</param>
        /// <returns>Json (bool jobSuccess, string status)</returns>
        [HttpPost, Authorize(Roles = "Administrator, Manager")]
        public ActionResult moveUp(int id)
        {
            service.moveUp(id, UserEmail);
            return Ok();
        }
    }
}
