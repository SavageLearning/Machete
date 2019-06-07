using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
using WorkOrder = Machete.Web.ViewModel.Api.WorkOrder;

namespace Machete.Web.Controllers.Api
{
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrdersController : MacheteApiController
    {
        private readonly IWorkOrderService serv;
        private readonly IWorkAssignmentService waServ;
        private readonly IMapper map;

        public WorkOrdersController(IWorkOrderService workOrderService, IWorkAssignmentService workAssignmentService,
            IMapper map)
        {
            this.serv = workOrderService;
            this.waServ = workAssignmentService;
            this.map = map;
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
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, Machete.Web.ViewModel.Api.WorkOrder>(e)
                ).AsEnumerable();            
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, WorkOrder>(serv.Get(id));
            return new JsonResult(new { data = result });
        }

        // POST api/values
        [Authorize(Roles = "Administrator")]
        [HttpPost("")]
        public void Post([FromBody]WorkOrder order)
        {
            var domain = map.Map<WorkOrder, Domain.WorkOrder>(order);
            serv.Save(domain, UserEmail);
        }

        // PUT api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]WorkOrder order)
        {
            var domain = serv.Get(order.id);
            // TODO employers must only be able to edit their record
            map.Map<WorkOrder, Domain.WorkOrder>(order, domain);
            serv.Save(domain,UserEmail);
        }

        // DELETE api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}