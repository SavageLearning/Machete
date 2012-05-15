using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Helpers
{
    public class AuthorizeExAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(AuthorizationContext filterContext)
        {
            // If result is null, we're authorized
            if (filterContext.Result == null) return;

            // If here, you're getting an HTTP 401 status code
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                
                ViewResult result = new ViewResult();
                result.ViewName = "Error";
                filterContext.Result = result;
            }
        }

    }
}