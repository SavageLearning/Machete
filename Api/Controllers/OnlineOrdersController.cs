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
using System.Web.Http.Controllers;
using PayPal.Api;
using Machete.Domain;
using System.Globalization;

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
        private string paypalId;
        private string paypalSecret;
        private string paypalUrl;
        private Domain.Employer employer;

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
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            // base executes first to populate userSubject
            base.Initialize(controllerContext);

            employer = eServ.Get(guid: userSubject);
            if (employer == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) {
                    Content = new StringContent(string.Format("No employer record associated with claim {0}", userSubject)),
                    ReasonPhrase = "Not found: employer record"});
            }
            paypalId = cServ.getConfig(Cfg.PaypalId);
            paypalSecret = cServ.getConfig(Cfg.PaypalSecret);
            paypalUrl = cServ.getConfig(Cfg.PaypalUrl);
        }

        // GET: api/OnlineOrders
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 500; // TODO dumping on the client; will address Angular using search later
            vo.displayStart = 0;
            vo.employerGuid = userSubject;
            vo.CI = Thread.CurrentThread.CurrentCulture;
            dataTableResult<Service.DTO.WorkOrdersList> list = woServ.GetIndexView(vo);
            var result = list.query
                .Select(
                    e => map.Map<Service.DTO.WorkOrdersList, ViewModel.WorkOrder>(e)
                ).AsEnumerable();
            return Json(new { data = result });
        }

        // GET: api/OnlineOrders/5
        [Route("api/onlineorders/{orderID}")]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult Get(int orderID)
        {
            Domain.WorkOrder order = null;
            try
            {
                order = serv.Get(orderID);
            }
            catch
            {
                throwInvalidOrder(orderID);
            }
            if (order.EmployerID != employer.ID)
            {
                throwInvalidOrder(orderID);
            }

            // TODO: Not mapping to view object throws JsonSerializationException, good to test error
            // handling with...(delay in error)
            var result = map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(order);
            return Json(new { data = result });
        }

        // POST: api/OnlineOrders
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult Post([FromBody]ViewModel.WorkOrder order)
        {

            var domain = map.Map<ViewModel.WorkOrder, Domain.WorkOrder>(order);
            domain.Employer = employer;
            domain.EmployerID = employer.ID;
            domain.onlineSource = true;
            Domain.WorkOrder newOrder = null;
            try {
                newOrder = serv.Create(domain, employer.email ?? employer.name);
            }
            catch(MacheteValidationException e)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(e.ErrorMessage),
                    ReasonPhrase = "Validation failed on workorder"
                };
                throw new HttpResponseException(res);
            } catch(InvalidOperationException e)
            {
                var res = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message),
                    ReasonPhrase = "Invalid operation "
                };
                throw new HttpResponseException(res);
            }
            var result = map.Map<Domain.WorkOrder, ViewModel.WorkOrder>(newOrder);
            return Json(new { data = result });
        }

        // POST: api/onlineorders/{orderid}/paypalexecute
        [HttpPost]
        [Route("api/onlineorders/{orderID}/paypalexecute")]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult PaypalExecute(int orderID, [FromBody]PaypalPayment data)
        {
            validatePaypalData(data);
            Domain.WorkOrder order = null;

            order = serv.Get(orderID);        
            if (order.EmployerID != employer.ID)
            {
                throwInvalidOrder(orderID);
            }

            validateNoPreviousPayment(order, data);

            if (order.ppState == null)
            {
                order.ppPayerID = data.payerID;
                order.ppPaymentID = data.paymentID;
                order.ppPaymentToken = data.paymentToken;
                order.ppState = "created";
                woServ.Save(order, userEmail);
            }

            var result = postExecute(data);
            var payment = JsonConvert.DeserializeObject<PayPal.Api.Payment>(result);
            order.ppResponse = result;
            order.ppState = payment.state;
            order.ppFee = Double.Parse(payment.transactions.Single().amount.total);
            woServ.Save(order, userEmail);
            return Json(payment);
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
        public void throwInvalidOrder(int id)
        {
            var res = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("Order {0} not found for employer {1}", id, employer.ID)),
                ReasonPhrase = "Workorder not found"
            };
            throw new HttpResponseException(res);
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
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Payment execute failed"
                });
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
