using AutoMapper;
using Machete.Api.Attributes;
using Machete.Domain;
using Machete.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : MacheteApiController
    {
        private readonly IConfigService serv;
        private readonly IMapper map;

        public ConfigsController(IConfigService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/Configs
        //[ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            var result = serv.GetMany(c => c.publicConfig == true);
            return new JsonResult(new { data = result });
        }

        // GET: api/Configs/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = serv.Get(id);
            return new JsonResult(new { data = result });
        }

        // POST: api/Configs
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Configs/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Configs/5
        [HttpDelete]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public void Delete(int id)
        {
        }
    }
}
