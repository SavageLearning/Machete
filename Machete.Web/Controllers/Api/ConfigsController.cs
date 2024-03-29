﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Helpers.Api;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Machete.Web.Controllers.Api
{
    [Route("api/configs")]
    [ApiController]
    public class ConfigsController : MacheteApi2Controller<Config, ConfigVM>
    {
        private IConfiguration _configuration;

        public ConfigsController(IConfigService serv, IConfiguration configuration, IMapper map) : base(serv, map)
        {
            _configuration = configuration;
        }

        // GET: api/Configs
        //[Authorize(Roles = "Administrator")]
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<ConfigVM>> Get()
        {
            List<ConfigVM> result = service.GetMany(c => c.publicConfig)
                .DefaultIfEmpty()
                .Select(a => map.Map<Config, ConfigVM>(a))
                .DefaultIfEmpty()
                .ToList();
            
            var facebookConfig = new ConfigVM
            {
                key = "FacebookAppId",
                value = _configuration["Authentication:Facebook:AppId"]
            };
            var googleConfig = new ConfigVM
            {
                key = "GoogleClientId",
                value = _configuration["Authentication:Google:ClientId"]
            };
            var state = new ConfigVM
            {
                key = "OAuthStateParameter",
                //TODO value = this.httpContext.setCookie(foo, bar) => urlencode(bar)
                value = _configuration["Authentication:State"]
            };
            
            result.Add(facebookConfig);
            result.Add(googleConfig);
            result.Add(state);
            return Ok( new {data = result});
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<ConfigVM> Get([FromRoute] int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<ConfigVM> Post([FromBody] ConfigVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<ConfigVM> Put([FromRoute] int id, [FromBody] ConfigVM value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Config dbRecord = service.Get(id);
            ConfigVM updatedRecordAsVM;

            if (dbRecord is null) return NotFound();

            if (!dbRecord.IsUserDefined()) return BadRequest("Record not editable by user");
            try
            {
                // key shouldn't change
                var ogKey = dbRecord.key;
                Config mappedAsDomain = map.Map(value, dbRecord);
                mappedAsDomain.key = ogKey;
                mappedAsDomain.ID = id;
                service.Save(mappedAsDomain, UserEmail);
                updatedRecordAsVM = map.Map<Config, ConfigVM>(service.Get(id));   
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new {data = updatedRecordAsVM});
        }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete([FromRoute] int id) { return base.Delete(id); }

    }
}
