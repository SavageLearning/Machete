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
            ControllerContext = controllerContext;
        }
        
        [NonAction]
        public string subjectFromUser()
        {
            return User?.FindFirst(CAType.nameidentifier)?.Value;
        }

        [NonAction]
        public string emailFromUser()
        {
            return User?.FindFirst(CAType.email)?.Value;
        }
    }
}