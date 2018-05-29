using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
{
    public string ClaimType { get; set; }
    public string[] ClaimValue { get; set; }

    public override Task OnAuthorizationAsync(
        HttpActionContext actionContext, 
        System.Threading.CancellationToken cancellationToken)
    {

        var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

        if (principal.Identity.IsAuthenticated)
        {
            foreach (var cv in ClaimValue)
            {
                if (cv == "Any" || principal.HasClaim(x => x.Type == ClaimType && x.Value == cv))
                {
                    return Task.FromResult<object>(null);
                }
            }
        }

        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        return Task.FromResult<object>(null);
    }

}