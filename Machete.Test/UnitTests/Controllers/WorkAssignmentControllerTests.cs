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

namespace Machete.Test.Controllers
{
    /// <summary>
    /// Summary description for WorkAssignmentControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkAssignmentsControllerTests
    {
        Mock<IWorkAssignmentService> _waServ;
        Mock<IWorkerService> _wkrServ;
        Mock<IWorkOrderService> _woServ;
        Mock<IWorkerSigninService> _wsiServ;
        WorkAssignmentController _ctrlr;
        WorkAssignmentIndex _view;
        FormCollection fakeform;

        [TestInitialize]
        public void TestInitialize()
        {
            _waServ = new Mock<IWorkAssignmentService>();
            _wkrServ = new Mock<IWorkerService>();
            _woServ = new Mock<IWorkOrderService>();
            _wsiServ = new Mock<IWorkerSigninService>();
            _ctrlr = new WorkAssignmentController(_waServ.Object, _wkrServ.Object, _woServ.Object, _wsiServ.Object);
            _view = new WorkAssignmentIndex();
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkAssignmentController_index_get_returns_WorkAssignmentIndexViewModel()
        {
            //Arrange

            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignmentIndex));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void WorkAssignmentController_create_get_returns_workAssignment()
        {
            //Arrange
            //Act
            var result = (PartialViewResult)_ctrlr.Create(0);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignment));
        }

        [TestMethod]
        public void WorkAssignmentController_create_valid_post_returns_json()
        {
            //Arrange            
            WorkAssignment _asmt = new WorkAssignment();
            fakeform.Add("ID", "11");
            fakeform.Add("englishlevelID", "0");
            fakeform.Add("skillID", "60");
            fakeform.Add("hours", "5");
            fakeform.Add("hourlyWage", "12");
            fakeform.Add("days", "1");
            WorkOrder _wo = new WorkOrder();
            _wo.paperOrderNum = 12345;
            _wo.ID = 123;
            int _num = 0;

            string username = "UnitTest";
            _woServ.Setup(p => p.Get(_num)).Returns(() => _wo);
            _waServ.Setup(p => p.Create(_asmt, username)).Returns(() => _asmt);
            
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            JsonResult result = (JsonResult)_ctrlr.Create(_asmt, username);
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = /WorkAssignment/Edit/12345, sNewLabel = Assignment #: 12345-01, iNewID = 12345 }", 
                            result.Data.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void WorkAssignmentController_create_post_invalid_throws_exception()
        {
            //Arrange
            WorkAssignment _asmt = new WorkAssignment();
            fakeform.Add("hours", "invalid data type");
            _waServ.Setup(p => p.Create(_asmt, "UnitTest")).Returns(_asmt);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(_asmt, "UnitTest");
            //Assert
        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod]
        public void WorkAssignmentController_edit_get_returns_workAssignment()
        {
            //Arrange            
            int testid = 4242;
            var fakeworkAssignment = new WorkAssignment();
            fakeworkAssignment.ID = 4243;
            _waServ.Setup(p => p.Get(testid)).Returns(() => fakeworkAssignment);
            //Act
            PartialViewResult result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignment));
        }

        [TestMethod]
        public void WorkAssignmentController_edit_post_valid_updates_model_returns_json()
        {
            //Arrange
            int testid = 4242;
            Worker wkr = new Worker();
            wkr.ID = 424;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("description", "blah");     //Every required field must be populated,
            fakeform.Add("comments", "UnitTest");  //or result will be null.            
            WorkAssignment asmt = new WorkAssignment();
            WorkAssignment savedAsmt = null;
            asmt.workerAssignedID = wkr.ID;
            asmt.ID = testid;
            string user = "";
            _waServ.Setup(p => p.Get(testid)).Returns(asmt);
            _waServ.Setup(x => x.Save(It.IsAny<WorkAssignment>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkAssignment p, string str) =>
                                         {
                                             savedAsmt = p;
                                             user = str;
                                         });
            _wkrServ.Setup(p => p.GetWorker((int)asmt.workerAssignedID)).Returns(wkr);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, null, "UnitTest") as JsonResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ jobSuccess = True }",
                            result.Data.ToString());
            Assert.AreEqual(asmt, savedAsmt);
            Assert.AreEqual(savedAsmt.description, "blah");
            Assert.AreEqual(savedAsmt.comments, "UnitTest");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void WorkAssignmentController_edit_post_invalid_throws_exception()
        {
            //Arrange
            var asmt = new WorkAssignment();
            Worker wkr = new Worker();
            wkr.ID = 424;
            int testid = 4243;
            asmt.ID = testid;
            asmt.workerAssignedID = wkr.ID;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("hours", "blah");
            fakeform.Add("comments", "UnitTest");
            //
            // Mock service and setup SaveWorkAssignment mock
            _waServ.Setup(p => p.Save(asmt, "UnitTest"));
            _waServ.Setup(p => p.Get(testid)).Returns(asmt);
            _wkrServ.Setup(p => p.GetWorker((int)asmt.workerAssignedID)).Returns(wkr);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            //_ctrlr.ModelState.AddModelError("TestError", "foo");
            _ctrlr.Edit(testid, null, "UnitTest");
            //Assert
        }
        #endregion

        [TestMethod]
        public void WorkAssignmentController_delete_post_returns_json()
        {
            //Arrange
            int testid = 4242;
            FormCollection fakeform = new FormCollection();

            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();

            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as JsonResult;
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Data.ToString());
        }
    }
}
