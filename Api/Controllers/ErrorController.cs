using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions]
        [AllowAnonymous]
        public ActionResult NotFound(string path)
        {
            //TODO log error to ELMAH
            //ErrorSignal.FromCurrentContext().Raise(new HttpException(404, "404 Not Found: /" + path));

            // return 404
            return NotFound();
        }
    }
}