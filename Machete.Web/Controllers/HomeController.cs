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
using Machete.Web.Helpers;
using System.Web.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Hirer"))
                {
                    return RedirectToAction("Index", "HirerWorkOrder");
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Changes()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult About()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Issues()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Wiki()
        {
            return PartialView();
        }

        [Authorize(Roles = "Manager, Administrator, PhoneDesk, User, Teacher, Check-in")]
        public ActionResult Docs()
        {
            return PartialView();
        }      
    }   
}
