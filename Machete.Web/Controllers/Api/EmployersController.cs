using System;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;
using EmployerViewModel = Machete.Web.ViewModel.Api.Employer;

namespace Machete.Web.Controllers.Api
{
    [Route("api/employer")]
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
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpGet]
        [Route("")]
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
        [Authorize(Roles = "Administrator, Manager, Phonedesk")]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.Employer, EmployerViewModel>(serv.Get(id));
            if (result == null) return NotFound();

            return new JsonResult(new { data = result });
        }

        [Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        [HttpGet]
        [Route("profile")]
        public ActionResult ProfileGet()
        {
            Domain.Employer e;
            try
            {
                e = findEmployerBySubjectOrEmail();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            if (e == null) return NotFound();
            // Ensure link between email, onlineSigninID, and employer account are correct
            if (e.email != UserEmail) e.email = UserEmail;
            if (e.onlineSigninID == null)
            {
                e.onlineSigninID = UserSubject;
                // aggressive linking of asp.net identity and employer record
                serv.Save(e, UserEmail);
            }
            if (e.onlineSigninID != UserSubject) return Conflict();
            var result = map.Map<Domain.Employer, EmployerViewModel>(e);
            return new JsonResult(new { data = result });
        }

        [NonAction]
        public Domain.Employer findEmployerBySubjectOrEmail()
        {
            var e = serv.Get(UserSubject);
            if (e != null) return e;
            if (UserEmail != null)
            {
                // If SingleOrDefault, then ~500 users will fail to login.
                // Need solution to de-duplicating employers before getting
                // string on emails duplication
                e = serv.GetMany(em => em.email == UserEmail).OrderByDescending(em => em.dateupdated)
                    .FirstOrDefault();
                return e;
            }

            return e;

            // legacy accounts wont have an email; comes from a claim
            // if we haven't found by userSubject, and userEmail is null, assume it's a 
            // legacy account. Legacy accounts have self-attested emails for userNames
        }

        // POST api/values
        // This action method is for ANY employer
        [Authorize(Roles = "Administrator, Manager, Phonedesk")]
        [HttpPost("")]
        public void Post([FromBody]EmployerViewModel employer)
        {
            var domain = map.Map<EmployerViewModel, Domain.Employer>(employer);
            serv.Create(domain, UserEmail);
        }

        // For an employer creating his/her own record
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPost("profile")]
        public ActionResult ProfilePost([FromBody]EmployerViewModel employer)
        {
            Domain.Employer e = null;
            e = findEmployerBySubjectOrEmail();
            // If 
            if (e != null) return Conflict();

            var domain = map.Map<EmployerViewModel, Domain.Employer>(employer);
            domain.onlineSigninID = UserSubject;
            if (UserEmail != null)
                domain.email = UserEmail;
            try
            {
                serv.Create(domain, UserEmail);

            }
            catch
            {
                return StatusCode(500);
            }
            return Ok();
        }

        // For editing any employer record
        [Authorize(Roles = "Administrator, Manager, Phonedesk")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]EmployerViewModel employer)
        {
            var domain = serv.Get(employer.id);
            map.Map<EmployerViewModel, Domain.Employer>(employer, domain);
            serv.Save(domain, UserEmail);
        }

        // for an employer editing his/her own employer record
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPut("profile")]
        public ActionResult ProfilePut([FromBody]EmployerViewModel viewmodel)
        {
            bool newEmployer = false;
            var employer = findEmployerBySubjectOrEmail();
            if (employer == null)
            {
                employer = new Domain.Employer();
                newEmployer = true;
            }
            employer.onlineSigninID = UserSubject;
            employer.email = UserEmail;
            map.Map<EmployerViewModel, Domain.Employer>(viewmodel, employer);

            Domain.Employer result;
            if (newEmployer)
            {
                result = serv.Create(employer, UserEmail);
            }
            else
            {
                serv.Save(employer, UserEmail);
                result = serv.Get(employer.ID);
            }
            var mapped = map.Map<Domain.Employer, EmployerViewModel>(result);
            return new JsonResult(new { data = mapped });

        }

        // DELETE api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            // TODO: Make a soft delete; never really delete record
            return StatusCode(501);
        }
    }
}