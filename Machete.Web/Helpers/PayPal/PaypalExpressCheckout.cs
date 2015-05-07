using NLog;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Helpers.PayPal
{
    public class PaypalExpressCheckout
    {
        Logger log = LogManager.GetCurrentClassLogger();
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "PaypalExpressCheckout", "");

        // # SetExpressCheckout API Operation
        // The SetExpressCheckout API operation initiates an Express Checkout transaction. 
        public SetExpressCheckoutResponseType SetExpressCheckout(string payment)
        {
            // Create the SetExpressCheckoutResponseType object
            SetExpressCheckoutResponseType responseSetExpressCheckoutResponseType = new SetExpressCheckoutResponseType();

            try
            {
                // # SetExpressCheckoutReq
                SetExpressCheckoutRequestDetailsType setExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();

                // URL to which the buyer's browser is returned after choosing to pay
                // with PayPal. For digital goods, you must add JavaScript to this page
                // to close the in-context experience.
                // `Note:
                // PayPal recommends that the value be the final review page on which
                // the buyer confirms the order and payment or billing agreement.`
                setExpressCheckoutRequestDetails.ReturnURL = System.Web.Configuration.WebConfigurationManager.AppSettings["HostingEndpoint"] + "/HirerWorkOrder/PaymentPost";

                // URL to which the buyer is returned if the buyer does not approve the
                // use of PayPal to pay you. For digital goods, you must add JavaScript
                // to this page to close the in-context experience.
                // `Note:
                // PayPal recommends that the value be the original page on which the
                // buyer chose to pay with PayPal or establish a billing agreement.`
                setExpressCheckoutRequestDetails.CancelURL = System.Web.Configuration.WebConfigurationManager.AppSettings["HostingEndpoint"] + "/HirerWorkOrder/PaymentCancel";

                // # Payment Information
                // list of information about the payment
                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();

                // information about the first payment
                PaymentDetailsType paymentDetails1 = new PaymentDetailsType();

                // Total cost of the transaction to the buyer. If shipping cost and tax
                // charges are known, include them in this value. If not, this value
                // should be the current sub-total of the order.
                //
                // If the transaction includes one or more one-time purchases, this field must be equal to
                // the sum of the purchases. Set this field to 0 if the transaction does
                // not include a one-time purchase such as when you set up a billing
                // agreement for a recurring payment that is not immediately charged.
                // When the field is set to 0, purchase-specific fields are ignored.
                //
                // * `Currency Code` - You must set the currencyID attribute to one of the
                // 3-character currency codes for any of the supported PayPal
                // currencies.
                // * `Amount`
                BasicAmountType orderTotal1 = new BasicAmountType(CurrencyCodeType.USD, payment);
                paymentDetails1.OrderTotal = orderTotal1;
                paymentDetails1.OrderDescription = System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalDescription"];

                // How you want to obtain payment. When implementing parallel payments,
                // this field is required and must be set to `Order`. When implementing
                // digital goods, this field is required and must be set to `Sale`. If the
                // transaction does not include a one-time purchase, this field is
                // ignored. It is one of the following values:
                //
                // * `Sale` - This is a final sale for which you are requesting payment
                // (default).
                // * `Authorization` - This payment is a basic authorization subject to
                // settlement with PayPal Authorization and Capture.
                // * `Order` - This payment is an order authorization subject to
                // settlement with PayPal Authorization and Capture.
                // `Note:
                // You cannot set this field to Sale in SetExpressCheckout request and
                // then change the value to Authorization or Order in the
                // DoExpressCheckoutPayment request. If you set the field to
                // Authorization or Order in SetExpressCheckout, you may set the field
                // to Sale.`
                paymentDetails1.PaymentAction = PaymentActionCodeType.SALE;

                // Unique identifier for the merchant. For parallel payments, this field
                // is required and must contain the Payer Id or the email address of the
                // merchant.
                SellerDetailsType sellerDetails1 = new SellerDetailsType();
                sellerDetails1.PayPalAccountID = System.Web.Configuration.WebConfigurationManager.AppSettings["PayPalAccountID"];
                paymentDetails1.SellerDetails = sellerDetails1;

                // A unique identifier of the specific payment request, which is
                // required for parallel payments.
                paymentDetails1.PaymentRequestID = "PaymentRequest1";

                paymentDetailsList.Add(paymentDetails1);

                setExpressCheckoutRequestDetails.PaymentDetails = paymentDetailsList;

                SetExpressCheckoutReq setExpressCheckout = new SetExpressCheckoutReq();
                SetExpressCheckoutRequestType setExpressCheckoutRequest = new SetExpressCheckoutRequestType(setExpressCheckoutRequestDetails);

                setExpressCheckout.SetExpressCheckoutRequest = setExpressCheckoutRequest;

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
                DoExpressCheckoutPaymentReq doExpressCheckoutPayment = new DoExpressCheckoutPaymentReq();

                DoExpressCheckoutPaymentRequestDetailsType doExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();

                // The timestamped token value that was returned in the
                // `SetExpressCheckout` response and passed in the
                // `GetExpressCheckoutDetails` request.
                doExpressCheckoutPaymentRequestDetails.Token = token;

                // Unique paypal buyer account identification number as returned in
                // `GetExpressCheckoutDetails` Response
                doExpressCheckoutPaymentRequestDetails.PayerID = payerId;

                // # Payment Information
                // list of information about the payment
                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();

                // information about the first payment
                PaymentDetailsType paymentDetails1 = new PaymentDetailsType();

                // Total cost of the transaction to the buyer. If shipping cost and tax
                // charges are known, include them in this value. If not, this value
                // should be the current sub-total of the order.
                //
                // If the transaction includes one or more one-time purchases, this field must be equal to
                // the sum of the purchases. Set this field to 0 if the transaction does
                // not include a one-time purchase such as when you set up a billing
                // agreement for a recurring payment that is not immediately charged.
                // When the field is set to 0, purchase-specific fields are ignored.
                //
                // * `Currency Code` - You must set the currencyID attribute to one of the
                // 3-character currency codes for any of the supported PayPal
                // currencies.
                // * `Amount`
                BasicAmountType orderTotal1 = new BasicAmountType(CurrencyCodeType.USD, payment);
                paymentDetails1.OrderTotal = orderTotal1;
                paymentDetails1.OrderDescription = System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalDescription"];

                // How you want to obtain payment. When implementing parallel payments,
                // this field is required and must be set to `Order`. When implementing
                // digital goods, this field is required and must be set to `Sale`. If the
                // transaction does not include a one-time purchase, this field is
                // ignored. It is one of the following values:
                //
                // * `Sale` - This is a final sale for which you are requesting payment
                // (default).
                // * `Authorization` - This payment is a basic authorization subject to
                // settlement with PayPal Authorization and Capture.
                // * `Order` - This payment is an order authorization subject to
                // settlement with PayPal Authorization and Capture.
                // Note:
                // You cannot set this field to Sale in SetExpressCheckout request and
                // then change the value to Authorization or Order in the
                // DoExpressCheckoutPayment request. If you set the field to
                // Authorization or Order in SetExpressCheckout, you may set the field
                // to Sale.
                paymentDetails1.PaymentAction = PaymentActionCodeType.SALE;

                // Unique identifier for the merchant. For parallel payments, this field
                // is required and must contain the Payer Id or the email address of the
                // merchant.
                SellerDetailsType sellerDetails1 = new SellerDetailsType();
                sellerDetails1.PayPalAccountID = System.Web.Configuration.WebConfigurationManager.AppSettings["PayPalAccountID"];
                paymentDetails1.SellerDetails = sellerDetails1;

                // A unique identifier of the specific payment request, which is
                // required for parallel payments.
                paymentDetails1.PaymentRequestID = "PaymentRequest1";

                // A unique identifier of the specific payment request, which is
                // required for parallel payments.
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