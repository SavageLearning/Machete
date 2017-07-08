using Machete.Data;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
{
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }

    public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
    {

        var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

        if (!principal.Identity.IsAuthenticated)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            return Task.FromResult<object>(null);
        }

        if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue)))
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            return Task.FromResult<object>(null);
        }

        //User is Authorized, complete execution
        return Task.FromResult<object>(null);

    }
}