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
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class HomeController : Controller
    {
        [Authorize] // if unauthenticated, bounce back to login until we figure it out
        public ActionResult Index()
        {
            if (User.IsInRole("Hirer")) return Redirect("/V2/Onlineorders");
            return View();
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

        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#set-the-culture-programmatically
        // https://github.com/aspnet/Docs/blob/master/aspnetcore/fundamentals/localization/sample/Localization/Controllers/HomeController.cs#L58
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }   
}
