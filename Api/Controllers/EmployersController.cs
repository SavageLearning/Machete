using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(id));
            if (result == null) return NotFound();

            return Json(new { data = result });
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk, CV.Employer })]
        [HttpGet, Route("api/employer/profile")]
        public IHttpActionResult ProfileGet()
        {
            Domain.Employer e;
            try
            {
                e = getEmployer();
            }
            catch
            {
                return InternalServerError();
            }

            if (e == null) return NotFound();
            // Ensure link between email, onlineSigninID, and employer account are correct
            if (e.email != userEmail) e.email = userEmail;
            if (e.onlineSigninID != userSubject) e.onlineSigninID = userSubject;

            var result = map.Map<Domain.Employer, Employer>(e);
            return Json(new { data = result });
        }

        [NonAction]
        public Domain.Employer getEmployer()
        {
            Domain.Employer e = null;
            e = serv.Get(userSubject);
            if (e == null)
            {
                e = serv.GetMany(em => em.email == userEmail).SingleOrDefault();
            }
            return e;
        }


        // POST api/values
        // This action method is for ANY employer
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public void Post([FromBody]Employer employer)
        {
            var domain = map.Map<Employer, Domain.Employer>(employer);
            serv.Create(domain, User.Identity.Name);
        }

        // For an employer creating his/her own record
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public IHttpActionResult ProfilePost([FromBody]Employer employer)
        {
            Domain.Employer e = null;
            e = getEmployer();
            // If 
            if (e != null) return Conflict();

            var domain = map.Map<Employer, Domain.Employer>(employer);
            domain.onlineSigninID = userSubject;
            domain.email = userEmail;
            try
            {
                serv.Create(domain, User.Identity.Name);

            }
            catch
            {
                return InternalServerError();
            }
            return Ok();
        }

        // For editing any employer record
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public void Put(int id, [FromBody]Employer employer)
        {
            var domain = serv.Get(employer.id);
            map.Map<Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, User.Identity.Name);
        }

        // for an employer editing his/her own employer record
        [HttpPut]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public IHttpActionResult ProfilePut([FromBody]Employer employer)
        {
            bool newEmployer = false;
            Domain.Employer e = null;
            e = getEmployer();
            if (e == null)
            {
                e = new Domain.Employer();
                newEmployer = true;
            }
            e.onlineSigninID = userSubject;
            e.email = userEmail;
            map.Map<Employer, Domain.Employer>(employer, e);

            Domain.Employer result;
            if (newEmployer)
            {
                result = serv.Create(e, User.Identity.Name);
            }
            else
            {
                serv.Save(e, User.Identity.Name);
                result = serv.Get(e.ID);
            }
            var mapped = map.Map<Domain.Employer, Employer>(result);
            return Json(new { data = mapped });

        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Delete(int id)
        {
            // TODO: Make a soft delete; never really delete record
        }
    }
}