using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using System.Linq;
using System.Web.Http;
using DTO = Machete.Service.DTO;

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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(id));
            return Json(new { data = result });
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk})]
        public IHttpActionResult Get(string sub)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(sub));
            return Json(new { data = result });
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk, CV.Hirer })]
        [HttpGet]
        [Route("api/employer/profile")]
        public IHttpActionResult ProfileGet()
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(userSubject));
            return Json(new { data = result });
        }

        // POST api/values
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Post([FromBody]Employer employer)
        {
            var domain = map.Map<Employer, Domain.Employer>(employer);
            serv.Save(domain, User.Identity.Name);
        }


        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        [HttpPost]
        [Route("api/employer/profile")]
        public void ProfilePost([FromBody]Employer employer)
        {
            var domain = map.Map<Employer, Domain.Employer>(employer);
            domain.onlineSigninID = userSubject;
            serv.Save(domain, User.Identity.Name);
        }

        // PUT api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Put(int id, [FromBody]Employer employer)
        {
            var domain = serv.Get(employer.id);
            map.Map<Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, User.Identity.Name);
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        [HttpPatch]
        [Route("api/employer/profile")]
        public void ProfilePut(int id, [FromBody]Employer employer)
        {
            var domain = serv.Get(userSubject);
            map.Map<Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, User.Identity.Name);
        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Delete(int id)
        {
        }
    }
}