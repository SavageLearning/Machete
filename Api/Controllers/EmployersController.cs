using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Domain;
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
                    e => map.Map<DTO.EmployersList, ViewModel.EmployersList>(e)
                ).AsEnumerable();
            return Json(new { data =  result });
        }

        // GET api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, ViewModel.Employer>(serv.Get(id));
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
                e = findEmployerBy();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (e == null) return NotFound();
            // Ensure link between email, onlineSigninID, and employer account are correct
            if (e.email != userEmail) e.email = userEmail;
            if (e.onlineSigninID == null)
            {
                e.onlineSigninID = userSubject;
                // aggressive linking of asp.net identity and employer record
                serv.Save(e, userEmail);
            }
            if (e.onlineSigninID != userSubject) return Conflict();
            var result = map.Map<Domain.Employer, ViewModel.Employer>(e);
            return Json(new { data = result });
        }

        [NonAction]
        public Domain.Employer findEmployerBy()
        {
            Domain.Employer e = null;
            e = serv.Get(userSubject);
            if (e != null) return e;
            
            // legacy accounts wont have an email; comes from a claim
            if (userEmail != null)
            {
                // If SingleOrDefault, then ~500 users will fail to login.
                // Need solution to de-duplicating employers before getting
                // string on emails duplication
                e = serv.GetMany(em => em.email == userEmail).OrderByDescending(em => em.dateupdated).FirstOrDefault();
                return e;
            }
            return e;
            // if we haven't found by userSubject, and userEmail is null, assume it's a 
            // legacy account. Legacy accounts have self-attested emails for userNames


        }


        // POST api/values
        // This action method is for ANY employer
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public void Post([FromBody]ViewModel.Employer employer)
        {
            var domain = map.Map<ViewModel.Employer, Domain.Employer>(employer);
            serv.Create(domain, userEmail);
        }

        // For an employer creating his/her own record
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public IHttpActionResult ProfilePost([FromBody]ViewModel.Employer employer)
        {
            Domain.Employer e = null;
            e = findEmployerBy();
            // If 
            if (e != null) return Conflict();

            var domain = map.Map<ViewModel.Employer, Domain.Employer>(employer);
            domain.onlineSigninID = userSubject;
            if (userEmail != null)
                domain.email = userEmail;
            try
            {
                serv.Create(domain, userEmail);

            }
            catch
            {
                return InternalServerError();
            }
            return Ok();
        }

        // For editing any employer record
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        public void Put(int id, [FromBody]ViewModel.Employer employer)
        {
            var domain = serv.Get(employer.id);
            map.Map<ViewModel.Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, userEmail);
        }

        // for an employer editing his/her own employer record
        [HttpPut]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public IHttpActionResult ProfilePut([FromBody]ViewModel.Employer employer)
        {
            bool newEmployer = false;
            Domain.Employer e = null;
            e = findEmployerBy();
            if (e == null)
            {
                e = new Domain.Employer();
                newEmployer = true;
            }
            e.onlineSigninID = userSubject;
            e.email = userEmail;
            map.Map<ViewModel.Employer, Domain.Employer>(employer, e);

            Domain.Employer result;
            if (newEmployer)
            {
                result = serv.Create(e, userEmail);
            }
            else
            {
                serv.Save(e, userEmail);
                result = serv.Get(e.ID);
            }
            var mapped = map.Map<Domain.Employer, ViewModel.Employer>(result);
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