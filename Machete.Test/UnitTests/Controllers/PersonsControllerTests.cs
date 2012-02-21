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
using System.Web.Routing;

namespace Machete.Test.Controllers
{
    /// <summary>
    /// Summary description for PersonControllerUnitTests
    /// </summary>

    [TestClass]
    public class PersonsControllerTests
    {
        Mock<IPersonService> _serv;
        PersonController _ctrlr;
        FormCollection fakeform;

        [TestInitialize]
        public void TestInitialize()
        {
            _serv = new Mock<IPersonService>();
            _ctrlr = new PersonController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
            fakeform.Add("firstname1", "Ronald");
            fakeform.Add("lastname1", "Reagan");
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void PersonController_index_get_returns_ActionResult()
        {
            //Arrange
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
        public void PersonController_create_get_returns_person()
        {
            //Arrange
            //Act
            var result = (PartialViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
        }

        [TestMethod]
        public void PersonController_create_post_valid_returns_JSON()
        {
            //Arrange
            var person = new Person();
            _serv.Setup(p => p.CreatePerson(person, "UnitTest")).Returns(person);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = (JsonResult)_ctrlr.Create(person, "UnitTest");
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual(12345, data["iNewID"]);
            Assert.AreEqual("Ronald Reagan", data["sNewLabel"]);
            Assert.AreEqual("/Person/Edit/12345", data["sNewRef"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void PersonController_create_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person();
            _serv.Setup(p => p.CreatePerson(person, "UnitTest")).Returns(person);
            fakeform.Remove("firstname1");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(person, "UnitTest");
            //Assert
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
            var result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
        }

        [TestMethod]
        public void PersonController_edit_post_valid_updates_model_returns_JSON()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");     //Every required field must be populated,
            fakeform.Add("lastname1", "UnitTest");  //or result will be null.
            fakeform.Add("gender", "47");
            Person fakeperson = new Person();
            Person savedperson = new Person();
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
            var result = _ctrlr.Edit(testid, "UnitTest") as JsonResult;
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(fakeperson, savedperson);
            Assert.AreEqual(savedperson.firstname1, "blah");
            Assert.AreEqual(savedperson.lastname1, "UnitTest");
            Assert.AreEqual(savedperson.gender, 47);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void PersonController_edit_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person();
            int testid = 12345;
            //
            // Mock service and setup SavePerson mock
            _serv.Setup(p => p.SavePerson(person, "UnitTest"));
            _serv.Setup(p => p.GetPerson(testid)).Returns(person);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            fakeform.Remove("firstname1");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            _ctrlr.Edit(testid, "UnitTest");
            //Assert
            //IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            //Assert.AreEqual("Controller UpdateModel failure on recordtype: person", data["status"]);
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
        public void PersonController_delete_post_returns_JSON()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new PersonController(_serv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            JsonResult result = _ctrlr.Delete(testid, "UnitTest") as JsonResult;
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(4242, data["deletedID"]);            
        }
    }
}
