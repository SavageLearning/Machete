using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : MacheteApiController
    {
        private readonly IConfigService _serv;
        private readonly IMapper _map; 

        public ConfigsController(IConfigService serv, IMapper map)
        {
            this._serv = serv;
            this._map = map;
        }

        // GET: api/Configs
        //[ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            var result = _serv.GetMany(c => c.publicConfig == true);
            return new JsonResult(new { data = result });
        }

        // GET: api/Configs/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var result = _serv.Get(id);
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
