using AutoMapper;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class TransportRulesController : ApiController
    {
        private readonly ITransportRuleService serv;
        private readonly IMapper map;

        public TransportRulesController(ITransportRuleService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/TransportRule
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        public IHttpActionResult Get()
        {
            var result = serv.GetAll();
            // TODO this is a hacky workaround until I have viewmodels
            result.Select(a => a.costRules.Select(b => { b.transportRule = null; return b; }).ToList()).ToList();
            return Json(new { data = result });
        }

        // GET: api/TransportRule/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TransportRule
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TransportRule/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TransportRule/5
        public void Delete(int id)
        {
        }
    }
}
