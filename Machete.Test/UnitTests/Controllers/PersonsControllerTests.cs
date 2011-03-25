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
    /// Summary description for UnitTest1
    /// </summary>

    [TestClass]
    public class PersonsControllerTests
    {
        Mock<IPersonService> _serv;

        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void PersonController_index_get_returns_enumerable_list()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            var _ctrlr = new PersonController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(IEnumerable<Person>));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod]
        public void PersonController_create_get_returns_person()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            var _ctrlr = new PersonController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
        }

        [TestMethod]
        public void PersonController_create_post_valid_redirects_to_Index()
        {
            //Arrange
            var person = new Person();
            _serv = new Mock<IPersonService>();
            _serv.Setup(p => p.CreatePerson(person, "UnitTest")).Returns(person);
            var _ctrlr = new PersonController(_serv.Object);
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(person, "UnitTest");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void PersonController_create_post_invalid_returns_view()
        {
            //Arrange
            var person = new Person();
            _serv = new Mock<IPersonService>();
            _serv.Setup(p => p.CreatePerson(person, "UnitTest")).Returns(person);
            var _ctrlr = new PersonController(_serv.Object);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (ViewResult)_ctrlr.Create(person, "UnitTest");
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
        public void PersonController_edit_get_returns_person()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            Person fakeperson = new Person();
            _serv.Setup(p => p.GetPerson(testid)).Returns(fakeperson);
            var _ctrlr = new PersonController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
        }

        [TestMethod]
        public void PersonController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");
            fakeform.Add("lastname1", "UnitTest");
            fakeform.Add("gender", "M");
            Person fakeperson = new Person();
            var savedperson = new Person();
            string user = "";
            _serv.Setup(p => p.GetPerson(testid)).Returns(fakeperson);
            _serv.Setup(x => x.SavePerson(It.IsAny<Person>(),
                                          It.IsAny<string>())
                                         ).Callback((Person p, string str) =>
                                                {
                                                    savedperson = p;
                                                    user = str;
                                                });
            var _ctrlr = new PersonController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeperson, savedperson);
            Assert.AreEqual(savedperson.firstname1, "blah");
            Assert.AreEqual(savedperson.lastname1, "UnitTest");
            Assert.AreEqual(savedperson.gender, "M");
        }

        [TestMethod]
        public void PersonController_edit_post_invalid_returns_view()
        {
            //Arrange
            var person = new Person();
            int testid = 4243;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");
            fakeform.Add("lastname1", "UnitTest");
            fakeform.Add("gender", "M");
            //
            // Mock service and setup SavePerson mock
            _serv = new Mock<IPersonService>();
            _serv.Setup(p => p.SavePerson(person, "UnitTest"));
            _serv.Setup(p => p.GetPerson(testid)).Returns(person);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            var _ctrlr = new PersonController(_serv.Object);
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
        public void PersonController_delete_get_returns_person()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            Person fakeperson = new Person();
            _serv.Setup(p => p.GetPerson(testid)).Returns(fakeperson);
            var _ctrlr = new PersonController(_serv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Delete(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
        }

        [TestMethod]
        public void PersonController_delete_post_redirects_to_index()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new PersonController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, fakeform, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
