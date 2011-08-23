using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Machete.Data;
using Machete.Service;
using Machete.Data.Infrastructure;
using Machete.Web.Controllers;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Test;
using Machete.Web.ViewModel;
using System.Data.Entity;

namespace Machete.Test.Controllers
{
    /// <summary>
    /// Summary description for WorkOrderControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkOrdersControllerTests
    {
        Mock<IWorkOrderService> _serv;
        Mock<IEmployerService> _empServ;
        Mock<IWorkAssignmentService> _waServ;
        Mock<IWorkerService> _reqServ;
        Mock<IWorkerRequestService> _wrServ;
        //Mock<ICollection<WorkerRequest>> _requests;
        FormCollection fullfakeform;
        List<WorkerRequest> workerRequest;
        WorkOrderController _ctrlr;
        int testid = 4242;
        //
        [TestInitialize]
        public void TestInitialize()
        {
            fullfakeform = new FormCollection();
            fullfakeform.Add("ID", testid.ToString());
            fullfakeform.Add("workSiteAddress1", "blah");     //Every required field must be populated,
            fullfakeform.Add("city", "UnitTest");  //or result will be null.
            fullfakeform.Add("state", "WA");
            fullfakeform.Add("phone", "123-456-7890");
            fullfakeform.Add("zipcode", "12345-6789");
            fullfakeform.Add("typeOfWorkID", "1");
            fullfakeform.Add("dateTimeofWork", "1/1/2011");
            fullfakeform.Add("transportMethodID", "1");
            fullfakeform.Add("transportFee", "20.00");
            fullfakeform.Add("contactName", "test script contact name");
            //fullfakeform.Add("workerRequests", "30123,301234,30122,12345");
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerService>();
            _wrServ = new Mock<IWorkerRequestService>();
            workerRequest = new List<WorkerRequest> { };
            _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkOrderController_index_get_returns_enumerable_list()
        {
            var result = (ViewResult)_ctrlr.Index();
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void WorkOrderController_create_get_returns_workOrder()
        {
            var result = (PartialViewResult)_ctrlr.Create(0);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod]
        public void WorkOrderController_create_valid_post_returns_JSON()
        {
            //Arrange
            var workOrder = new WorkOrder();
            var _model = new WorkOrder();
            _serv.Setup(p => p.CreateWorkOrder(workOrder, "UnitTest")).Returns(workOrder);                        
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(_model, "UnitTest", workerRequest);
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void WorkOrderController_create_post_invalid_returns_view()
        {
            //Arrange
            var workOrder = new WorkOrder();
            _serv.Setup(p => p.CreateWorkOrder(workOrder, "UnitTest")).Returns(workOrder);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (PartialViewResult)_ctrlr.Create(workOrder, "UnitTest", workerRequest);
            //Assert
            var error = result.ViewData.ModelState["TestError"].Errors[0];
            Assert.AreEqual("foo", error.ErrorMessage);
        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod]
        public void WorkOrderController_edit_get_returns_workOrder()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(fakeworkOrder);
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod]
        public void WorkOrderController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _empServ = new Mock<IEmployerService>();
            _reqServ = new Mock<IWorkerService>();
            int testid = 4242;
            FormCollection fakeform = fullfakeform;
            WorkOrder fakeworkOrder = new WorkOrder();
            WorkOrder savedworkOrder = new WorkOrder();
            string user = "";
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(fakeworkOrder);
            _serv.Setup(x => x.SaveWorkOrder(It.IsAny<WorkOrder>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkOrder p, string str) =>
                                         {
                                             savedworkOrder = p;
                                             user = str;
                                         });
            _waServ = new Mock<IWorkAssignmentService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            List<WorkerRequest> list = new List<WorkerRequest>();
            list.Add(new WorkerRequest { WorkerID = 12345 });
            list.Add(new WorkerRequest { WorkerID = 30002 });
            list.Add(new WorkerRequest { WorkerID = 30311 });
            list.Add(new WorkerRequest { WorkerID = 30420 });
            list.Add(new WorkerRequest { WorkerID = 30421 });
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest", list) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkOrder, savedworkOrder);
            Assert.AreEqual(savedworkOrder.workSiteAddress1, "blah");
            Assert.AreEqual(savedworkOrder.city, "UnitTest");
            Assert.AreEqual(savedworkOrder.state, "WA");
            Assert.AreEqual(savedworkOrder.phone, "123-456-7890");
            Assert.AreEqual(savedworkOrder.zipcode, "12345-6789");
            Assert.AreEqual(savedworkOrder.typeOfWorkID, 1);
            Assert.AreEqual(savedworkOrder.dateTimeofWork, Convert.ToDateTime("1/1/2011 12:00:00 AM"));
            Assert.AreEqual(savedworkOrder.transportMethodID, 1);
            Assert.AreEqual(savedworkOrder.transportFee, Convert.ToDouble("20.00"));
            //Assert.AreEqual(savedworkOrder.workerRequests.Count(), 5);

        }
        [TestMethod]
        public void WorkOrderController_edit_post_workerRequests_finds_duplicates()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _empServ = new Mock<IEmployerService>();
            _reqServ = new Mock<IWorkerService>();
            int testid = 4242;
            FormCollection fakeform = fullfakeform;
            WorkOrder fakeworkOrder = new WorkOrder();
            fakeworkOrder.workerRequests = new List<WorkerRequest>();
            fakeworkOrder.workerRequests.Add(new WorkerRequest
            {
                ID = 111,
                WorkerID = 1,
                WorkOrderID = 4242,
                workerRequested = new Worker { ID = 1, dwccardnum = 12345 }

            });
            fakeworkOrder.workerRequests.Add(new WorkerRequest 
            {
                ID = 222,
                WorkerID = 2,
                WorkOrderID = 4242,
                workerRequested = new Worker { ID = 2, dwccardnum = 12346 } 
            });
            WorkOrder savedworkOrder = new WorkOrder();
            string user = "";
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(fakeworkOrder);
            _serv.Setup(x => x.SaveWorkOrder(It.IsAny<WorkOrder>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkOrder p, string str) =>
                                         {
                                             savedworkOrder = p;
                                             user = str;
                                         });
            _waServ = new Mock<IWorkAssignmentService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            List<WorkerRequest> list = new List<WorkerRequest>();
            list.Add(new WorkerRequest { WorkerID = 12345 });
            list.Add(new WorkerRequest { WorkerID = 30002 });
            list.Add(new WorkerRequest { WorkerID = 30311 });
            list.Add(new WorkerRequest { WorkerID = 30420 });
            list.Add(new WorkerRequest { WorkerID = 30421 });
            //Act

            var result = _ctrlr.Edit(testid, fakeform, "UnitTest", list) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkOrder, savedworkOrder);

            Assert.AreEqual(savedworkOrder.workerRequests.Count(), 5);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 12345), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30002), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30311), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30420), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30421), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 12346), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void WorkOrderController_edit_post_invalid_returns_view()
        {
            //Arrange
            var workOrder = new WorkOrder();
            int testid = 4243;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");
            fakeform.Add("lastname1", "UnitTest");
            fakeform.Add("gender", "M");
            //
            // Mock service and setup SaveWorkOrder mock
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _serv.Setup(p => p.SaveWorkOrder(workOrder, "UnitTest"));
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(workOrder);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            _waServ = new Mock<IWorkAssignmentService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            List<WorkerRequest> list = new List<WorkerRequest>();
            var result = (ViewResult)_ctrlr.Edit(testid, fakeform, "UnitTest", list);
            //Assert
            var error = result.ViewData.ModelState["TestError"].Errors[0];
            Assert.AreEqual("foo", error.ErrorMessage);
        }
        #endregion
        #region delete tests
        /// <summary>
        /// delete GET returns workOrder
        /// </summary>
        [TestMethod]
        public void WorkOrderController_delete_get_returns_workOrder()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            _reqServ = new Mock<IWorkerService>();
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(fakeworkOrder);
            _waServ = new Mock<IWorkAssignmentService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Delete(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }
        /// <summary>
        /// delete POST redirects to index
        /// </summary>
        [TestMethod]
        public void WorkOrderController_delete_post_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerService>();
            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
        #endregion
        //
        // Testing /AddRequest functionality
        //
        [TestMethod]
        public void WorkOrderController_addRequest_finds_duplicate()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            int testworkerid = 4269;
            WorkOrder fakeorder = new WorkOrder();
            fakeorder.workerRequests = new List<WorkerRequest>();
            fakeorder.workerRequests.Add(new WorkerRequest() { WorkerID = 4269 });
            FormCollection fakeform = new FormCollection();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerService>();
            _serv.Setup(x => x.GetWorkOrder(It.IsAny<int>())).Returns(fakeorder);

            _wrServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object, _wrServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            //var result = _ctrlr.AddRequest(testid, testworkerid, fakeform, "UnitTest");
            //Assert
            //Assert.IsFalse(result);
        }
    }
}
