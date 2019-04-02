using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/error")] // TODO is this doing anything?
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions]
        [Route("{path}")]
        public ActionResult NotFound(string path)
        {
            // TODO ELMAH
            //ErrorSignal.FromCurrentContext().Raise(new HttpException(404, "404 Not Found: /" + path));

            // return 404
            return NotFound();
        }
    }
}