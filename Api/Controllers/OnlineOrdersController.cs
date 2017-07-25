using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class OnlineOrdersController : ApiController
    {
        private readonly IOnlineOrdersService serv;
        private readonly IMapper map;
        public OnlineOrdersController(IOnlineOrdersService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }
        // GET: api/OnlineOrders
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]

        public IHttpActionResult Get()
        {
            var principal = RequestContext.Principal as ClaimsPrincipal;
            var user = principal.FindFirst(CAType.nameidentifier).Value;
            var result = serv.GetMany(a => a.Employer.onlineSigninID == user)
                .Select(a => map.Map<Domain.WorkOrder, WorkOrder>(a));

            return Json(new { data = result });
        }

        // GET: api/OnlineOrders/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OnlineOrders
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Post([FromBody]string value)
        {
        }

    }
}
