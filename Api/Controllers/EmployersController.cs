using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [AllowAnonymous]
    public class EmployersController : MacheteApiController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;
        private readonly IMapper map;

        public EmployersController(
            IEmployerService employerService, 
            IWorkOrderService workorderService,
            IMapper map)
        {
            this.serv = employerService;
            this.woServ = workorderService;
            this.map = map;
        }
        // GET api/values
        // TODO Add real permissions
        //[ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<DTO.EmployersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.EmployersList, Employer>(e)
                ).AsEnumerable();
            return Json(new { data =  result });
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(id));
            return Json(new { data = result });
        }

        // TODO: If employer/hirer, only get my own employer record
        public IHttpActionResult Get(string sub)
        {

            var result = map.Map<Domain.Employer, Employer>(serv.Get(sub));
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