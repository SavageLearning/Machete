using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Machete.Web.Helpers;

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
            levent.Level = LogLevel.Error; levent.Message = this.ToString() + "EXCEPTIONS:" + exceptionMsg + "MODELERR:" + modelerrors;
            levent.Properties["RecordID"] = ModelState["ID"]; log.Log(levent);
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

}