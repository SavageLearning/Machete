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
    public class OnlineOrdersController : MacheteApiController
    {
        private readonly IOnlineOrdersService serv;
        private readonly IEmployerService eServ;
        private readonly IWorkOrderService woServ;
        private readonly ITransportRuleService trServ;
        private readonly IMapper map;

        public OnlineOrdersController(
            IOnlineOrdersService serv, 
            IEmployerService eServ,
            IWorkOrderService woServ,
            ITransportRuleService trServ,
            IMapper map)
        {
            this.serv = serv;
            this.eServ = eServ;
            this.woServ = woServ;
            this.trServ = trServ;
            this.map = map;
        }
        // GET: api/OnlineOrders
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]

        public IHttpActionResult Get()
        {

            var vo = new viewOptions();
            vo.displayLength = 10;
            vo.displayStart = 0;
            vo.employerGuid = userSubject;
            dataTableResult<Service.DTO.WorkOrdersList> list = woServ.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<Service.DTO.WorkOrdersList, WorkOrder>(e)
                ).AsEnumerable();
            return Json(new { data = result });
        }

        // GET: api/OnlineOrders/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OnlineOrders
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        public IHttpActionResult Post([FromBody]WorkOrder order)
        {
            var employer = eServ.Get(guid: userSubject);
            if (employer == null)
            {
                throw new Exception("no employer record associated with subject claim");
            }
            var domain = map.Map<WorkOrder, Domain.WorkOrder>(order);
            domain.EmployerID = employer.ID;
            var result = serv.Create(domain, employer.email ?? employer.name);
            result.Employer = null;
            // TODO this is a hacky workaround until I have viewmodels
            result.workAssignments.Select(a => { a.workOrder = null; return a; } ).ToList();
            return Json(new { data = result });
        }

    }
}
