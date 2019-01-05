#region COPYRIGHT
// File:     WorkerController.cs
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

using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class WorkerController : MacheteController
    {
        private readonly IWorkerService serv;
        private readonly IImageService imageServ;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;

        public WorkerController(IWorkerService workerService, 
                                IPersonService personService,
                                IImageService  imageServ,
            IDefaults def,
            IMapper map)
        {
            serv = workerService;
            this.imageServ = imageServ;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = Session["Culture"];
            // TODO this needs to be scheduled elsewhere
            //serv.ExpireMembers();
            //serv.ReactivateMembers();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Administrator, Teacher, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, Teacher, PhoneDesk")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<WorkerList> list = serv.GetIndexView(vo);
            var result = list.query
            .Select(
                e => map.Map<WorkerList, ViewModel.WorkerList>(e)
            ).AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wkr"></param>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")] 
        public ActionResult Create(int ID)
        {
            // TODO handle exception of next worker number
            var nextnum = serv.GetNextWorkerNum();
            var w = map.Map<Worker, ViewModel.Worker>(new Worker {
                ID = ID,
                dwccardnum = nextnum
            });
            w.def = def;
            return PartialView("Create", w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="userName"></param>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")]
        public async Task<ActionResult> Create(Worker worker, string userName, IFormFile imagefile)
        {
            if (await TryUpdateModelAsync(worker)) {
                if (imagefile != null) await updateImage(worker, imagefile);
                Worker newWorker = serv.Create(worker, userName);
                var result = map.Map<Worker, ViewModel.Worker>(newWorker);
                return Json(new {
                    sNewRef = result.tabref,
                    sNewLabel = result.tablabel,
                    iNewID = result.ID,
                    jobSuccess = true
                });
            } else {
                return Json(new {jobSuccess = "false"});
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")] 
        public ActionResult Edit(int id)
        {
            Worker w = serv.Get(id);
            var m = map.Map<Worker, ViewModel.Worker>(w);
            m.def = def;
            return PartialView(m);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_model"></param>
        /// <param name="userName"></param>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")]
        public async Task<ActionResult> Edit(int id, Worker _model, string userName, IFormFile imagefile)
        {
            Worker worker = serv.Get(id);
            if (await TryUpdateModelAsync(worker)) {
            
            if (imagefile != null) await updateImage(worker, imagefile);                
            serv.Save(worker, userName);
            return Json(new
            {
                jobSuccess = true
            });
            } else { return Json(new { jobSuccess = false }); }
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
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="imageFile"></param>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator")]
        private async Task updateImage(Worker worker, IFormFile imageFile)
        {
            var userIdentity = new ClaimsIdentity("Cookies");
            if (worker == null) throw new MacheteNullObjectException("updateImage called with null worker");
            if (imageFile == null) throw new MacheteNullObjectException("updateImage called with null imagefile");
            if (worker.ImageID != null)
            {
                var image = imageServ.Get((int)worker.ImageID);
                image.ImageMimeType = imageFile.ContentType;
                image.parenttable = "Workers";
                image.filename = imageFile.FileName;
                image.recordkey = worker.ID.ToString();
                image.ImageData = new byte[imageFile.Length];

                using (var memoryStream = new MemoryStream()) {
                    await imageFile.CopyToAsync(memoryStream);
                    image.ImageData = memoryStream.ToArray();
                }

                imageServ.Save(image, userIdentity.Name);
            }
            else
            {
                var image = new Image();
                image.ImageMimeType = imageFile.ContentType;
                image.parenttable = "Workers";
                image.recordkey = worker.ID.ToString();
                image.ImageData = new byte[imageFile.Length];

                using (var memoryStream = new MemoryStream()) {
                    await imageFile.CopyToAsync(memoryStream);
                    image.ImageData = memoryStream.ToArray();
                }
                
                Image newImage = imageServ.Create(image, userIdentity.Name);
                worker.ImageID = newImage.ID;
            }
        }
    }
}