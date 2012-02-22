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
using System.Web;

namespace Machete.Test.Controllers
{
    [TestClass]
    public class WorkerControllerTests
    {
        Mock<IWorkerService> _wserv;
        Mock<IPersonService> _pserv;
        Mock<IImageService> _iserv;
        WorkerController _ctrlr;
        FormCollection fakeform;
        [TestInitialize]
        public void TestInitialize()
        {
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            _iserv = new Mock<IImageService>();
            _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
            fakeform.Add("typeOfWorkID", "1");
            fakeform.Add("RaceID", "1");
            fakeform.Add("height", "1");
            fakeform.Add("weight", "1");
            fakeform.Add("englishlevelID", "1");
            fakeform.Add("recentarrival", "true");
            fakeform.Add("dateinUSA", "1/1/2000");
            fakeform.Add("dateinseattle", "1/1/2000");
            fakeform.Add("disabled", "true");
            fakeform.Add("maritalstatus", "1");
            fakeform.Add("livewithchildren", "true");
            fakeform.Add("numofchildren", "1");
            fakeform.Add("incomeID", "1");
            fakeform.Add("livealone", "true");
            fakeform.Add("emcontUSAname", "");
            fakeform.Add("emcontUSAphone", "");
            fakeform.Add("emcontUSArelation", "");
            fakeform.Add("dwccardnum", "12345");
            fakeform.Add("neighborhoodID", "1");
            fakeform.Add("immigrantrefugee", "false");
            fakeform.Add("countryoforiginID", "1");
            fakeform.Add("emcontoriginname", "");
            fakeform.Add("emcontoriginphone", "");
            fakeform.Add("emcontoriginrelation", "");
            fakeform.Add("memberexpirationdate", "1/1/2000");
            fakeform.Add("driverslicense", "false");
            fakeform.Add("licenseexpirationdate", "");
            fakeform.Add("carinsurance", "false");
            fakeform.Add("insuranceexpiration", "");
            fakeform.Add("dateOfBirth", "1/1/2000");
            fakeform.Add("dateOfMembership", "1/1/2000");
            //fakeform.Add("", "");
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkerController_index_get_WorkIndexViewModel()
        {
            //Arrange           
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void WorkerController_create_get_returns_person()
        {
            //Arrange
            //Act
            var result = (PartialViewResult)_ctrlr.Create(0);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Worker));
        }

        [TestMethod]
        public void WorkerController_create_post_valid_returns_json()
        {
            //Arrange
            var _worker = new Worker();
            var _person = new Person();
            var _viewmodel = new WorkerViewModel();
            _viewmodel.person = _person;
            _viewmodel.worker = _worker;
            //
            _wserv.Setup(p => p.CreateWorker(_worker, "UnitTest")).Returns(_worker);
            _pserv.Setup(p => p.CreatePerson(_person, "UnitTest")).Returns(_person);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Create(_worker, "UnitTest", null) as JsonResult;
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual("{ iNewID = 12345, jobSuccess = True }",
                            result.Data.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void WorkerController_create_post_invalid_throws_exception()
        {
            //Arrange
            var _worker = new Worker();

            fakeform.Remove("height");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(_worker, "UnitTest", null);
            //Assert
        }

        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod]
        public void WorkerController_edit_get_returns_worker()
        {
            //Arrange
            var _worker = new Worker();
            var _person = new Person();
            var _viewmodel = new WorkerViewModel();
            _viewmodel.person = _person;
            _viewmodel.worker = _worker;
            int testid = 4242;
            Person fakeperson = new Person();
            _wserv.Setup(p => p.GetWorker(testid)).Returns(_worker);
            //Act
            var result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Worker));
        }

        [TestMethod]
        public void WorkerController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange

            int testid = 4242;
            Mock<HttpPostedFileBase> image = new Mock<HttpPostedFileBase>();
            FormCollection fakeform = _fakeCollection(testid);

            Worker fakeworker = new Worker();
            Worker savedworker = new Worker();
            Person fakeperson = new Person();
            fakeworker.Person = fakeperson;
            WorkerViewModel _viewmodel = new WorkerViewModel();
            _viewmodel.person = fakeperson;
            _viewmodel.worker = fakeworker;

            string user = "TestUser";
            _wserv.Setup(p => p.GetWorker(testid)).Returns(fakeworker);
            _pserv.Setup(p => p.GetPerson(testid)).Returns(fakeperson);
            _wserv.Setup(x => x.SaveWorker(It.IsAny<Worker>(),
                                          It.IsAny<string>())
                                         ).Callback((Worker p, string str) =>
                                         {
                                             savedworker = p;
                                             user = str;
                                         });

            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            //TODO Solve TryUpdateModel moq problem
            var result = _ctrlr.Edit(testid, fakeworker, "UnitTest", null) as PartialViewResult;
            //Assert
            //Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworker, savedworker);
            Assert.AreEqual(savedworker.height, "UnitTest");
            Assert.AreEqual(savedworker.height, "UnitTest");
        }

        private FormCollection _fakeCollection(int id)
        {
            FormCollection _fc = new FormCollection();
            _fc.Add("ID", id.ToString());
            _fc.Add("firstname1", "blah_firstname");
            //_fc.Add("person.firstname2", "");
            _fc.Add("lastname1", "unittest");
            //_fc.Add("person.lastname2", "");
            //_fc.Add("person.address1", "");
            //_fc.Add("person.address2", "");
            //_fc.Add("person.city", "");
            //_fc.Add("person.state", "");
            //_fc.Add("person.zipcode", "");
            //_fc.Add("person.phone", "");
            _fc.Add("gender", "M");
            _fc.Add("typeOfWorkID", "1");          
            _fc.Add("RaceID", "1");     //Every required field must be populated,
            _fc.Add("height", "UnitTest");  //or result will be null.
            _fc.Add("weight", "UnitTest");
            _fc.Add("englishlevelID", "1");
            _fc.Add("dateinUSA", "1/1/2001");
            _fc.Add("dateinseattle", "1/1/2001");
            _fc.Add("dateOfBirth", "1/1/2001");
            _fc.Add("dateOfMembership", "1/1/2001");
            _fc.Add("maritalstatus", "1");
            _fc.Add("numofchildren", "1");
            _fc.Add("incomeID", "1");
            _fc.Add("dwccardnum", "12345");
            _fc.Add("neighborhoodID", "1");
            _fc.Add("countryoforigin", "1");
            _fc.Add("memberexpirationdate", "1/1/2002");
            return _fc;
        }
        #endregion  
    }
}