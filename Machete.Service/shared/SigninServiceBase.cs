#region COPYRIGHT
// File:     SigninServiceBase.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Service
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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Linq;

namespace Machete.Service
{
    public interface ISigninService<T> : IService<T> where T : Signin
    {
        string getImageRef(int dwccardnum);        
    }
    public abstract class SigninServiceBase<T> : ServiceBase<T> where T : Signin
    {
        protected readonly IWorkerService wServ;
        protected readonly IWorkerRequestService wrServ;
        protected readonly IImageService iServ;
        protected readonly IMapper map;
        protected readonly IConfigService cfg;
        //
        //
        protected SigninServiceBase(
            IRepository<T> repo,
            IWorkerService wServ,
            IImageService iServ,
            IWorkerRequestService wrServ,
            IUnitOfWork uow,
            IMapper map,
            IConfigService cfg)
            : base(repo, uow)
        {
            this.wServ = wServ;
            this.wrServ = wrServ;
            this.iServ = iServ;
            this.map = map;
            this.logPrefix = "SigninServiceBase";
            this.cfg = cfg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardrequest"></param>
        /// <returns></returns>
        public string getImageRef(int cardrequest)
        {
            // TODO2017: Make no-image-available.jpg configurable form the web UI
            string imageRef = "/Content/images/NO-IMAGE-AVAILABLE.jpg";
            Worker w_query = wServ.GetAll().Where(w => w.dwccardnum == cardrequest).FirstOrDefault();
            if (w_query == null) return imageRef;
            if (w_query.ImageID != null)
            {
                imageRef = "/Image/GetImage/" + w_query.ImageID;
            }
            return imageRef;
        } 
    }
}