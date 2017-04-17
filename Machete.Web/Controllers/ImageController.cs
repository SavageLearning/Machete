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
//using Machete.Helpers;
using Machete.Service;
using Machete.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ImageController : MacheteController
    {
        private readonly IImageService _serv;
        //
        // GET: /Image/
        public ImageController(IImageService imageService)
        {
            this._serv = imageService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator, Check-in")]
        /// 
        public FileContentResult GetImage(int ID)
        {
            if (ID != 0)
            {
                Image image = _serv.Get(ID);
                return File(image.ImageData, image.ImageMimeType, image.filename);
            }
            else
            {
                return null; //File(new byte [1], "");
            }
        }
    }
}
