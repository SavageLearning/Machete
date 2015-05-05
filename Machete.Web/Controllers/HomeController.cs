#region COPYRIGHT
// File:     HomeController.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Service;
using System.Web.Helpers;
using System.Globalization;
using Machete.Web.Helpers;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Changes()
        {
            return PartialView();
        }
        public ActionResult About()
        {
            return PartialView();
        }
        public ActionResult Issues()
        {
            return PartialView();
        }
        public ActionResult Wiki()
        {
            return PartialView();
        }
        public ActionResult Docs()
        {
            return PartialView();
        }      
    }   
}
