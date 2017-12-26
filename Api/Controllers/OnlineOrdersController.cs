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
using System.Threading;
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]

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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OnlineOrders
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult PaypalExecute(int orderID, [FromBody]PaypalPayment data)
        {
            validatePaypalData(data);

            var employer = eServ.Get(userSubject);
            validateEmployer(employer);

            var order = serv.GetMany(o => o.ID == orderID && o.EmployerID == employer.ID).First();
            validateOrder(order, employer);

            validateNoPreviousPayment(order, data);

            if (order.ppState == null)
            {
                order.ppPayerID = data.payerID;
                order.ppPaymentID = data.paymentID;
                order.ppPaymentToken = data.paymentToken;
                order.ppState = "created";
                woServ.Save(order, User.Identity.Name);
            }

            var result = postExecute(data);
            return Json(result);
        }

        [NonAction]
        public void validateNoPreviousPayment(Domain.WorkOrder wo, PaypalPayment pp)
        {
            if (wo.ppPayerID != null && wo.ppPayerID != pp.payerID)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaypalID already set to {0}, conflicts with {1}", pp.payerID, wo.ppPayerID)),
                    ReasonPhrase = "PaypalID already set to a different ID"
                };
                throw new HttpResponseException(res);
            }
            if (wo.ppPaymentID != null && wo.ppPaymentID != pp.paymentID)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaymentID already set to {0}, conflicts with {1}", pp.paymentID, wo.ppPaymentID)),
                    ReasonPhrase = "PaymentID already set to a different ID"
                };
                throw new HttpResponseException(res);
            }

            if (wo.ppPaymentToken != null && wo.ppPaymentToken != pp.paymentToken)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaymentToken already set to {0}, conflicts with {1}", pp.paymentToken, wo.ppPaymentToken)),
                    ReasonPhrase = "PaymentToken already set to a different ID"
                };
                throw new HttpResponseException(res);
            }
        }

        [NonAction]
        public void validateOrder(Domain.WorkOrder wo, Domain.Employer e)
        {
            if (wo == null)
            {
                var res = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Order {0} not found for employer {1}", wo.ID, e.ID)),
                    ReasonPhrase = "Workorder not found"
                };
                throw new HttpResponseException(res);
            }
        }

        [NonAction]
        public void validatePaypalData(PaypalPayment pp)
        {
            if (pp.payerID == null || pp.paymentID == null || pp.paymentToken == null)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Paypal data: {0}", JsonConvert.SerializeObject(pp))),
                    ReasonPhrase = "Incomplete Paypal data"
                };
                throw new HttpResponseException(res);
            }
        }

        [NonAction]
        public void validateEmployer(Domain.Employer e)
        {
            if (e == null)
            {
                var res = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No employer for IdentitySubject: {0}", userSubject)),
                    ReasonPhrase = "Employer not found for user's IdentitySubject"
                };
                throw new HttpResponseException(res);
            }
        }

        [NonAction]
        public string postExecute(PaypalPayment data)
        {
            // paypal info loaded from database, set at controller creation
            var auth = getPaypalAuthToken(paypalUrl, paypalId, paypalSecret);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new RestClient(paypalUrl + "/payments/payment/" + data.paymentID + "/execute");
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };

            request.AddHeader("Authorization", string.Format("bearer {0}", auth.access_token));
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", "{ \"payer_id\": \"" + data.payerID + "\"}", ParameterType.RequestBody);

            var result = client.Execute(request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw result.ErrorException;
            }

            return result.Content;
        }

        [NonAction]
        public PayPalTokenModel getPaypalAuthToken(string url, string clientId, string clientSecret)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(url + "/oauth2/token");
            client.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);

            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("grant_type", "client_credentials");

            IRestResponse response;
            for (int i = 0; i<5; i++)
            {
                response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var payPalTokenModel = JsonConvert.DeserializeObject<PayPalTokenModel>(response.Content);
                    return payPalTokenModel;
                }
                Thread.Sleep(1000);
            }
            var res = new HttpResponseMessage(HttpStatusCode.GatewayTimeout)
            {
                ReasonPhrase = "Failed to retrieve access token from Paypal"
            };
            throw new HttpResponseException(res);
        }
    }

    public class PaypalPayment
    {
        public string payerID;
        public string paymentID;
        public string paymentToken;

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
