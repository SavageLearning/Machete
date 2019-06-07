using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions]
        [Route("{path}")]
        public ActionResult NotFound(string path)
        {
            return NotFound();
        }
    }
}