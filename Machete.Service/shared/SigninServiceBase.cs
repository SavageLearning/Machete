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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Globalization;

namespace Machete.Service
{
    public interface ISigninService<T> : IService<T> where T : Signin
    {
        Image getImage(int dwccardnum);        
    }
    public abstract class SigninServiceBase<T> : ServiceBase<T> where T : Signin
    {
        protected readonly IWorkerRepository wRepo;
        protected readonly IWorkerRequestRepository wrRepo;
        protected readonly IImageRepository iRepo;
        protected readonly IWorkerCache wcache;
        //
        //
        protected SigninServiceBase(
            IRepository<T> repo,
            IWorkerRepository wRepo,
            IImageRepository iRepo,
            IWorkerRequestRepository wrRepo,
            IWorkerCache wc,
            IUnitOfWork uow)
            : base(repo, uow)
        {
            this.wRepo = wRepo;
            this.wrRepo = wrRepo;
            this.iRepo = iRepo;
            this.wcache = wc;
            this.logPrefix = "SigninServiceBase";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardrequest"></param>
        /// <returns></returns>
        public Image getImage(int cardrequest)
        {
            Worker w_query = wRepo.GetAllQ().Where(w => w.dwccardnum == cardrequest).AsEnumerable().FirstOrDefault();
            if (w_query == null) return null;
            if (w_query.ImageID != null)
            {
                return iRepo.Get(i => i.ID == w_query.ImageID);
            }
            return null;
        } 
    }
}