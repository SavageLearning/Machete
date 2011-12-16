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
using System.Data.Entity.Validation;
using System.Globalization;

namespace Machete.Test
{
    [TestClass]
    public class WorkOrderServiceTest
    {
        DatabaseFactory dbFactory;
        WorkOrderRepository woRepo;
        IWorkerRepository wRepo;
        ILookupRepository lRepo;
        WorkOrderService woServ;
        WorkAssignmentService waServ;
        IUnitOfWork uow;
        MacheteContext MacheteDB;
        WorkAssignmentRepository waRepo;
        WorkerSigninRepository wsiRepo;
        WorkerRequestRepository wrRepo;
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Database.SetInitializer<MacheteContext>(new TestInitializer());
        }


        [TestInitialize]
        public void TestInitialize()
        {
            //Database.SetInitializer<MacheteContext>(new MacheteInitializer());
            MacheteDB = new MacheteContext();
            dbFactory = new DatabaseFactory();
            woRepo = new WorkOrderRepository(dbFactory);
            uow = new UnitOfWork(dbFactory);
            wRepo = new WorkerRepository(dbFactory);
            waRepo = new WorkAssignmentRepository(dbFactory);
            wsiRepo = new WorkerSigninRepository(dbFactory);
            wrRepo = new WorkerRequestRepository(dbFactory);
            lRepo = new LookupRepository(dbFactory);
            waServ = new WorkAssignmentService(waRepo, wRepo, lRepo, wsiRepo, wrRepo, uow);
            woServ = new WorkOrderService(woRepo, waServ, uow);
        }

        [TestMethod]
        public void DbSetWorkOrderService_Intergation_GetSummary()
        {
            //
            //Arrange
            //
            //Act
            var result = woServ.GetSummary("");
            //
            //Assert
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod]
        public void DbSetWorkOrderService_Intergation_CombinedSummary()
        {
            //
            //Act
            var result = woServ.CombinedSummary("", true, 0,0);
            //
            //Assert
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod]
        public void DbSetWorkOrderService_Intergation_get_GroupView()
        {
            //
            //Act
            var result = woServ.GetActiveOrders(DateTime.Now, false);
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod]
        public void DbSet_WorkOrderService_Intergation_GetIndexView()
        {
            //
            //Arrange
            CultureInfo CI = new CultureInfo("en-US", false);
            //
            //Act
            var result = woServ.GetIndexView(
                    CI,
                    "7/2011",   //search str
                    null, //employerID
                    null, //status 
                    true, //desc(true), asc(false)
                    0, 20, "WOID"
                );

            //
            //Assert
            var foo = result.query.ToList();
            Assert.IsNotNull(result, "Person.ID is Null");
            //Assert.IsTrue(person.ID == 1);
        }
    }
}
