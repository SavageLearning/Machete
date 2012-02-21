using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace Machete.Test
{
    [TestClass]
    public class WorkerServiceTest : ServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
        }
        /// <summary>
        /// Create a worker record from the Worker Service
        /// </summary>
        [TestMethod]
        public void Integration_Worker_Service_CreateWorker()
        {
            //
            //Arrange
            Worker _w = (Worker)Records.worker.Clone();
            Person _p = (Person)Records.person.Clone();
            _p.firstname2 = "WorkerService_Intergation_CreateWorker";
            _w.height = "WorkerService_Intergation_CreateWorker";
            _w.Person = _p;
            //
            //Act
            _wServ.CreateWorker(_w, "UnitTest");
            //
            //Assert
            Assert.IsNotNull(_w.ID, "Worker.ID is Null");
            Assert.IsTrue(_p.ID == 4, "Record did not have expected ID");
            Assert.IsTrue(_w.ID == _p.ID, "Worker.ID doesn't match Person.ID");
        }
        /// <summary>
        /// Create, Edit, and Save a worker record from the Worker Service
        /// </summary>
        [TestMethod]
        public void Integration_Worker_Service_EditWorker()
        {
            //
            //Arrange
            Worker _w = (Worker)Records.worker.Clone();
            Person _p = (Person)Records.person.Clone();
            _p.firstname2 = "WorkerService_Intergation_CreateWorker";
            _w.height = "tall";
            _w.Person = _p;
            //
            //Act
            Worker result = _wServ.CreateWorker(_w, "UnitTest");
            result.height = "short"; //EF should keep _w and result the same
            _wServ.SaveWorker(result, "UnitTest");
            //
            //Assert
            Assert.IsNotNull(_w.ID, "Worker.ID is Null");
            Assert.IsNotNull(result.ID, "(worker) result.ID is Null");
            Assert.IsTrue(_p.ID == 4, "Record did not have expected ID");
            Assert.IsTrue(result.Person.ID == 4, "Record did not have expected ID");
            Assert.IsTrue(_w.ID == _p.ID, "Worker.ID doesn't match Person.ID");
            Assert.IsTrue(_w.height == "short", "SaveWorker failed to save property change");
            Assert.IsTrue(result.height == "short", "SaveWorker failed to save property change");
            Assert.AreSame(_w, result, "CreateWorker did not return the expected object");
        }
    }
}
