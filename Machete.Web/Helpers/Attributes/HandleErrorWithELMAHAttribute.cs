#region COPYRIGHT
// File:     HandleErrorWithELMAHAttribute .cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// ReSharper disable once CheckNamespace
namespace Machete.Web.Helpers
{
    //From http://stackoverflow.com/questions/766610/
    public class ElmahHandleErrorAttribute : ExceptionFilterAttribute
    {
        // DO NOT MODIFY this class, it's been taken apart to make MVC Core work
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var e = context.Exception;
            if (!context.ExceptionHandled   // if unhandled, will be logged anyhow
                    || RaiseErrorSignal(e, context)      // prefer signaling, if possible
                    || IsFiltered(context))     // filtered?
                return;

            LogException(e);
        }

        private static bool RaiseErrorSignal(Exception e, ActionContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext == null)
                return false;
//            var signal = ErrorSignal.FromContext(httpContext);
//            if (signal == null)
//                return false;
//            signal.Raise(e, httpContext);
            return true;
        }

        private static bool IsFiltered(ExceptionContext context)
        {
//            var config = context.HttpContext.GetSection("elmah/errorFilter")
//                                     as ErrorFilterConfiguration;

//            if (config == null)
//                return false;

//            var testContext = new ErrorFilterModule.AssertionHelperContext(
//                                                                context.Exception, HttpContext.Current);
//
//            return config.Assertion.Test(testContext);
            return false;
        }

        private static void LogException(Exception e)
        {
//            var context = HttpContext.Current;
//            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}
