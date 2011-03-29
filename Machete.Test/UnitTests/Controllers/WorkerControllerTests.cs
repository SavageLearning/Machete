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
    [TestClass]
    public class WorkerControllerTests
    {
        Mock<IWorkerService> _wserv;
        Mock<IPersonService> _pserv;
        Mock<IImageService> _iserv;
        //
        //   Testing /Index functionality
        //
        [TestMethod]
        public void WorkerController_index_get_returns_enumerable_list()
        {
            //Arrange
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(IEnumerable<Worker>));
        }

        [TestMethod]
        public void WorkerController_create_get_returns_person()
        {
            //Arrange
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            //Act
            var result = (ViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkerViewModel));
        }

        [TestMethod]
        public void WorkerController_create_post_valid_redirects_to_Index()
        {
            //Arrange
            var _worker = new Worker();
            var _person = new Person();
            var _viewmodel = new WorkerViewModel();
            _viewmodel.person = _person;
            _viewmodel.worker = _worker;
            //
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            _wserv.Setup(p => p.CreateWorker(_worker, "UnitTest")).Returns(_worker);
            _pserv.Setup(p => p.CreatePerson(_person, "UnitTest")).Returns(_person);
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            //Act
            var result = (RedirectToRouteResult)_ctrlr.Create(_viewmodel, "UnitTest");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void WorkerController_create_post_invalid_returns_view()
        {
            //Arrange
            var _worker = new Worker();
            var _person = new Person();
            var _viewmodel = new WorkerViewModel();
            _viewmodel.person = _person;
            _viewmodel.worker = _worker;
            //
            _pserv = new Mock<IPersonService>();
            _wserv = new Mock<IWorkerService>();
            _wserv.Setup(p => p.CreateWorker(_worker, "UnitTest")).Returns(_worker);
            _pserv.Setup(p => p.CreatePerson(_person, "UnitTest")).Returns(_person);
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            _ctrlr.ModelState.AddModelError("TestError", "foo");
            //Act
            var result = (ViewResult)_ctrlr.Create(_viewmodel, "UnitTest");
            //Assert
            var error = result.ViewData.ModelState["TestError"].Errors[0];
            Assert.AreEqual("foo", error.ErrorMessage);
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
            //
            _pserv = new Mock<IPersonService>();
            _wserv = new Mock<IWorkerService>();
            int testid = 4242;
            Person fakeperson = new Person();
            _wserv.Setup(p => p.GetWorker(testid)).Returns(_worker);
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            //Act
            ViewResult result = (ViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkerViewModel));
        }

        [TestMethod]
        public void WorkerController_edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            _pserv = new Mock<IPersonService>();
            _wserv = new Mock<IWorkerService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();

            Worker fakeworker = new Worker();
            Worker savedworker = new Worker();
            Person fakeperson = new Person();
            WorkerViewModel _viewmodel = new WorkerViewModel();
            _viewmodel.person = fakeperson;
            _viewmodel.worker = fakeworker;

            string user = "";
            _wserv.Setup(p => p.GetWorker(testid)).Returns(fakeworker);
            _pserv.Setup(p => p.GetPerson(testid)).Returns(fakeperson);
            _wserv.Setup(x => x.SaveWorker(It.IsAny<Worker>(),
                                          It.IsAny<string>())
                                         ).Callback((Worker p, string str) =>
                                         {
                                             savedworker = p;
                                             user = str;
                                         });
            _iserv = new Mock<IImageService>();
            var _ctrlr = new WorkerController(_wserv.Object, _pserv.Object, _iserv.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            //TODO Solve TryUpdateModel moq problem
            var result = _ctrlr.Edit(testid, _viewmodel, "UnitTest") as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworker, savedworker);
            Assert.AreEqual(savedworker.height, "too tall");
            Assert.AreEqual(savedworker.height, "too heavy");
        }

        private FormCollection _fakeCollection(int id)
        {
            FormCollection _fc = new FormCollection();
            _fc.Add("id", id.ToString());
            _fc.Add("person.firstname1", "blah_firstname");
            _fc.Add("person.firstname2", "");
            _fc.Add("person.lastname1", "unittest");
            _fc.Add("person.lastname2", "");
            _fc.Add("person.address1", "");
            _fc.Add("person.address2", "");
            _fc.Add("person.city", "");
            _fc.Add("person.state", "");
            _fc.Add("person.zipcode", "");
            _fc.Add("person.phone", "");
            _fc.Add("person.gender", "");
            _fc.Add("person.genderother", "");

            _fc.Add("worker.height", "too tall");
            _fc.Add("worker.weight", "too heavy");
            _fc.Add("worker.raceother", "multi-cultural");
            return _fc;
        }
        #endregion  
    }
}