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
        Mock<IWorkerRequestService> _reqServ;
        Mock<ICollection<WorkerRequest>> _requests;

        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkOrderController_index_get_returns_enumerable_list()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void WorkOrderController_create_get_returns_workOrder()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            int EmployerID = 4242;
            //Act
            var result = (PartialViewResult)_ctrlr.Create(0);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod]
        public void WorkOrderController_create_post_valid_redirects_to_Index()
        {
            //Arrange
            var workOrder = new WorkOrder();
            var _model = new WorkOrder();
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _serv.Setup(p => p.CreateWorkOrder(workOrder, "UnitTest")).Returns(workOrder);
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            
            
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(_model, "UnitTest");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void WorkOrderController_create_post_invalid_returns_view()
        {
            //Arrange
            var workOrder = new WorkOrder();
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            _serv.Setup(p => p.CreateWorkOrder(workOrder, "UnitTest")).Returns(workOrder);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (PartialViewResult)_ctrlr.Create(workOrder, "UnitTest");
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
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
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
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("workSiteAddress1", "blah");     //Every required field must be populated,
            fakeform.Add("city", "UnitTest");  //or result will be null.
            fakeform.Add("state", "WA");
            fakeform.Add("phone", "1234567890");
            fakeform.Add("zipcode", "12345-6789");
            fakeform.Add("typeOfWorkID", "1");
            fakeform.Add("dateTimeofWork", "1/1/2011");
            fakeform.Add("hourlyWage", "12.50");
            fakeform.Add("days", "1");
            fakeform.Add("transportMethodID", "1");
            fakeform.Add("transportFee", "20.00");
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
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkOrder, savedworkOrder);
            Assert.AreEqual(savedworkOrder.workSiteAddress1, "blah");
            Assert.AreEqual(savedworkOrder.city, "UnitTest");
            Assert.AreEqual(savedworkOrder.state, "WA");
            Assert.AreEqual(savedworkOrder.phone, "1234567890");
            Assert.AreEqual(savedworkOrder.zipcode, "12345-6789");
            Assert.AreEqual(savedworkOrder.typeOfWorkID, 1);
            //Assert.AreEqual(savedworkOrder.dateTimeofWork, "1/1/2011");
            //Assert.AreEqual(savedworkOrder.hourlyWage, "12.50");
            //Assert.AreEqual(savedworkOrder.days, "1");
            //Assert.AreEqual(savedworkOrder.transportMethodID, "1");
            //Assert.AreEqual(savedworkOrder.transportFee, "20.00");

        }

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
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            var result = (ViewResult)_ctrlr.Edit(testid, fakeform, "UnitTest");
            //Assert
            var error = result.ViewData.ModelState["TestError"].Errors[0];
            Assert.AreEqual("foo", error.ErrorMessage);
        }
        #endregion

        //
        // Testing /Delete functionality
        //
        [TestMethod]
        public void WorkOrderController_delete_get_returns_workOrder()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            _reqServ = new Mock<IWorkerRequestService>();
            _serv.Setup(p => p.GetWorkOrder(testid)).Returns(fakeworkOrder);
            _waServ = new Mock<IWorkAssignmentService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Delete(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod]
        public void WorkOrderController_delete_post_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IWorkOrderService>();
            _empServ = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            _waServ = new Mock<IWorkAssignmentService>();
            _reqServ = new Mock<IWorkerRequestService>();
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

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
            _reqServ = new Mock<IWorkerRequestService>();
            _serv.Setup(x => x.GetWorkOrder(It.IsAny<int>())).Returns(fakeorder);
    
            var _ctrlr = new WorkOrderController(_serv.Object, _waServ.Object, _empServ.Object, _reqServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.AddRequest(testid, testworkerid, fakeform, "UnitTest");
            //Assert
            Assert.IsFalse(result);
        }
    }
}
