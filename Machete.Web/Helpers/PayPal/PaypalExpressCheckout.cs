using NLog;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System.Collections.Generic;

namespace Machete.Web.Helpers.PayPal
{
    public class PaypalExpressCheckout
    {
        Logger log = LogManager.GetCurrentClassLogger();
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "PaypalExpressCheckout", "");
        private IDefaults def;

        public PaypalExpressCheckout(IDefaults def)
        {
            this.def = def;
        }
        // # SetExpressCheckout API Operation
        // The SetExpressCheckout API operation initiates an Express Checkout transaction. 
        public SetExpressCheckoutResponseType SetExpressCheckout(string payment)
        {
            // Create the SetExpressCheckoutResponseType object
            SetExpressCheckoutResponseType responseSetExpressCheckoutResponseType = new SetExpressCheckoutResponseType();

            try
            {
                // # SetExpressCheckoutReq
                var checkoutRequestDetails = new SetExpressCheckoutRequestDetailsType();

                checkoutRequestDetails.ReturnURL = def.getConfig("HostingEndpoint") + "/HirerWorkOrder/PaymentPost";
                checkoutRequestDetails.CancelURL = def.getConfig("HostingEndpoint") + "/HirerWorkOrder/PaymentCancel";

                // # Payment Information
                // list of information about the payment
                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();

                // information about the first payment
                PaymentDetailsType paymentDetails1 = new PaymentDetailsType() {
                    OrderTotal = new BasicAmountType(CurrencyCodeType.USD, payment),
                    OrderDescription = def.getConfig("PaypalDescription"),
                    PaymentAction = PaymentActionCodeType.SALE,
                    SellerDetails = new SellerDetailsType() { PayPalAccountID = def.getConfig("PayPalAccountID") },
                    PaymentRequestID = "PaymentRequest1"
                };

                paymentDetailsList.Add(paymentDetails1);
                checkoutRequestDetails.PaymentDetails = paymentDetailsList;

                SetExpressCheckoutReq setExpressCheckout = new SetExpressCheckoutReq() {
                    SetExpressCheckoutRequest = new SetExpressCheckoutRequestType(checkoutRequestDetails)
                };

                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                // # API call            
                // Invoke the SetExpressCheckout method in service wrapper object
                responseSetExpressCheckoutResponseType = service.SetExpressCheckout(setExpressCheckout);
            }
            // # Exception log    
            catch (System.Exception ex)
            {
                // Log the exception message       
                levent.Level = LogLevel.Error;
                levent.Message = "Logon failed for PaypalExpressCheckout: " + ex.Message;
                log.Log(levent);
            }
            return responseSetExpressCheckoutResponseType;
        }

        // # GetExpressCheckout API Operation
        // The GetExpressCheckoutDetails API operation obtains information about an Express Checkout transaction
        public GetExpressCheckoutDetailsResponseType GetExpressCheckoutDetails(string token)
        {
            // Create the GetExpressCheckoutDetailsResponseType object
            GetExpressCheckoutDetailsResponseType responseGetExpressCheckoutDetailsResponseType = new GetExpressCheckoutDetailsResponseType();

            try
            {
                // Create the GetExpressCheckoutDetailsReq object
                GetExpressCheckoutDetailsReq getExpressCheckoutDetails = new GetExpressCheckoutDetailsReq();

                // A timestamped token, the value of which was returned by `SetExpressCheckout` response
                GetExpressCheckoutDetailsRequestType getExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token);
                getExpressCheckoutDetails.GetExpressCheckoutDetailsRequest = getExpressCheckoutDetailsRequest;

                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                // # API call
                // Invoke the GetExpressCheckoutDetails method in service wrapper object
                responseGetExpressCheckoutDetailsResponseType = service.GetExpressCheckoutDetails(getExpressCheckoutDetails);
            }
            // # Exception log    
            catch (System.Exception ex)
            {
                // Log the exception message       
                levent.Level = LogLevel.Error;
                levent.Message = "Logon failed for PaypalExpressCheckout: " + ex.Message;
                log.Log(levent);
            }
            return responseGetExpressCheckoutDetailsResponseType;
        }

        // # DoExpressCheckoutPayment API Operation
        // The DoExpressCheckoutPayment API operation completes an Express Checkout transaction. 
        // If you set up a billing agreement in your SetExpressCheckout API call, 
        // the billing agreement is created when you call the DoExpressCheckoutPayment API operation. 
        public DoExpressCheckoutPaymentResponseType DoExpressCheckoutPayment(string token, string payerId, string payment)
        {
            // Create the DoExpressCheckoutPaymentResponseType object
            DoExpressCheckoutPaymentResponseType responseDoExpressCheckoutPaymentResponseType = new DoExpressCheckoutPaymentResponseType();

            try
            {
                // Create the DoExpressCheckoutPaymentReq object
                var doExpressCheckoutPayment = new DoExpressCheckoutPaymentReq();
                var doExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType() {
                    Token = token,
                    PayerID = payerId
                };

                var paymentDetailsList = new List<PaymentDetailsType>();

                // information about the first payment
                var paymentDetails1 = new PaymentDetailsType() {
                    OrderTotal = new BasicAmountType(CurrencyCodeType.USD, payment),
                    OrderDescription = def.getConfig("PaypalDescription"),
                    PaymentAction = PaymentActionCodeType.SALE,
                    SellerDetails = new SellerDetailsType() { PayPalAccountID = def.getConfig("PayPalAccountID") },
                    PaymentRequestID = "PaymentRequest1",
                };

                paymentDetailsList.Add(paymentDetails1);
                doExpressCheckoutPaymentRequestDetails.PaymentDetails = paymentDetailsList;

                DoExpressCheckoutPaymentRequestType doExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType(doExpressCheckoutPaymentRequestDetails);
                doExpressCheckoutPayment.DoExpressCheckoutPaymentRequest = doExpressCheckoutPaymentRequest;

                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                // # API call
                // Invoke the DoExpressCheckoutPayment method in service wrapper object
                responseDoExpressCheckoutPaymentResponseType = service.DoExpressCheckoutPayment(doExpressCheckoutPayment);

                return responseDoExpressCheckoutPaymentResponseType;
            }
            // # Exception log    
            catch (System.Exception ex)
            {
                // Log the exception message       
                levent.Level = LogLevel.Error;
                levent.Message = "Logon failed for PaypalExpressCheckout: " + ex.Message;
                log.Log(levent);
            }
            return responseDoExpressCheckoutPaymentResponseType;
        }

    }
}