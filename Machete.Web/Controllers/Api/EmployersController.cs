using System;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
using Employer = Machete.Web.ViewModel.Api.Employer; // dear past Chaim: maybe not a great idea; dear future Chaim: TODO

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Employer })]
        [HttpGet]
        public ActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            dataTableResult<DTO.EmployersList> list = serv.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<DTO.EmployersList, EmployersList>(e)
                ).AsEnumerable();
            return new JsonResult(new { data =  result });
        }

        // GET api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, Employer>(serv.Get(id));
            if (result == null) return NotFound();

            return new JsonResult(new { data = result });
        }

        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager, CV.Phonedesk, CV.Employer })]
        [HttpGet, Route("api/employer/profile")]
        public ActionResult ProfileGet()
        {
            Domain.Employer e;
            try
            {
                e = findEmployerBy();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
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
            var result = map.Map<Domain.Employer, Employer>(e);
            return new JsonResult(new { data = result });
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        [HttpPost]
        public void Post([FromBody]Employer employer)
        {
            var domain = map.Map<Employer, Domain.Employer>(employer);
            serv.Create(domain, userEmail);
        }

        // For an employer creating his/her own record
        [HttpPost]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public ActionResult ProfilePost([FromBody]Employer employer)
        {
            Domain.Employer e = null;
            e = findEmployerBy();
            // If 
            if (e != null) return Conflict();

            var domain = map.Map<Employer, Domain.Employer>(employer);
            domain.onlineSigninID = userSubject;
            if (userEmail != null)
                domain.email = userEmail;
            try
            {
                serv.Create(domain, userEmail);

            }
            catch
            {
                return StatusCode(500);
            }
            return Ok();
        }

        // For editing any employer record
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager, CV.Phonedesk })]
        [HttpPut]
        public void Put(int id, [FromBody]Employer employer)
        {
            var domain = serv.Get(employer.id);
            map.Map<Employer, Domain.Employer>(employer, domain);
            serv.Save(domain, userEmail);
        }

        // for an employer editing his/her own employer record
        [HttpPut]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Employer })]
        [Route("api/employer/profile")]
        public ActionResult ProfilePut([FromBody]Employer employer)
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
            map.Map<Employer, Domain.Employer>(employer, e);

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
            var mapped = map.Map<Domain.Employer, Employer>(result);
            return new JsonResult(new { data = mapped });

        }

        // DELETE api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            // TODO: Make a soft delete; never really delete record
            return StatusCode(501);
        }
    }
}