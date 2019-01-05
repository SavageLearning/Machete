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

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Machete.Web.Helpers
{
    /// <summary>
    /// SelectListItem with extra properties for WorkAssignment skill dropdown
    /// </summary>
    public class SelectListItemEx : SelectListItem
    {
        public string wage { get; set; }
        public string minHour { get; set; }
        public string fixedJob { get; set; }
    }

    public class SelectListEmployerSkills : SelectListItem
    {
        public double wage { get; set; }
        public int minHour { get; set; }
        public int ID { get; set; }
        public int typeOfWorkID { get; set; }
        public string skillDescriptionEs { get; set; }
        public string skillDescriptionEn { get; set; }
    }

    public class SelectListItemEmail : SelectListItem
    {
        public string template { get; set; }
    }
}
