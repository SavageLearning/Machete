using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    // Everything in this class needs to be protected (not public). The various decorator attributes [Http, Auth, EF]
    // can't be inherited, or you'll have a bad time. Base class methods need to be protected, or they'll be
    // visible to EF and Swagger reflection, and you'll have a bad time. 
    // 
    // Controllers that extend this class will need to `new` an override method that calls the base method for each
    // method that is mean to be re-used.
    public abstract class MacheteApi2Controller<TD, TVM> : ControllerBase where TD : Record
    {
        protected string UserSubject => User?.FindFirst(CAType.nameidentifier)?.Value;
        protected string UserEmail => User?.FindFirst(CAType.email)?.Value;
        protected IService<TD> service;
        protected IMapper map;
        protected void Initialize(ControllerContext controllerContext)
        {
            ControllerContext = controllerContext;
        }

        protected MacheteApi2Controller(IService<TD> service, IMapper map) 
        {
            this.service = service;
            this.map = map;    
        }
        // GET: api/TransportRule
        protected virtual ActionResult<IEnumerable<TVM>> Get(
            int displayLength = 10,
            int displayStart = 0
        ) {
            try
            {
                var result = service.GetAll()
                    .Skip(displayStart)
                    .Take(displayLength)
                    .Select(e => map.Map<TD, TVM>(e))
                    .AsEnumerable();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // GET: api/TransportRule/5
        protected virtual ActionResult<TVM> Get(int id)
        {
            TVM result;
            try {
              result = map.Map<TD, TVM>(service.Get(id));
            } 
            catch(Exception ex) {
                return StatusCode(500, ex);
            }
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/TransportRule
        protected virtual  ActionResult<TVM> Post(TVM value)
        {
            TVM newValue;
            try {
                newValue = Create(value);
            }
            catch (Exception ex) {
                return StatusCode(500, ex);
            }
            return Ok(newValue);
        }

        [NonAction]
        private TVM Create(TVM value) 
        {
            TD domain = map.Map<TVM, TD>(value);
            return map.Map<TD, TVM>(service.Create(domain, UserEmail)); 
        }

        [NonAction]
        private TVM Update(int id, TVM value)
        {
            TD domain = service.Get(id);
            TD fromTVM = map.Map<TVM, TD>(value, domain);
            fromTVM.ID = id;
            service.Save(fromTVM, UserEmail);
            return map.Map<TD, TVM>(service.Get(id));
        }
        // PUT: api/TransportRule/5
        protected virtual  ActionResult<TVM> Put(int id, TVM value)
        {
            TVM newValue;

            try {
                if (id == 0) {
                    newValue = Create(value);
                } else {
                    newValue = Update(id, value);
                }
            } 
            catch(Exception ex) {
                return StatusCode(500, ex);
            }
            return Ok(newValue);
        }

        // DELETE: api/TransportRule/5
        protected virtual  ActionResult Delete(int id)
        {
            try {
                service.Delete(id, UserEmail);
            }
            catch (Exception ex) {
                return StatusCode(500, ex);
            }
            return Ok();
        }
        
    }
}
