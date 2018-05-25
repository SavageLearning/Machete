using AutoMapper;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class ConfigsController : MacheteApiController
    {
        private readonly IConfigService serv;
        private readonly IMapper map;
        // GET: api/Configs

        public ConfigsController(IConfigService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }
        //[ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var result = serv.GetMany(c => c.publicConfig == true);
            return Json(new { data = result });
        }

        // GET: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public IHttpActionResult Get(int id)
        {
            var result = serv.Get(id);
            return Json(new { data = result });
        }

        // POST: api/Configs
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Delete(int id)
        {
        }
    }
}
