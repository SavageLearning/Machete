#region COPYRIGHT
// File:     ImageController.cs
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

using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ImageController : MacheteController
    {
        private readonly IImageService serv;
        //
        // GET: /Image/
        public ImageController(IImageService imageService)
        {
            serv = imageService;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            var httpContext = requestContext.HttpContext;
            var currentCulture = httpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var currentUrl = UriHelper.BuildRelative(httpContext.Request.PathBase, httpContext.Request.Path, httpContext.Request.QueryString);        }
        /// <summary>
        /// Get an image from the database as file content.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator, Check-in")]
        /// 
        public FileContentResult GetImage(int ID)
        {
            if (ID != 0)
            {
                Image image = serv.Get(ID);
                return File(image.ImageData, image.ImageMimeType, image.filename);
            }

            return null; //File(new byte [1], "");
        }
    }
}
