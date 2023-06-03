using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Api.Helpers;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Machete.Api.Controllers
{
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrdersController : MacheteApiController<WorkOrder, WorkOrderVM, WorkOrderListVM>
    {
        private readonly IWorkOrderService serv;

        public WorkOrdersController(IWorkOrderService serv, ITenantService tenantService,
            IMapper map) : base(serv, map)
        {
            this.serv = serv;
            this.map = map;
        }


        // GET api/values
        [HttpGet, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Get(
            [FromQuery] ApiRequestParams apiRequestParams)
        {
            var vo = new viewOptions();
            vo.displayLength = apiRequestParams.pageSize;
            vo.displayStart = apiRequestParams.Skip;
            vo.employerGuid = UserSubject;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);

            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, WorkOrderListVM>(e)
                ).AsEnumerable();
            return Ok(new { data = result });
        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkOrderVM> Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, WorkOrderVM>(serv.Get(id));
            return Ok(new { data = result });
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Post([FromBody] WorkOrderVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkOrderVM> Put(int id, [FromBody] WorkOrderVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }
    }
}
