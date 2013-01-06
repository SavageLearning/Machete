#region COPYRIGHT
// File:     Combres.cs
// Author:   Savage Learning, LLC.
// Created:  2012/11/03 
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
[assembly: WebActivator.PreApplicationStartMethod(typeof(Machete.Web.App_Start.CombresHook), "PreStart")]
namespace Machete.Web.App_Start {
	using System.Web.Routing;
	using Combres;
	
    public static class CombresHook {
        public static void PreStart() {
            //RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}