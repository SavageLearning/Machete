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
        EmployerController _ctrlr;
        FormCollection fakeform;

        [TestInitialize]
        public void TestInitialize()
        {
            _serv = new Mock<IEmployerService>();
            _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
            fakeform.Add("name", "blah");
            fakeform.Add("address1", "UnitTest");
            fakeform.Add("city", "footown");
            fakeform.Add("state", "WA");
            fakeform.Add("phone", "123-456-7890");
            fakeform.Add("zipcode", "1234567890");
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void EmployerController_index_get_returns_enumerable_list()
        {
            //Arrange
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void EmployerController_create_get_returns_employer()
        {
            //Arrange
            //Act
            var result = (PartialViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod]
        public void EmployerController_create_valid_post_returns_json()
        {
            //Arrange
            var employer = new Employer();
            employer.ID = 4242;
            employer.name = "unit test";
            _serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = (JsonResult)_ctrlr.Create(employer, "UnitTest");
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ sNewRef = /Employer/Edit/12345, sNewLabel = blah, iNewID = 12345, jobSuccess = True }", 
                            result.Data.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void EmployerController_create_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer();
            fakeform.Remove("name");

            _serv = new Mock<IEmployerService>();
            _serv.Setup(p => p.Create(employer, "UnitTest")).Returns(employer);
            var _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            var result = _ctrlr.Create(employer, "UnitTest") as JsonResult;
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
            _serv.Setup(p => p.Get(testid)).Returns(fakeemployer);
            var _ctrlr = new EmployerController(_serv.Object);
            //Act
            var result = _ctrlr.Edit(testid) as PartialViewResult;
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Employer));
        }

        [TestMethod]
        public void EmployerController_edit_post_valid_updates_model_returns_json()
        {
            //Arrange
            int testid = 4242;
            Employer fakeemployer = new Employer();
            Employer savedemployer = new Employer();
            string user = "";
            _serv.Setup(p => p.Get(testid)).Returns(fakeemployer);
            _serv.Setup(x => x.Save(It.IsAny<Employer>(),
                                          It.IsAny<string>())
                                         ).Callback((Employer p, string str) =>
                                         {
                                             savedemployer = p;
                                             user = str;
                                         });
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest") as JsonResult;
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ jobSuccess = True }", result.Data.ToString());
            Assert.AreEqual(fakeemployer, savedemployer);
            Assert.AreEqual(savedemployer.name, "blah");
            Assert.AreEqual(savedemployer.address1, "UnitTest");
            Assert.AreEqual(savedemployer.city, "footown");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void EmployerController_edit_post_invalid_throws_exception()
        {
            //Arrange
            var employer = new Employer();
            int testid = 4243;
            //
            // Mock service and setup SaveEmployer mock
            _serv.Setup(p => p.Save(employer, "UnitTest"));
            _serv.Setup(p => p.Get(testid)).Returns(employer);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            fakeform.Remove("phone");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            //_ctrlr.ModelState.AddModelError("TestError", "foo");
            _ctrlr.Edit(testid, fakeform, "UnitTest");
            //Assert
        }
        #endregion

        //
        // Testing /Delete functionality
        [TestMethod]
        public void EmployerController_delete_post_returns_json()
        {
            //Arrange
            _serv = new Mock<IEmployerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new EmployerController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, "UnitTest") as JsonResult;
            //Assert
            Assert.AreEqual("{ status = OK, jobSuccess = True, deletedID = 4242 }", 
                            result.Data.ToString());
        }
    }
}
