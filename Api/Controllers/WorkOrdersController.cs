using AutoMapper;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Machete.Api.ViewModel;

namespace Machete.Api.Controllers
{
    [AllowAnonymous]
    public class WorkOrdersController : ApiController
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
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkOrdersList, WorkOrder>(e)
                ).AsEnumerable();            
            return Json(new { data =  result });
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, WorkOrder>(serv.Get(id));
            return Json(new { data = result });
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}