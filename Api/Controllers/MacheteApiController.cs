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
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            userSubject = subjectFromUser();
            base.Initialize(controllerContext);
        }

        private string subjectFromUser()
        {
            if (User == null) return null;

            var claim = ((ClaimsPrincipal)User).FindFirst(CAType.nameidentifier);
            if (claim == null) return null;

            return claim.Value;
        }
    }
}