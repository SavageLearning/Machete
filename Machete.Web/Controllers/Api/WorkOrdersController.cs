using System;
using System.Linq;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Machete.Web.Controllers.Api
{
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrdersController : MacheteApiController
    {
        private readonly IWorkOrderService _serv;
        private readonly IMapper _map;
        private readonly TimeZoneInfo _clientTimeZoneInfo;

        public WorkOrdersController(IWorkOrderService workOrderService, ITenantService tenantService,
            IMapper map)
        {
            _serv = workOrderService;
            _map = map;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }


        // GET api/values
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = UserSubject;
            dataTableResult<DTO.WorkOrdersList> list = _serv.GetIndexView(vo);
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var result = list.query
                .Select(
                    e => _map.Map<DTO.WorkOrdersList, ViewModel.Api.WorkOrder>(e)
                ).AsEnumerable();            
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
        
            var result = _map.Map<Domain.WorkOrder, ViewModel.Api.WorkOrder>(_serv.Get(id));
            return new JsonResult(new { data = result });
        }

        // POST api/values
        [Authorize(Roles = "Administrator")]
        [HttpPost("")]
        public void Post([FromBody]ViewModel.Api.WorkOrder order)
        {
            var domain = _map.Map<ViewModel.Api.WorkOrder, Domain.WorkOrder>(order);
            _serv.Save(domain, UserEmail);
        }

        // PUT api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ViewModel.Api.WorkOrder order)
        {
            var domain = _serv.Get(order.id);
            // TODO employers must only be able to edit their record
            _map.Map<ViewModel.Api.WorkOrder, Domain.WorkOrder>(order, domain);
            _serv.Save(domain,UserEmail);
        }

        // DELETE api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
