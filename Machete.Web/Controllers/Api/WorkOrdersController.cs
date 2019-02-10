using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
using WorkOrder = Machete.Web.ViewModel.Api.WorkOrder;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public ActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = userSubject;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, Machete.Web.ViewModel.Api.WorkOrder>(e)
                ).AsEnumerable();            
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, WorkOrder>(serv.Get(id));
            return new JsonResult(new { data = result });
        }

        // POST api/values
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public void Post([FromBody]WorkOrder order)
        {
            var domain = map.Map<WorkOrder, Domain.WorkOrder>(order);
            serv.Save(domain, userEmail);
        }

        // PUT api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public void Put(int id, [FromBody]WorkOrder order)
        {
            var domain = serv.Get(order.id);
            // TODO employers must only be able to edit their record
            map.Map<WorkOrder, Domain.WorkOrder>(order, domain);
            serv.Save(domain,userEmail);
        }

        // DELETE api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public void Delete(int id)
        {
        }
    }
}