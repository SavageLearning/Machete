using System.Security.Claims;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MacheteApiController : ControllerBase
    {
        public string userSubject;
        public string userEmail;
        protected void Initialize(ControllerContext controllerContext)
        {
            userSubject = subjectFromUser();
            userEmail = emailFromUser();
            base.ControllerContext = controllerContext;
        }
        [NonAction]
        public string subjectFromUser()
        {
            if (User == null) return null;

            var claim = ((ClaimsPrincipal)User).FindFirst(CAType.nameidentifier);
            if (claim == null) return null;

            return claim.Value;
        }

        [NonAction]
        public string emailFromUser()
        {
            if (User == null) return null;

            var claim = ((ClaimsPrincipal)User).FindFirst(CAType.email);
            if (claim == null) return null;

            return claim.Value;
        }
    }
}