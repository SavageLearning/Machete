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
using System.Data.Entity.Database;

namespace Machete.Test
{
    [TestClass]
    public class WorkerServiceTest
    {
        WorkerRepository _workerRepo;
        DatabaseFactory _dbFactory;
        WorkerService _service;
        IUnitOfWork _unitofwork;
        MacheteContext MacheteDB;
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
        }


        [TestInitialize]
        public void TestInitialize()
        {
            DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
            this.MacheteDB = new MacheteContext();
            _dbFactory = new DatabaseFactory();
            _workerRepo = new WorkerRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _service = new WorkerService(_workerRepo, _unitofwork);
        }

        [TestMethod]
        public void WorkerService_Intergation_CreateWorker()
        {
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Worker _worker3 = Records._worker3;
            Person _person3 = Records._person3;
            _person3.firstname2 = "WorkerService_Intergation_CreateWorker";
            _worker3.height = "WorkerService_Intergation_CreateWorker";
            _worker3.Person = _person3;
            //
            //Act
            _service.CreateWorker(_worker3);

            //
            //Assert
            Assert.IsNotNull(_worker3.ID, "Worker.ID is Null");
            //Assert.IsTrue(Records._person3.ID == 1, "Record did not have expected ID");
            Assert.IsTrue(_worker3.ID == _person3.ID, "Worker.ID doesn't match Person.ID");
        }
        [TestMethod]
        [ExpectedException(typeof(MissingReferenceException))]
        public void WorkerService_Intergation_Detect_Missing_Person_Reference()
        {
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person2 = Records._person2;
            Worker _worker2 = Records._worker2;
            _person2.Worker = _worker2;
            _person2.firstname2 = "WorkerService_Intergation_Detect_Missing_Person_Reference";
            _worker2.height = "WorkerService_Intergation_Detect_Missing_Person_Reference";
            //
            //Act
            _service.CreateWorker(_worker2);
            //
            //Assert
        }
        [TestMethod]
        public void WorkerService_Intergation_CreateWorkers_NoDuplicate()
        {
            int reccount = 0;
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Worker _worker3 = Records._worker3;
            Person _person3 = Records._person3;
            _person3.firstname2 = "WorkerService_Intergation_CreateWorkers_NoDuplicate";
            _worker3.height = "WorkerService_Intergation_CreateWorkers_NoDuplicate";
 
            if (_worker3.Person  == null) 
                { _worker3.Person = _person3;}
            //
            //Act          
            _service.CreateWorker(_worker3);
            _service.CreateWorker(_worker3);
            _service.CreateWorker(_worker3);
            reccount = MacheteDB.Workers.Count(n => n.raceother == _worker3.raceother);
            //
            //Assert
            //TODO: figure out why de-dup isn't working
            Assert.IsNotNull(_worker3.ID);
            Assert.IsTrue(reccount == 1);
        }
    }
}
