using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportRulesController : MacheteApiController<TransportRule, TransportRuleVM, TransportRuleListVM>
    {
        private readonly ITransportCostRuleService _costServ;
        public TransportRulesController(
            ITransportRuleService serv,
            ITransportCostRuleService costServ,
            IMapper map) : base(serv, map)
        {
            _costServ = costServ;
        }

        // GET: api/TransportRule
        [HttpGet, Authorize(Roles = "Administrator, Hirer")]
        public ActionResult<IEnumerable<TransportRuleVM>> Get()
        {
            try
            {
                var result = service.GetAll()
                    .Select(e => map.Map<Domain.TransportRule, TransportRuleVM>(e))
                    .ToList();

                var transportCostRules = _costServ.GetAll().ToList();
                foreach (var p in result)
                {
                    var pCostRules = transportCostRules.Where(costRule => costRule.transportRuleID == p.id).ToList();
                    p.costRules = map.Map<List<TransportCostRuleVM>>(pCostRules);
                }

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<TransportRuleVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<TransportRuleVM> Post([FromBody] TransportRuleVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<TransportRuleVM> Put(int id, [FromBody] TransportRuleVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }
    }
}
