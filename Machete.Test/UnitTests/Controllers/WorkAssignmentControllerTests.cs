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
        Mock<IWorkAssignmentService> _serv;
        Mock<IWorkerService> _wkrServ;
        Mock<IWorkOrderService> _woServ;
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkAssignmentController_index_get_returns_WorkAssignmentIndexViewModel()
        {
            //Arrange
            _serv = new Mock<IWorkAssignmentService>();
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            var _view = new WorkAssignmentIndex();
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignmentIndex));
        }
        //TODO: test filter unit stuff
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void WorkAssignmentController_create_get_returns_workAssignment()
        {
            //Arrange
            _serv = new Mock<IWorkAssignmentService>();
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Create(0);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignment));
        }

        [TestMethod]
        public void WorkAssignmentController_create_post_valid_redirects_to_Index()
        {
            //Arrange
            var _editor = new WorkAssignment();
            _serv = new Mock<IWorkAssignmentService>();
            _serv.Setup(p => p.Create(_editor, "UnitTest")).Returns(_editor);
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(_editor, "UnitTest");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void WorkAssignmentController_create_post_invalid_returns_view()
        {
            //Arrange
            var _editor = new WorkAssignment();
            _serv = new Mock<IWorkAssignmentService>();
            _serv.Setup(p => p.Create(_editor, "UnitTest")).Returns(_editor);
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (ViewResult)_ctrlr.Create(_editor, "UnitTest");
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
            _serv = new Mock<IWorkAssignmentService>();
            int testid = 4242;
            WorkAssignment fakeworkAssignment = new WorkAssignment();
            _serv.Setup(p => p.Get(testid)).Returns(fakeworkAssignment);
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignment));
        }

        [TestMethod]
        public void WorkAssignmentController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IWorkAssignmentService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("description", "blah");     //Every required field must be populated,
            fakeform.Add("comments", "UnitTest");  //or result will be null.            
            WorkAssignment fakeworkAssignment = new WorkAssignment();
            WorkAssignment savedworkAssignment = new WorkAssignment();
            string user = "";
            _serv.Setup(p => p.Get(testid)).Returns(fakeworkAssignment);
            _serv.Setup(x => x.Save(It.IsAny<WorkAssignment>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkAssignment p, string str) =>
                                         {
                                             savedworkAssignment = p;
                                             user = str;
                                         });
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
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
            _serv = new Mock<IWorkAssignmentService>();
            _serv.Setup(p => p.Save(workAssignment, "UnitTest"));
            _serv.Setup(p => p.Get(testid)).Returns(workAssignment);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
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
        public void WorkAssignmentController_delete_get_returns_workAssignment()
        {
            //Arrange
            _serv = new Mock<IWorkAssignmentService>();
            int testid = 4242;
            WorkAssignment fakeworkAssignment = new WorkAssignment();
            _serv.Setup(p => p.Get(testid)).Returns(fakeworkAssignment);
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            //Act
            var result = (ViewResult)_ctrlr.Delete(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkAssignment));
        }

        [TestMethod]
        public void WorkAssignmentController_delete_post_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IWorkAssignmentService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new WorkAssignmentController(_serv.Object, _wkrServ.Object, _woServ.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
