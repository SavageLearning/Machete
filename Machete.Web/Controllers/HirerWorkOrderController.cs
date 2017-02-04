#region COPYRIGHT
// File:     HirerWorkOrderController.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.Helpers.PayPal;
using Machete.Web.Resources;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NLog;
using PayPal.Exception;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class HirerWorkOrderController : MacheteController
    {
        private readonly IWorkOrderService woServ;
        private readonly IEmployerService eServ;
        private readonly IWorkerService wServ;
        private readonly IWorkerRequestService wrServ;
        private readonly IWorkAssignmentService waServ;
        private readonly ILookupCache lcache;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;
        Logger log = LogManager.GetCurrentClassLogger();
        LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "HirerWorkOrderController", "");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="woServ">Work Order service</param>
        /// <param name="waServ">Work Assignment service</param>
        /// <param name="eServ">Employer service</param>
        /// <param name="wServ">Worker service</param>
        /// <param name="wrServ">Worker request service</param>
        /// <param name="lcache">Lookup cache</param>
        public HirerWorkOrderController(IWorkOrderService woServ,
                                   IWorkAssignmentService waServ,
                                   IEmployerService eServ,
                                   IWorkerService wServ,
                                   IWorkerRequestService wrServ,
                                   ILookupCache lcache,
                                   IDefaults def,
                                   IMapper map
            )
        {
            this.woServ = woServ;
            this.eServ = eServ;
            this.wServ = wServ;
            this.waServ = waServ;
            this.wrServ = wrServ;
            this.lcache = lcache;
            this.map = map;
            this.def = def;
            ViewBag.employerReferenceList = def.getSelectList(Machete.Domain.LCategory.emplrreference);
            ViewBag.OnlineEmployerSkill = def.getOnlineEmployerSkill();
            ViewBag.YesNoSelectList = def.yesnoSelectList();
            ViewBag.TransportMethodList = def.getTransportationMethodList();
        }

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.CI = (CultureInfo)Session["Culture"];
        }

        #region Index

        /// <summary>
        /// HTTP GET /HirerWorkOrder/Index
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Index()
        {
            ViewBag.employerId = null;

            // Retrieve employer ID of signed in Employer
            string employerID = HttpContext.User.Identity.GetUserId();

            Domain.Employer employer = eServ.GetRepo().GetAllQ().Where(e => e.onlineSigninID == employerID).FirstOrDefault();
            if (employer != null)
            {
                ViewBag.employerId = employer.ID;
            }

            return View("Index");
        }

        #endregion

        #region List

        /// <summary>
        /// HTTP GET /HirerWorkOrder/List
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult List()
        {
            return View();
        }

        #endregion

        #region Ajaxhandler
        /// <summary>
        /// Determines displayState value in WorkOrder/AjaxHandler. Display state is used to provide color highlighting to records based on state.
        /// The displayState is not presented to the user, so don't have to provide internationalization text.
        /// </summary>
        /// <param name="wo">WorkOrder</param>
        /// <returns>status string</returns>
        private string _getDisplayState(WorkOrder wo)
        {
            string status = lcache.textByID(wo.statusID, "en");

            if (wo.statusID == WorkOrder.iCompleted)
            {
                // If WO is completed, but 1 (or more) WA aren't assigned - the WO is still Unassigned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID == null) > 0)
                {
                    return "Unassigned";
                }
                // If WO is completed, but 1 (or more) Assigned Worker(s) never signed in, then the WO has been Orphaned
                if (wo.workAssignments.Count(wa => wa.workerAssignedID != null && wa.workerSigninID == null) > 0)
                {
                    return "Orphaned";
                }
            }
            return status;
        }

        #endregion

        #region Create

        /// <summary>
        /// HTTP GET /HirerWorkOrder/Create
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Create()
        {
            WorkOrder wo = new WorkOrder();

            // Retrieve user ID of signed in Employer
            string userID = HttpContext.User.Identity.GetUserId();

            // Retrieve Employer record
            Domain.Employer employer = eServ.GetRepo().GetAllQ().Where(e => e.onlineSigninID == userID).FirstOrDefault();
            if (employer != null)
            {
                // Employer has created profile or work order already

                // Associate WO with Employer
                wo.EmployerID = employer.ID;

                // Set up default values from Employer record
                wo.contactName = employer.name;
                wo.phone = employer.phone;
                wo.workSiteAddress1 = employer.address1;
                wo.workSiteAddress2 = employer.address2;
                wo.city = employer.city;
                wo.state = employer.state;
                wo.zipcode = employer.zipcode;
            }

            // Set default values
            wo.dateTimeofWork = DateTime.Today.AddHours(9).AddDays(3); // Set default work time to 9am three days from now
            wo.transportMethodID = def.getDefaultID(LCategory.transportmethod);
            wo.typeOfWorkID = def.getDefaultID(LCategory.worktype);
            wo.statusID = def.getDefaultID(LCategory.orderstatus);
            wo.timeFlexible = true;
            wo.onlineSource = true;
            wo.disclosureAgreement = false;
            ViewBag.workerRequests = new List<SelectListItem> { };

            // Build Skill lookups
            ViewBag.ID = new int[22];
            ViewBag.text_EN = new string[22];
            ViewBag.text_ES = new string[22];
            ViewBag.wage = new double[22];
            ViewBag.minHour = new int[22];
            ViewBag.workType = new int[22];
            ViewBag.desc_ES = new string[22];
            ViewBag.desc_EN = new string[22];


            //IEnumerable<Lookup> lookup = lcache.getCache();
            List<SelectListEmployerSkills> lookup = def.getOnlineEmployerSkill();

            int counter = 0;

            for (int i = 0; i < lookup.Count(); i++)
            {
                SelectListEmployerSkills lup = lookup.ElementAt(i);
                //Lookup lup = lookup.ElementAt(i);
                //if (lup.ID == 60 || lup.ID == 61 || lup.ID == 62 || lup.ID == 63 || lup.ID == 64 || lup.ID == 65 || lup.ID == 66 || lup.ID == 67 || lup.ID == 68 || lup.ID == 69 || lup.ID == 77 || lup.ID == 83 || lup.ID == 88 || lup.ID == 89 || lup.ID == 118 || lup.ID == 120 || lup.ID == 122 || lup.ID == 128 || lup.ID == 131 || lup.ID == 132 || lup.ID == 133 || lup.ID == 183)
                if (lup.ID == 60 || lup.ID == 61 || lup.ID == 62 || lup.ID == 63 || lup.ID == 64 || lup.ID == 65 || lup.ID == 66 || lup.ID == 67 || lup.ID == 68 || lup.ID == 69 || lup.ID == 77 || lup.ID == 83 || lup.ID == 88 || lup.ID == 89 || lup.ID == 118 || lup.ID == 120 || lup.ID == 122 || lup.ID == 128 || lup.ID == 131 || lup.ID == 132 || lup.ID == 133 || lup.ID == 183)
                {
                    ViewBag.ID[counter] = lup.ID;
                    ViewBag.wage[counter] = lup.wage;
                    ViewBag.minHour[counter] = lup.minHour;
                    ViewBag.workType[counter] = lup.typeOfWorkID;
                    ViewBag.text_EN[counter] = lup.skillDescriptionEn;
                    ViewBag.text_ES[counter] = lup.skillDescriptionEs;
                    counter++;
                }
            }

            return PartialView("Create", wo);
        }

        /// <summary>
        /// POST: /HirerWorkOrder/Create
        /// </summary>
        /// <param name="wo">WorkOrder to create</param>
        /// <param name="userName">User performing action</param>
        /// <param name="workerAssignments">List of worker assignments & requests (stringified JSON object with array of Assignments objects & array of Requests objects</param>
        /// <returns>JSON Object representing new Work Order</returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult Create(WorkOrder wo, string userName, string workerAssignments)
        {
            UpdateModel(wo);

            // Retrieve user ID of signed in Employer
            string userID = HttpContext.User.Identity.GetUserId();

            // Retrieve Employer record
            Domain.Employer onlineEmployer = eServ.GetRepo().GetAllQ().Where(e => e.onlineSigninID == userID).FirstOrDefault();
            if (onlineEmployer != null)
            {
                // Employer has created profile or work order already

                // Associate WO with Employer
                wo.EmployerID = onlineEmployer.ID;
            }
            else
            {
                // Employer has NOT created profile or work order yet
                Domain.Employer employer = new Domain.Employer();

                // Set up default values from WO
                employer.name = wo.contactName;
                employer.phone = wo.phone;
                employer.address1 = wo.workSiteAddress1;
                employer.address2 = wo.workSiteAddress2;
                employer.city = wo.city;
                employer.state = wo.state;
                employer.zipcode = wo.zipcode;

                // Set up default online Employer profile
                employer.isOnlineProfileComplete = true;
                employer.onlineSigninID = userID;
                employer.email = HttpContext.User.Identity.GetUserName(); // The Employer's username is their email address
                employer.active = true;
                employer.business = false;
                employer.blogparticipate = false;
                employer.onlineSource = true;
                employer.returnCustomer = false;
                employer.receiveUpdates = true;
                employer.business = false;

                Domain.Employer newEmployer = eServ.Create(employer, userName);
                if (newEmployer != null)
                {
                    wo.EmployerID = newEmployer.ID;
                }
            }

            // Set disclosure agreement - to get here, the user had to accept
            wo.disclosureAgreement = true;

            if (workerAssignments == "")
            {
                // Set WA counter 
                wo.waPseudoIDCounter = 0;
            }

            // Create Work Order
            WorkOrder neworder = woServ.Create(wo, userName);

            if (workerAssignments != "")
            {
                // Create Work Assignments
                dynamic parsedWorkerRequests = JObject.Parse(workerAssignments);

                // Set WA counter 
                wo.waPseudoIDCounter = parsedWorkerRequests["assignments"].Count;
                woServ.Save(neworder, userName);

                for (int i = 0; i < parsedWorkerRequests["assignments"].Count; i++)
                {
                    WorkAssignment wa = new WorkAssignment();


                    // Create WA from Employer data
                    wa.workOrderID = neworder.ID;
                    wa.skillID = parsedWorkerRequests["assignments"][i].skillId;
                    wa.hours = parsedWorkerRequests["assignments"][i].hours;
                    wa.weightLifted = parsedWorkerRequests["assignments"][i].weight;
                    wa.hourlyWage = parsedWorkerRequests["assignments"][i].hourlyWage; // TODO: consider looking this up instead - however, this is the value quoted to the customer online
                    wa.pseudoID = i + 1;
                    wa.description = parsedWorkerRequests["assignments"][i].desc;

                    // Set up defaults
                    wa.active = true;
                    wa.englishLevelID = 0; // TODO: note- all incoming online work assignments won't have the proper English level set (this needs to be set by the worker center)
                    wa.surcharge = 0.0;
                    wa.days = 1;
                    wa.qualityOfWork = 0;
                    wa.followDirections = 0;
                    wa.attitude = 0;
                    wa.reliability = 0;
                    wa.transportProgram = 0;

                    WorkAssignment newWa = waServ.Create(wa, userName);
                }

                // TODO: test
                // New Worker Requests to add
                for (int i = 0; i < parsedWorkerRequests["requests"].Count; i++)
                {
                    WorkerRequest wr = new WorkerRequest();

                    // Create Worker Request from Employer data
                    wr.WorkOrderID = neworder.ID;
                    wr.WorkerID = parsedWorkerRequests["requests"][i].workerId;

                    WorkerRequest newWr = wrServ.Create(wr, userName);
                }
            }

            if (neworder.transportFee > 0)
            {
                return View("IndexPrePaypal", neworder);
            }
            else
            {
                return View("IndexComplete", neworder);
            }
        }

        #endregion

        #region Profile

        /// <summary>
        /// HTTP GET /HirerWorkOrder/Profile
        /// </summary>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult HirerProfile()
        {
            // Retrieve user ID of signed in Employer
            string userID = HttpContext.User.Identity.GetUserId();

            // Retrieve Employer record
            Domain.Employer employer = eServ.GetRepo().GetAllQ().Where(e => e.onlineSigninID == userID).FirstOrDefault();
            if (employer != null)
            {
                // Employer has created profile or work order already
            }
            else
            {
                employer = new Domain.Employer();
            }

            // Set default values
            employer.active = true;
            employer.onlineSource = true;
            employer.returnCustomer = false;
            employer.receiveUpdates = true;
            employer.business = false;
            employer.email = HttpContext.User.Identity.Name;
            employer.onlineSigninID = userID;
            employer.isOnlineProfileComplete = true;

            return PartialView("Profile", employer);
        }

        /// <summary>
        /// POST: /HirerWorkOrder/Profile
        /// </summary>
        /// <param name="e">Employer record to update/create</param>
        /// <param name="wo">Profile to create</param>
        /// <param name="userName">User performing action</param>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult HirerProfile(Domain.Employer e, string userName)
        {
            UpdateModel(e);

            // Retrieve user ID of signed in Employer
            string userID = HttpContext.User.Identity.GetUserId();

            // Retrieve Employer record
            Domain.Employer onlineEmp = eServ.GetRepo().GetAllQ().Where(emp => emp.onlineSigninID == userID).FirstOrDefault();

            if (onlineEmp != null)
            {
                Domain.Employer onlineEmployer = eServ.Get(onlineEmp.ID);
                //e.ID = onlineEmployer.ID;
                onlineEmployer.active = true;
                onlineEmployer.address1 = e.address1;
                onlineEmployer.address2 = e.address2;
                onlineEmployer.business = e.business;
                onlineEmployer.businessname = e.businessname;
                onlineEmployer.cellphone = e.cellphone;
                onlineEmployer.city = e.city;
                onlineEmployer.email = e.email;
                onlineEmployer.isOnlineProfileComplete = true;
                onlineEmployer.name = e.name;
                onlineEmployer.onlineSigninID = HttpContext.User.Identity.GetUserId();
                onlineEmployer.onlineSource = true;
                onlineEmployer.phone = e.phone;
                onlineEmployer.receiveUpdates = e.receiveUpdates;
                onlineEmployer.referredby = e.referredby;
                onlineEmployer.referredbyOther = e.referredbyOther;
                onlineEmployer.returnCustomer = e.returnCustomer;
                onlineEmployer.state = e.state;
                onlineEmployer.zipcode = e.zipcode;
                // Employer has created profile already - just need to update profile
                eServ.Save(onlineEmployer, userName);
                return View("Index");
            }
            else
            {
                // Create Employer record
                Domain.Employer newEmployer = eServ.Create(e, userName);
                return View("Index");
            }
        }

        #endregion

        #region Edit

        /// <summary>
        /// GET: /HirerWorkOrder/Edit/ID
        /// </summary>
        /// <param name="id">WorkOrder ID</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult Edit(int id)
        {
            // Retrieve Work Order
            WorkOrder workOrder = woServ.Get(id);

            return PartialView("Edit", workOrder);
        }

        #endregion

        #region View

        /// <summary>
        /// GET: /HirerWorkOrder/PaymentPost
        /// </summary>
        /// <param name="token">PayPal token</param>
        /// <param name="payerId">PayPal Payer ID</param>
        /// <param name="userName">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult PaymentPost(string token, string payerId, string userName)
        {
            double payment = 0.0;

            // TODO: There was an issue with the WO returned by the first query below - the work order
            // can't be saved unless the work order is retrieved with the woServ.Get() call
            WorkOrder woAll = woServ.GetRepo().GetAllQ().Where(wo => wo.paypalToken == token).FirstOrDefault();
            WorkOrder workOrder = woServ.Get(woAll.ID);
            if (workOrder != null)
            {
                if (workOrder.transportFee <= 0.0)
                {
                    levent.Level = LogLevel.Error;
                    levent.Message = "There is no transportation fee associated with this work order - there is no PayPal transaction required. WO#:" + workOrder.ID;
                    log.Log(levent);
                    return View("IndexError", workOrder);
                }
                else
                {
                    payment = workOrder.transportFee;
                }
            }
            else
            {
                levent.Level = LogLevel.Error;
                levent.Message = "WorkOrder ID not valid Work Order. WO#:" + workOrder.ID;
                log.Log(levent);
                return View("IndexError", workOrder);
            }

            // PayPal call to get buyer details
            PaypalExpressCheckout paypal = new PaypalExpressCheckout();
            GetExpressCheckoutDetailsResponseType detailsResponse = paypal.GetExpressCheckoutDetails(token);

            if (detailsResponse != null)
            {
                // # Success values
                if (detailsResponse.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                {
                    if ((detailsResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails == null) ||
                        (Convert.ToDouble(detailsResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails[0].OrderTotal.value) != workOrder.transportFee))
                    {
                        // PayPal charge is different than transportFee in database - can't process payment
                        levent.Level = LogLevel.Error;
                        levent.Message = "Transport Fee request to PayPal is a different amount than associated with the WO in database. WO# " + workOrder.ID;
                        log.Log(levent);
                        return View("IndexError", workOrder);
                    }

                    // Unique PayPal Customer Account identification number. This value will be null unless 
                    // you authorize the payment by redirecting to PayPal after `SetExpressCheckout` call.
                    workOrder.paypalPayerId = detailsResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;

                    woServ.Save(workOrder, userName);

                }
                // # Error Values
                else
                {
                    List<ErrorType> errorMessages = detailsResponse.Errors;
                    foreach (ErrorType error in errorMessages)
                    {
                        levent.Level = LogLevel.Error;
                        levent.Message += error.LongMessage;
                        log.Log(levent);

                        workOrder.paypalErrors += error.ShortMessage;
                    }

                    // Save work order updates
                    woServ.Save(workOrder, userName);

                    return View("IndexError", workOrder);
                }
            }
            else
            {
                levent.Level = LogLevel.Error;
                levent.Message = "The response from PayPal GetExpressCheckoutDetailsResponseType API was null. WO#:" + workOrder.ID;
                log.Log(levent);
                return View("IndexError", workOrder);
            }

            return View("IndexPostPaypal", workOrder);

        }

        /// <summary>
        /// GET: /HirerWorkOrder/PaymentCancel
        /// </summary>
        /// <param name="token">PayPal Token</param>
        /// <returns>MVC Action Result</returns>
        [Authorize(Roles = "Hirer")]
        public ActionResult PaymentCancel(string token, string orderID)
        {

            return View("IndexCancel");
        }

        /// <summary>
        /// POST: /HirerWorkOrder/PaymentCancel
        /// </summary>
        /// <param name="token">PayPal Token</param>
        /// <returns>MVC Action Result</returns>
        [HttpPost]
        [Authorize(Roles = "Hirer")]
        public ActionResult PaymentCancel(string token2)
        {
            WorkOrder woAll = woServ.GetRepo().GetAllQ().Where(wo => wo.paypalToken == token2).FirstOrDefault();
            WorkOrder workOrder = woServ.Get(woAll.ID);
            if (workOrder == null)
            {
                levent.Level = LogLevel.Error;
                levent.Message = "WorkOrder ID not valid Work Order. WO#:" + workOrder.ID;
                log.Log(levent);
                return View("IndexError", workOrder);
            }

            return View("IndexCancel", workOrder);
        }

        /// <summary>
        /// GET: /HirerWorkOrder/PaymentComplete
        /// </summary>
        /// <param name="token">PayPal Token</param>
        /// <param name="payerId">PayPal Payer ID</param>
        /// <param name="userName">User performing action</param>
        /// <returns>MVC Action Result</returns>
        [UserNameFilter]
        [Authorize(Roles = "Hirer")]
        public ActionResult PaymentComplete(string userName)
        {
            return View("IndexCompleteEmpty");
        }

        #endregion
    }
}
