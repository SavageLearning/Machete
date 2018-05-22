using AutoMapper;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Machete.Api.ViewModel;
using System.Security.Claims;
using Machete.Domain;

namespace Machete.Api.Controllers
{
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = userSubject;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, ViewModel.WorkOrder>(e)
                ).AsEnumerable();            
            return Json(new { data =  result });
        }

        // GET api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(serv.Get(id));
            return Json(new { data = result });
        }

        // POST api/values
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Post([FromBody]ViewModel.WorkOrder order)
        {
            var domain = map.Map<ViewModel.WorkOrder, Domain.WorkOrder>(order);
            serv.Save(domain, userEmail);
        }

        // PUT api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Put(int id, [FromBody]ViewModel.WorkOrder order)
        {
            var domain = serv.Get(order.id);
            // TODO employers must only be able to edit their record
            map.Map<ViewModel.WorkOrder, Domain.WorkOrder>(order, domain);
            serv.Save(domain,userEmail);
        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Delete(int id)
        {
        }
    }
}