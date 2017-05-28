using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class WorkAssignmentsController : ApiController
    {
        private readonly IWorkAssignmentService serv;
        private readonly IWorkOrderService woServ;
        private readonly IMapper map;
        private readonly IDefaults def;

        public WorkAssignmentsController(IWorkAssignmentService employerService, IWorkOrderService workorderService,
            IDefaults def,
            IMapper map)
        {
            this.serv = employerService;
            this.woServ = workorderService;
            this.map = map;
            this.def = def;
        }
        // GET api/values
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<DTO.WorkAssignmentsList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkAssignmentsList, Web.ViewModel.Api.WorkAssignment>(e)
                ).AsEnumerable();
            return Json(new { data =  result });
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkAssignment, Web.ViewModel.Api.WorkAssignment>(serv.Get(id));
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