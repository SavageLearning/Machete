using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Machete.Api.Controllers
{
    public abstract class MacheteApiController : ApiController
    {
        public string userSubject;
        public string userEmail;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            userSubject = subjectFromUser();
            userEmail = emailFromUser();
            base.Initialize(controllerContext);
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