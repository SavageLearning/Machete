using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Machete.Api.ViewModel;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : MacheteApi2Controller<Employer, EmployerVM>
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;

        public EmployersController(
            IEmployerService serv,
            IWorkOrderService workorderService,
            IMapper map) : base(serv, map)
        {
            this.serv = serv;
            this.woServ = workorderService;
        }

        // GET api/values
        // TODO Add real permissions
        [Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        [HttpGet]
        public new ActionResult<IEnumerable<EmployersList>> Get(
            [FromQuery] ApiRequestParams apiRequestParams
            )
        {
            var list = serv.GetIndexView(new viewOptions
            {
                displayLength = apiRequestParams.pageSize,
                displayStart = apiRequestParams.Skip
            });

            if (list.query == null) return NotFound();
            return Ok(new { data = list.query });
        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk")]
        public new ActionResult<EmployerVM> Get(int id) { return base.Get(id); }


        [Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        [HttpGet]
        [Route("profile")]
        public ActionResult<EmployerVM> ProfileGet()
        {
            Employer e;
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
            var result = map.Map<Employer, EmployerVM>(e);
            return Ok(new { data = result });
        }

        [NonAction]
        public Employer findEmployerBySubjectOrEmail()
        {
            var currentEmployer = serv.Get(UserSubject);
            if (currentEmployer != null) return currentEmployer;
            if (UserEmail != null)
            {
                // If SingleOrDefault, then ~500 users will fail to login.
                // Need solution to de-duplicating employers before getting
                // string on emails duplication
                currentEmployer = serv.GetMany(em => em.email == UserEmail).OrderByDescending(em => em.dateupdated)
                    .FirstOrDefault();
                return currentEmployer;
            }

            return currentEmployer;

            // legacy accounts wont have an email; comes from a claim
            // if we haven't found by userSubject, and userEmail is null, assume it's a
            // legacy account. Legacy accounts have self-attested emails for userNames
        }

        // POST api/values
        // This action method is for ANY employer
        [HttpPost, Authorize(Roles = "Administrator, Manager, Phonedesk")]
        public new ActionResult<EmployerVM> Post([FromBody] EmployerVM employer) { return base.Post(employer); }

        // For an employer creating his/her own record
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPost("profile")]
        public ActionResult<EmployerVM> ProfilePost([FromBody] EmployerVM employer)
        {
            Employer existingEmployer = null;
            EmployerVM newEmployer = null;
            existingEmployer = findEmployerBySubjectOrEmail();
            // If
            if (existingEmployer != null) return Conflict();

            var domain = map.Map<EmployerVM, Employer>(employer);
            domain.onlineSigninID = UserSubject;
            if (UserEmail != null)
                domain.email = UserEmail;
            try
            {
                newEmployer = map.Map<Employer, EmployerVM>(serv.Create(domain, UserEmail));
            }
            // catch (DbExpception e) {

            // }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new { data = newEmployer });
        }

        // For editing any employer record
        [Authorize(Roles = "Administrator, Manager, Phonedesk")]
        [HttpPut("{id}")]
        public new ActionResult<EmployerVM> Put(int id, [FromBody] EmployerVM employer) { return base.Put(id, employer); }

        // for an employer editing his/her own employer record
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPut("profile")]
        public ActionResult<EmployerVM> ProfilePut([FromBody] EmployerVM viewmodel)
        {
            bool newEmployer = false;
            var employer = findEmployerBySubjectOrEmail();
            if (employer == null)
            {
                employer = new Employer();
                newEmployer = true;
            }
            employer.onlineSigninID = UserSubject;
            employer.email = UserEmail;
            viewmodel.onlineSource = true;
            map.Map<EmployerVM, Employer>(viewmodel, employer);

            Employer result;
            try
            {
                if (newEmployer)
                {
                    result = serv.Create(employer, UserEmail);
                }
                else
                {
                    serv.Save(employer, UserEmail);
                    result = serv.Get(employer.ID);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);

            }
            var mapped = map.Map<Employer, EmployerVM>(result);
            return Ok(new { data = mapped });

        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }

    }
}
