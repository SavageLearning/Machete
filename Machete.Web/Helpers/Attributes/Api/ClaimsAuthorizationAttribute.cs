using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Machete.Web.Helpers.Api
{

    //https://stackoverflow.com/a/41348219/2496266
    public class ClaimsAuthorizationAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizationAttribute(string claimType, string[] claimValues) : base(
            typeof(ClaimRequirementFilter))
        {
            var claims = claimValues.Select(value => new Claim(claimType, value)).ToList();
            Arguments = new object[] {claims};
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim[] _claims;

        public ClaimRequirementFilter(Claim[] claims)
        {
            _claims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = false;
            foreach (var claim in _claims) {
                if (context.HttpContext.User.Claims.Any(c => c.Type == claim.Value))
                    hasClaim = true;
            }

            if (!hasClaim) {
                context.Result = new ForbidResult();
            }
        }
    }
}
