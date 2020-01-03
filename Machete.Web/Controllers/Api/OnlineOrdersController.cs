using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Machete.Web.Controllers.Api
{
    [Route("api/onlineorders")]
    [ApiController]
    public class OnlineOrdersController : MacheteApi2Controller<WorkOrder, WorkOrderVM>
    {
        private readonly IOnlineOrdersService serv;
        private readonly string paypalId;
        private readonly string paypalSecret;
        private readonly string paypalUrl;
        private readonly TimeZoneInfo _clientTimeZoneInfo;

        //private Employer Employer;
        private Employer Employer => serv.GetEmployer(UserSubject) ??
                throw new MacheteNullObjectException($"Not found: employer record; no employer record associated with claim {UserSubject}");

        public OnlineOrdersController(
            IOnlineOrdersService serv, 
            ITenantService tenantService,
            IMapper map,
            IConfigService cServ) : base(serv, map)
        {
            this.serv = serv;
            this.map = map;

            paypalId = cServ.getConfig(Cfg.PaypalId);
            paypalSecret = cServ.getConfig(Cfg.PaypalSecret);
            paypalUrl = cServ.getConfig(Cfg.PaypalUrl);
            
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        // GET: api/onlineorders
        [HttpGet, Authorize(Roles = "Administrator, Hirer")]
        public ActionResult Get()
        {
            var vo = new viewOptions();
            vo.displayLength = 500; // TODO dumping on the client; will address Angular using search later
            vo.displayStart = 0;
            vo.employerGuid = UserSubject;
            vo.CI = Thread.CurrentThread.CurrentCulture;
            dataTableResult<Service.DTO.WorkOrdersList> list = serv.GetIndexView(vo);
            
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var result = list.query
                .Select(
                    e => map.Map<Service.DTO.WorkOrdersList, WorkOrderVM>(e)
                ).AsEnumerable();
            return new JsonResult(new { data = result });
        }

        // GET: api/onlineorders/5
        [HttpGet("{orderID}"), Authorize(Roles = "Administrator, Hirer")]
        public new ActionResult<WorkOrderVM> Get(int orderID)
        {
            WorkOrder order = null;
            try
            {
                order = serv.Get(orderID);
                if (order.EmployerID != Employer.ID) throwInvalidOrder(orderID);
            }
            catch
            {
                throwInvalidOrder(orderID);
            }

            // TODO: Not mapping to view object throws JsonSerializationException, good to test error
            // handling with...(delay in error)
            WorkOrderVM result = map.Map<WorkOrder, WorkOrderVM>(order);
            return Ok(result);
        }

        // POST: api/OnlineOrders
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPost("")]
        public new ActionResult<WorkOrderVM> Post([FromBody]WorkOrderVM viewmodel)
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            
            var workOrder = map.Map<WorkOrderVM, WorkOrder>(viewmodel);
            workOrder.Employer = Employer;
            workOrder.EmployerID = Employer.ID;
            workOrder.onlineSource = true;
            WorkOrder newOrder;
            try {
                newOrder = serv.Create(workOrder, Employer.email ?? Employer.name);
            }
            catch(MacheteValidationException e)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(e.ErrorMessage),
                    ReasonPhrase = "Validation failed on workOrder"
                };
                return BadRequest(res);
            } catch(InvalidOperationException e)
            {
                var res = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message),
                    ReasonPhrase = "Invalid operation "
                };
                return BadRequest(res);
            }
            WorkOrderVM result = map.Map<WorkOrder, WorkOrderVM>(newOrder);
            return Ok(result);
        }

        // POST: api/onlineorders/{orderid}/paypalexecute
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpPost]
        [Route("{orderID}/paypalexecute")]
        public ActionResult PaypalExecute(int orderID, [FromBody]PaypalPayment data)
        {
            validatePaypalData(data);

            var order = serv.Get(orderID);        
            if (order.EmployerID != Employer.ID)
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
                serv.Save(order, UserEmail);
            }

            var result = postExecute(data);
            order.ppResponse = result;
            serv.Save(order, UserEmail);
            return new JsonResult(result);
        }

        [NonAction]
        public void validateNoPreviousPayment(WorkOrder wo, PaypalPayment pp)
        {
            if (wo.ppPayerID != null && wo.ppPayerID != pp.payerID)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaypalID already set to {0}, conflicts with {1}", pp.payerID, wo.ppPayerID)),
                    ReasonPhrase = "PaypalID already set to a different ID"
                };
                throw new Exception(res.ToString());
            }
            if (wo.ppPaymentID != null && wo.ppPaymentID != pp.paymentID)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaymentID already set to {0}, conflicts with {1}", pp.paymentID, wo.ppPaymentID)),
                    ReasonPhrase = "PaymentID already set to a different ID"
                };
                throw new Exception(res.ToString());
            }

            if (wo.ppPaymentToken != null && wo.ppPaymentToken != pp.paymentToken)
            {
                var res = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("PaymentToken already set to {0}, conflicts with {1}", pp.paymentToken, wo.ppPaymentToken)),
                    ReasonPhrase = "PaymentToken already set to a different ID"
                };
                throw new Exception(res.ToString());
            }
        }

        [NonAction]
        public void throwInvalidOrder(int id)
        {
            var res = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("Order {0} not found for employer {1}", id, Employer.ID)),
                ReasonPhrase = "Workorder not found"
            };
            throw new Exception(res.ToString());
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
                throw new Exception(res.ToString());
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
                throw new Exception(StatusCode(500, new 
                {
                    ReasonPhrase = "Payment execute failed"
                }).ToString()); // TODO make less ugly
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
            throw new Exception(res.ToString()); //TODO check
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
