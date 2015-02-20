#region COPYRIGHT
// File:     IsInRole.cs
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

namespace Machete.Web.Helpers
{
    //Take from stackoverflow.com discussion
    //http://stackoverflow.com/questions/4649795/hiding-column-in-table-based-on-role-in-mvc
    //
    public static class IsInRoleHelper
    {
        private static Dictionary<string, string[]> _functionalityRole = new Dictionary<string, string[]> {
            {"Worker.Edit.WorkerInfo.ShowTab", new[] { "Administrator", "Manager" }},
            {"Employer.CreateNew.ShowTab", new[] { "Administrator", "Manager", "PhoneDesk"}}
        };

        public static bool IsInRole(this HtmlHelper instance, params string[] roles)
        {
            var user = instance.ViewContext.HttpContext.User;
            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                    return true;
            }
            return false;
        }

        public static bool IsRoleAllowed(this HtmlHelper instance, string functionality)
        {
            var user = instance.ViewContext.HttpContext.User;
            foreach (var role in _functionalityRole[functionality])
            {
                if (user.IsInRole(role))
                    return true;
            }
            return false;
        }
    }
    // TODO: RoleGroupHelper relies on magic strings; replace magic strings
    public static class RoleGroupHelper
    {
        public static string[] Role_AMPCU(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk", "Check-in", "User"}; }
        public static string[] Role_AMPU(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk", "User"}; }
        public static string[] Role_AMP(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk"}; }
        public static string[] Role_AM(this HtmlHelper html) { return new[] { "Administrator", "Manager" }; }
        public static string[] Role_A(this HtmlHelper html) {return new[]{"Administrator"}; }
        public static string[] Role_T(this HtmlHelper html) { return new[] { "Teacher" }; }
        public static string[] Role_H(this HtmlHelper html) { return new[] { "Hirer" }; }
    }  
}