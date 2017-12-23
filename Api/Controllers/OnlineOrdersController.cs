using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly IConfigService cServ;
        private readonly string paypalId;
        private readonly string paypalSecret;
        private readonly string paypalUrl;

        public OnlineOrdersController(
            IOnlineOrdersService serv, 
            IEmployerService eServ,
            IWorkOrderService woServ,
            ITransportRuleService trServ,
            IMapper map,
            IConfigService cServ)
        {
            this.serv = serv;
            this.eServ = eServ;
            this.woServ = woServ;
            this.trServ = trServ;
            this.map = map;
            this.cServ = cServ;
            this.paypalId = cServ.getConfig(Cfg.PaypalId);
            this.paypalSecret = cServ.getConfig(Cfg.PaypalSecret);
            this.paypalUrl = cServ.getConfig(Cfg.PaypalUrl);
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
            var newOrder = serv.Create(domain, employer.email ?? employer.name);
            var result = map.Map<Domain.WorkOrder, WorkOrder>(newOrder);
            return Json(new { data = result });
        }

        // POST: api/onlineorders/{orderid}/paypalexecute
        [HttpPost]
        [Route("api/onlineorders/{orderID}/paypalexecute")]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        public IHttpActionResult PaypalExecute(int orderID, [FromBody]PaypalPayment data)
        {
            var token = getPaypalAuthToken(paypalUrl, paypalId, paypalSecret);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://api.sandbox.paypal.com/v1/payments/payment/"+data.paymentID+"/execute");
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };

            request.AddHeader("Authorization", string.Format("bearer {0}", token.access_token));
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", "{ \"payer_id\": \""+ data.payerID +"\"}", ParameterType.RequestBody);

            var result = client.Execute(request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw result.ErrorException;
            }

            return Ok(result.Content);
        }


        public PayPalTokenModel getPaypalAuthToken(string url, string clientId, string clientSecret)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            client.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);

            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);
            var payPalTokenModel = JsonConvert.DeserializeObject<PayPalTokenModel>(response.Content);
            return payPalTokenModel;
        }
    }

    public class PaypalPayment
    {
        public string payerID;
        public string paymentID;

    }

    public class PayPalTokenModel
    {
        public string scope { get; set; }
        public string nonce { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
    }
}
