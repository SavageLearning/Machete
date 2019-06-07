using System;
using System.Configuration;
using System.Linq;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Machete.Web.Controllers.Api
{
    [Route("api/configs")]
    [ApiController]
    public class ConfigsController : MacheteApiController
    {
        private readonly IConfigService _serv;
        private IConfiguration _configuration;

        public ConfigsController(IConfigService serv, IConfiguration configuration)
        {
            _serv = serv;
            _configuration = configuration;
        }

        // GET: api/Configs
        //[Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            var result = _serv.GetMany(c => c.publicConfig).ToList();
            
            var facebookConfig = new Config
            {
                key = "FacebookAppId",
                value = _configuration["Authentication:Facebook:AppId"]
            };
            var googleConfig = new Config
            {
                key = "GoogleClientId",
                value = _configuration["Authentication:Google:ClientId"]
            };
            var state = new Config
            {
                key = "OAuthStateParameter",
                //TODO value = this.httpContext.setCookie(foo, bar) => urlencode(bar)
                value = _configuration["Authentication:State"]
            };
            
            result.Add(facebookConfig);
            result.Add(googleConfig);
            result.Add(state);
            
            return new JsonResult(new { data = result });
        }

        // GET: api/Configs/5
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            var result = _serv.Get(id);
            return new JsonResult(new { data = result });
        }

        // POST: api/Configs
        [Authorize(Roles = "Administrator")]
        [HttpPost("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Configs/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Configs/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
