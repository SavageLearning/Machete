using AutoMapper;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Machete.Api.ViewModel;
using System.Security.Claims;

namespace Machete.Api.Controllers
{
    public class WorkOrdersController : MacheteApiController
    {
        private readonly IWorkOrderService serv;
        private readonly IWorkAssignmentService waServ;
        private readonly IMapper map;
        private string userGuid;

        public WorkOrdersController(IWorkOrderService workOrderService, IWorkAssignmentService workAssignmentService,
            IMapper map)
        {
            this.serv = workOrderService;
            this.waServ = workAssignmentService;
            this.map = map;
        }


        // GET api/values
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = userSubject;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, WorkOrder>(e)
                ).AsEnumerable();            
            return Json(new { data =  result });
        }

        // GET api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, WorkOrder>(serv.Get(id));
            return Json(new { data = result });
        }

        // POST api/values
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Post([FromBody]WorkOrder order)
        {
            var domain = map.Map<WorkOrder, Domain.WorkOrder>(order);
            serv.Save(domain, User.Identity.Name);
        }

        // PUT api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Put(int id, [FromBody]WorkOrder order)
        {
            var domain = serv.Get(order.id);
            // TODO employers must only be able to edit their record
            map.Map<WorkOrder, Domain.WorkOrder>(order, domain);
            serv.Save(domain, User.Identity.Name);
        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Delete(int id)
        {
        }
    }
}