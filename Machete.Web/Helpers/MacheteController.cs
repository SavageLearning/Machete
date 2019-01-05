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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace Machete.Web.Controllers
{
    public class MacheteController : Controller
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly LogEventInfo _levent = new LogEventInfo(LogLevel.Debug, "Controller", "");
        public MacheteSessionState Session;
        
        /// <summary>
        /// Unified exception handling for controllers. NLog and json response.
        /// </summary>
        /// <param name="filterContext"></param>
        protected void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;

            var exceptionMsg = Helpers.GetRootException(filterContext.Exception, ToString());
            var modelerrors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            _levent.Level = LogLevel.Error;

            //
            // Build errorMessage string with detail call-stacks
            //

            StringBuilder errorMessage = new StringBuilder(ToString());

            errorMessage.Append(string.Format(", EXCEPTIONS: {0}", exceptionMsg));
            errorMessage.Append(string.Format(", MODELERR: {0}", modelerrors));
            errorMessage.Append(string.Format(", OUTER_EXCEPTION_STACKTRACE: {0}", filterContext.Exception.StackTrace));

            if (filterContext.Exception.InnerException != null) {
                errorMessage.Append(string.Format(", INNER_EXCEPTION_STACKTRACE: {0}", filterContext.Exception.InnerException.StackTrace));
            }
            
            _levent.Message = errorMessage.ToString();


            var modelStateIdData = ModelState["ID"];
            if (modelStateIdData != null) {
                _levent.Properties["RecordID"] = modelStateIdData.GetHashCode();//.Value.ConvertTo(typeof(int));
            }
            
            _log.Log(_levent);
            filterContext.Result = Json(new
            {
                status = exceptionMsg,
                rtnMessage = exceptionMsg,
                modelErrors = modelerrors,
                jobSuccess = false
            });
            filterContext.ExceptionHandled = true;
        }

        protected virtual void Initialize(ActionContext requestContext)
        {
            throw new NotImplementedException();
        }
    }

    public class MacheteSessionState
    {
        private readonly IDictionary<string, CultureInfo> salad = new Dictionary<string, CultureInfo>();

        public CultureInfo this[string culture]
        {
            get => salad[culture];
            set => salad[culture] = value;
        }
    }

    public static class Helpers
    {
        //// GET: /GetRootException/
        public static string GetRootException(Exception ex, string prefix)
        {
            var exception = ex;
            var messages = prefix + " Exception: \"" + ex.Message + "\"";
            while (exception.InnerException != null)
            {
                messages = messages + "\r\nInner exception: \"" + exception.Message + "\"";
                exception = exception.InnerException;
            }
            messages = messages + "\r\nInnermost exception: \"" + exception.Message + "\"";
            return messages;
        }
    }
}
