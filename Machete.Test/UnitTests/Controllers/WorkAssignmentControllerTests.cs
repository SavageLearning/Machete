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
        [TestInitialize]
        public void TestInitialize()
        {
            _waServ = new Mock<IWorkAssignmentService>();
            _wkrServ = new Mock<IWorkerService>();
            _woServ = new Mock<IWorkOrderService>();
            _wsiServ = new Mock<IWorkerSigninService>();
            _ctrlr = new WorkAssignmentController(_waServ.Object, _wkrServ.Object, _woServ.Object, _wsiServ.Object);
            _view = new WorkAssignmentIndex();
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
            WorkAssignment _newWA = new WorkAssignment();
            WorkAssignment _editor = new WorkAssignment();
            _newWA.ID = 11;
            _editor.Updatedby = "derp";
            WorkOrder _wo = new WorkOrder();
            _newWA.workOrder = _wo;
            _wo.paperOrderNum = 12345;
            _wo.ID = 123;
            int _num = 0;
            string username = "UnitTest";
            _waServ.Setup(p => p.Create(_editor, username)).Returns(() => _newWA);
            _woServ.Setup(p => p.GetWorkOrder(_num)).Returns(() => _wo);
            //Act
            JsonResult result = (JsonResult)_ctrlr.Create(_editor, username);
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual(result.Data.ToString(), "{ sNewRef = /WorkAssignment/Edit/11, sNewLabel = Assignment #: 12345-00011, iNewID = 11 }");
        }

        [TestMethod]
        public void WorkAssignmentController_create_post_invalid_returns_view()
        {
            //Arrange
            var _editor = new WorkAssignment();
            _waServ.Setup(p => p.Create(_editor, "UnitTest")).Returns(_editor);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (PartialViewResult)_ctrlr.Create(_editor, "UnitTest");
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
        public void WorkAssignmentController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("description", "blah");     //Every required field must be populated,
            fakeform.Add("comments", "UnitTest");  //or result will be null.            
            WorkAssignment fakeworkAssignment = new WorkAssignment();
            WorkAssignment savedworkAssignment = new WorkAssignment();
            string user = "";
            _waServ.Setup(p => p.Get(testid)).Returns(fakeworkAssignment);
            _waServ.Setup(x => x.Save(It.IsAny<WorkAssignment>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkAssignment p, string str) =>
                                         {
                                             savedworkAssignment = p;
                                             user = str;
                                         });
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, null,  fakeform, "UnitTest") as PartialViewResult;
            //Assert
            //Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkAssignment, savedworkAssignment);
            Assert.AreEqual(savedworkAssignment.description, "blah");
            Assert.AreEqual(savedworkAssignment.comments, "UnitTest");
        }

        [TestMethod]
        public void WorkAssignmentController_edit_post_invalid_returns_view()
        {
            //Arrange
            var workAssignment = new WorkAssignment();
            int testid = 4243;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("description", "blah");
            fakeform.Add("comments", "UnitTest");
            //
            // Mock service and setup SaveWorkAssignment mock
            _waServ.Setup(p => p.Save(workAssignment, "UnitTest"));
            _waServ.Setup(p => p.Get(testid)).Returns(workAssignment);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            var result = (PartialViewResult)_ctrlr.Edit(testid, null, fakeform, "UnitTest");
            //Assert
            var error = result.ViewData.ModelState["TestError"].Errors[0];
            Assert.AreEqual("foo", error.ErrorMessage);
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
            Assert.AreEqual(result.Data.ToString(), "{ status = OK, deletedID = 4242 }");
        }
    }
}
