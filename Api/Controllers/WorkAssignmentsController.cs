using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Linq;
using Machete.Api.Attributes;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
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
                    e => map.Map<DTO.WorkAssignmentsList, Api.ViewModels.WorkAssignment>(e)
                ).AsEnumerable();
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.WorkAssignment, Api.ViewModels.WorkAssignment>(serv.Get(id));
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
