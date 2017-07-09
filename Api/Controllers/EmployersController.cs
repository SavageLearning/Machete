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
    public class EmployersController : ApiController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;
        private readonly IMapper map;
        private readonly IDefaults def;

        public EmployersController(IEmployerService employerService, IWorkOrderService workorderService,
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
            dataTableResult<DTO.EmployersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.EmployersList, Web.ViewModel.Api.Employer>(e)
                ).AsEnumerable();
            return Json(new { data =  result });
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var m = map.Map<Domain.Employer, Web.ViewModel.Api.Employer>(serv.Get(id));
            return Ok(m);
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