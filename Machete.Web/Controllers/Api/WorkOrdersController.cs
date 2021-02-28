using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Machete.Web.Controllers.Api
{
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrdersController : MacheteApi2Controller<WorkOrder, WorkOrderVM>
    {
        private readonly IWorkOrderService serv;
        private readonly TimeZoneInfo _clientTimeZoneInfo;

        public WorkOrdersController(IWorkOrderService serv, ITenantService tenantService,
            IMapper map) : base(serv, map)
        {
            this.serv = serv;
            this.map = map;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }


        // GET api/values
        [HttpGet, Authorize(Roles = "Administrator")]
        public new ActionResult<IEnumerable<WorkOrderVM>> Get(
            [FromQuery]int displayLength = 10,
            [FromQuery]int displayStart = 0)
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = UserSubject;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, WorkOrderVM>(e)
                ).AsEnumerable();            
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkOrderVM> Get(int id)
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
        
            var result = map.Map<Domain.WorkOrder, WorkOrderVM>(serv.Get(id));
            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Post([FromBody]WorkOrderVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Put(int id, [FromBody]WorkOrderVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Delete(int id) { return base.Delete(id); }
    }
}
