#region COPYRIGHT
// File:     ListItemToOption.cs
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
using System.Text;
using System.Web.Mvc.Resources;

namespace System.Web.Mvc.Html
{
    public static class SelectExtensions
    {
        /// <summary>
        /// Handles extra attributes for WorkAssignment skill dropdown
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static string ListItemToOption(SelectListItemEx item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            if (item.wage != null)
            {
                builder.Attributes["wage"] = item.wage;
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}

namespace System.Web.Mvc
{
    /// <summary>
    /// SelectListItem with extra properties for WorkAssignment skill dropdown
    /// </summary>
    public class SelectListItemEx : SelectListItem
    {
        public string wage
        {
            get;
            set;
        }
        public string minHour
        {
            get;
            set;
        }
        public string fixedJob
        {
            get;
            set;
        }
    }
}