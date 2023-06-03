using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.Helpers;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Machete.Service.DTO;

namespace Machete.Api.Controllers
{
    // Everything in this class needs to be protected (not public). The various decorator attributes [Http, Auth, EF]
    // can't be inherited, or you'll have a bad time. Base class methods need to be protected, or they'll be
    // visible to EF and Swagger reflection, and you'll have a bad time.
    //
    // Controllers that extend this class will need to `new` an override method that calls the base method for each
    // method that is mean to be re-used.
    public abstract class MacheteApiController<TD, TVM, TLVM> : ControllerBase
        where TD : Record
        where TVM : RecordVM
        where TLVM : ListVM
    {
        protected string UserSubject => User?.FindFirst(CAType.nameidentifier)?.Value;
        protected string UserEmail => User?.FindFirst(CAType.email)?.Value;
        protected IService<TD> service;
        protected IMapper map;
        protected void Initialize(ControllerContext controllerContext)
        {
            ControllerContext = controllerContext;
        }

        protected MacheteApiController(IService<TD> service, IMapper map)
        {
            this.service = service;
            this.map = map;
        }
        protected virtual ActionResult<IEnumerable<TLVM>> Get(
            ApiRequestParams requestParams
        )
        {
            try
            {
                // Some controllers return all records in DB, e.g. Schedule Rules
                if (requestParams.AllRecords)
                    return Ok(new
                    {
                        data = service.GetAll()
                        .Select(e => map.Map<TD, TLVM>(e))
                    });
                var result = service.GetAll()
                    .Skip(requestParams.Skip)
                    .Take(requestParams.pageSize)
                    .Select(e => map.Map<TD, TLVM>(e))
                    .AsEnumerable();
                //!!TODO Implement pagination and filtering
                var paginationMetaData = new PaginationMetaData();
                return Ok(new { metaData = paginationMetaData, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        protected virtual ActionResult<TVM> Get(int id)
        {
            TVM result;
            try
            {
                result = map.Map<TD, TVM>(service.Get(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            if (result == null) return NotFound();
            return Ok(new { data = result });
        }

        protected virtual ActionResult<TVM> Post(TVM value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            TVM newValue;
            try
            {
                newValue = Create(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return CreatedAtAction(nameof(Get), new { newValue.id }, new { data = newValue });
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
        protected virtual ActionResult<TVM> Put(int id, TVM value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            TVM newValue;

            try
            {
                if (id == 0)
                {
                    newValue = Create(value);
                }
                else
                {
                    if (service.Get(id) == null) return NotFound();
                    newValue = Update(id, value);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new { data = newValue });
        }

        // DELETE: api/TransportRule/5
        protected virtual ActionResult Delete(int id)
        {
            if (service.Get(id) == null) return NotFound();
            try
            {
                service.Delete(id, UserEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok();
        }

    }
}
