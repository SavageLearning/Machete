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

namespace Machete.Test.Controllers
{
    /// <summary>
    /// Summary description for EmployerControllerUnitTests
    /// </summary>

    [TestClass]
    public class EmployersControllerTests
    {
        Mock<IEmployerService> _serv;

        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void EmployerController_index_get_returns_enumerable_list()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(IEnumerable<Employer>));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void EmployerController_create_get_returns_employer()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod]
        public void EmployerController_create_post_valid_redirects_to_Index()
        {
            //Arrange
            var employer = new Employer();
            _serv = new Mock<IEmployerService>();
            _serv.Setup(p => p.CreateEmployer(employer, "UnitTest")).Returns(employer);
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(employer, "UnitTest");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EmployerController_create_post_invalid_returns_view()
        {
            //Arrange
            var employer = new Employer();
            _serv = new Mock<IEmployerService>();
            _serv.Setup(p => p.CreateEmployer(employer, "UnitTest")).Returns(employer);
            var _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (ViewResult)_ctrlr.Create(employer, "UnitTest");
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
        public void EmployerController_edit_get_returns_employer()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            int testid = 4242;
            Employer fakeemployer = new Employer();
            _serv.Setup(p => p.GetEmployer(testid)).Returns(fakeemployer);
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod]
        public void EmployerController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("name", "blah");
            fakeform.Add("address1", "UnitTest");
            fakeform.Add("city", "footown");
            fakeform.Add("state", "WA");
            fakeform.Add("phone", "1234567890");
            fakeform.Add("zipcode", "1234567890");
            Employer fakeemployer = new Employer();
            var savedemployer = new Employer();
            string user = "";
            _serv.Setup(p => p.GetEmployer(testid)).Returns(fakeemployer);
            _serv.Setup(x => x.SaveEmployer(It.IsAny<Employer>(),
                                          It.IsAny<string>())
                                         ).Callback((Employer p, string str) =>
                                         {
                                             savedemployer = p;
                                             user = str;
                                         });
            var _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeemployer, savedemployer);
            Assert.AreEqual(savedemployer.name, "blah");
            Assert.AreEqual(savedemployer.address1, "UnitTest");
            Assert.AreEqual(savedemployer.city, "footown");
        }

        [TestMethod]
        public void EmployerController_edit_post_invalid_returns_view()
        {
            //Arrange
            var employer = new Employer();
            int testid = 4243;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");
            fakeform.Add("lastname1", "UnitTest");
            fakeform.Add("gender", "M");
            //
            // Mock service and setup SaveEmployer mock
            _serv = new Mock<IEmployerService>();
            _serv.Setup(p => p.SaveEmployer(employer, "UnitTest"));
            _serv.Setup(p => p.GetEmployer(testid)).Returns(employer);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            var _ctrlr = new EmployerController(_serv.Object);
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
        public void EmployerController_delete_get_returns_employer()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            int testid = 4242;
            Employer fakeemployer = new Employer();
            _serv.Setup(p => p.GetEmployer(testid)).Returns(fakeemployer);
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Delete(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod]
        public void EmployerController_delete_post_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
