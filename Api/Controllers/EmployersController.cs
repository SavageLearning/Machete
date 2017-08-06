using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using DTO = Machete.Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Machete.Api.Controllers
{
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(id));
            return Json(new { data = result });
        }

        // TODO: If employer/hirer, only get my own employer record
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get(string sub)
        {

            var result = map.Map<Domain.Employer, Employer>(serv.Get(sub));
            return Json(new { data = result });
        }

        // POST api/values
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Post([FromBody]Employer employer)
        {
            var domain = map.Map<Employer, Domain.Employer>(employer);
            serv.Save(domain, User.Identity.Name);
        }

        // PUT api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Put(int id, [FromBody]Employer employer)
        {
            var domain = serv.Get(employer.id);
            // TODO employers must only be able to edit their record
            map.Map<Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, User.Identity.Name);
        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Delete(int id)
        {
        }
    }
}