using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;

namespace Machete.Api.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class WorkOrdersController : ApiController
    {
        private readonly IWorkOrderService serv;
        private readonly IWorkAssignmentService waServ;
        private readonly IMapper map;
        private readonly IDefaults def;

        public WorkOrdersController(IWorkOrderService workOrderService, IWorkAssignmentService workAssignmentService,
            IDefaults def,
            IMapper map)
        {
            this.serv = workOrderService;
            this.waServ = workAssignmentService;
            this.map = map;
            this.def = def;
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
                    e => map.Map<DTO.WorkOrdersList, Web.ViewModel.Api.WorkOrder>(e)
                ).AsEnumerable();            
            return Json(new { data =  result });
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkOrder, Web.ViewModel.Api.WorkOrder>(serv.Get(id));
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