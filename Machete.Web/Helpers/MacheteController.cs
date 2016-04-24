#region COPYRIGHT
// File:     MacheteController.cs
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
using NLog;
using Machete.Web.Helpers;
using Machete.Service;
using Machete.Domain;
using System.Text;

namespace Machete.Web.Controllers
{
    public class MacheteController : Controller
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "Controller", "");
        /// <summary>
        /// Unified exception handling for controllers. NLog and json response.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            string exceptionMsg = "";
            string modelerrors = null;
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            exceptionMsg = RootException.Get(filterContext.Exception, this.ToString());
            modelerrors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            levent.Level = LogLevel.Error;

            //
            // Build errorMessage string with detail call-stacks
            //

            StringBuilder errorMessage = new StringBuilder(this.ToString());

            errorMessage.Append(string.Format(", EXCEPTIONS: {0}", exceptionMsg));
            errorMessage.Append(string.Format(", MODELERR: {0}", modelerrors));
            errorMessage.Append(string.Format(", OUTER_EXCEPTION_STACKTRACE: {0}", filterContext.Exception.StackTrace));

            if (filterContext.Exception.InnerException != null) {
                errorMessage.Append(string.Format(", INNER_EXCEPTION_STACKTRACE: {0}", filterContext.Exception.InnerException.StackTrace));
            }
            
            levent.Message = errorMessage.ToString();


            ModelState modelStateIdData = ModelState["ID"];
            if (modelStateIdData != null) {
                levent.Properties["RecordID"] = modelStateIdData.Value.ConvertTo(typeof(int));
            }
            
            log.Log(levent);
            filterContext.Result = Json(new
            {
                status = exceptionMsg,
                rtnMessage = exceptionMsg,
                modelErrors = modelerrors,
                jobSuccess = false
            }, JsonRequestBehavior.AllowGet);
            filterContext.ExceptionHandled = true;
        }
    }
    public static class RootException
    {
        ////
        //// GET: /GetRootException/
        public static string Get(Exception e, string prefix)
        {
            Exception ee = e;
            string messages = prefix + " Exception: \"" + e.Message + "\"";
            while (ee.InnerException != null)
            {
                messages = messages + "\r\nInner exception: \"" + ee.Message + "\"";
                ee = ee.InnerException;
            }
            messages = messages + "\r\nInnermost exception: \"" + ee.Message + "\"";
            return messages;
        }
    }
}