using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public abstract class MacheteApiController : ApiController
    {
        public string subjectFromUser()
        {
            if (User == null) return null;

            var claim = ((ClaimsPrincipal)User).FindFirst(CAType.nameidentifier);
            if (claim == null) return null;

            return claim.Value;
        }
    }
}