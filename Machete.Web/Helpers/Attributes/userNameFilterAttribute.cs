using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Helpers
{
    /// <summary>
    /// Controller decorator to handle UserName
    /// </summary>
	public class UserNameFilter : ActionFilterAttribute
	{
	    public override void OnActionExecuting(ActionExecutingContext filterContext)
	    {
	        const string Key = "userName";
	 
	        if (filterContext.ActionParameters.ContainsKey(Key))
	        {
	            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
	            {
	                filterContext.ActionParameters[Key] = filterContext.HttpContext.User.Identity.Name;
	            }
	        }	 
	        base.OnActionExecuting(filterContext);
	    }
	}
}