using System.Linq;
using AutoMapper;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ElmahHandleError]
    [Authorize]
    public class WorkAssignmentsController : ControllerBase
    {
        private readonly IWorkAssignmentService serv;
        private readonly IMapper map;

        public WorkAssignmentsController(IWorkAssignmentService employerService, IWorkOrderService workorderService,
            IMapper map)
        {
            this.serv = employerService;
            this.map = map;
        }
        // GET api/values
        public ActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<DTO.WorkAssignmentsList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.WorkAssignmentsList, Machete.Web.ViewModel.Api.WorkAssignment>(e)
                ).AsEnumerable();
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkAssignment, Machete.Web.ViewModel.Api.WorkAssignment>(serv.Get(id));
            return new JsonResult(new { data = result });
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
