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
using Machete.Web.Helpers;

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
        int active;
        int pending;
        int completed;
        int cancelled;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Database.SetInitializer<MacheteContext>(new TestInitializer());

        }


        [TestInitialize]
        public void TestInitialize()
        {
            MacheteDB = new MacheteContext();
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Records.Initialize(new MacheteContext());
            WorkerCache.Initialize(new MacheteContext());
            LookupCache.Initialize(new MacheteContext());
            Lookups.Initialize();
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
            active = LookupCache.getSingleEN("orderstatus", "Active");
            pending = LookupCache.getSingleEN("orderstatus", "Pending");
            completed = LookupCache.getSingleEN("orderstatus", "Completed");
            cancelled = LookupCache.getSingleEN("orderstatus", "Cancelled");
        }

        [TestMethod]
        public void Integration_WO_Service_GetSummary()
        {
            //
            //Arrange
            //
            //Act
            IEnumerable<WorkOrderSummary> result = woServ.GetSummary("").ToList();
            //
            //Assert
            Assert.IsNotNull(result, "GetSummary result is Null");
            Assert.IsTrue(result.Where(r => r.status == active).First().count == 2, "GetSummary returned incorrect number of Active records");
            Assert.IsTrue(result.Where(r => r.status == pending).First().count == 1, "GetSummary returned incorrect number of Pending records");
            Assert.IsTrue(result.Where(r => r.status == completed).First().count == 2, "GetSummary returned incorrect number of Completed records");
            Assert.IsTrue(result.Where(r => r.status == cancelled).First().count == 1, "GetSummary returned incorrect number of Cancelled records");
        }
        [TestMethod]
        public void Integration_WO_Service_CombinedSummary()
        {
            //
            // Arrange
            string search = "";
            bool orderdescending = true;
            int displayStart = 0;
            int displayLength = 50;
            //
            //Act
            ServiceIndexView<WOWASummary> result = woServ.CombinedSummary(search, orderdescending, displayStart, displayLength);
            WOWASummary wowa = result.query.First();
            //
            //Assert
            Assert.IsNotNull(result, "CombinedSummary.ServiceIndexView is Null");
            Assert.IsNotNull(wowa, "CombinedSummary.ServiceIndexView.query is null");
            Assert.AreEqual(4, wowa.active_wa, "CombinedSummary returned unexpected active_wa value");
            Assert.AreEqual(2, wowa.active_wo, "CombinedSummary returned unexpected active_wo value");
            Assert.AreEqual(1, wowa.cancelled_wa, "CombinedSummary returned unexpected cancelled_wa value");
            Assert.AreEqual(1, wowa.cancelled_wo, "CombinedSummary returned unexpected cancelled_wo value");
            Assert.AreEqual(3, wowa.completed_wa, "CombinedSummary returned unexpected completed_wa value");
            Assert.AreEqual(2, wowa.completed_wo, "CombinedSummary returned unexpected completed_wo value");
            Assert.AreEqual(0, wowa.expired_wa, "CombinedSummary returned unexpected expired_wa value");
            Assert.AreEqual(0, wowa.expired_wo, "CombinedSummary returned unexpected expired_wo value");
            Assert.AreEqual(2, wowa.pending_wa, "CombinedSummary returned unexpected pending_wa value");
            Assert.AreEqual(1, wowa.pending_wo, "CombinedSummary returned unexpected pending_wo value");

        }
        [TestMethod]
        public void Integration_WO_Service_get_GroupView()
        {
            //
            //Act
            var result = woServ.GetActiveOrders(DateTime.Now, false);
            Assert.IsNotNull(result, "Person.ID is Null");
        }
        [TestMethod]
        public void Integration_WO_Service_GetIndexView()
        {
            //
            //Arrange
            CultureInfo CI = new CultureInfo("en-US", false);
            string search = DateTime.Today.ToShortDateString();
            int? empID = null;
            int? status = null;
            bool descending = true;
            int displayStart = 0;
            int displayLength = 20;
            string sortColName = "WOID";
            //
            //Act
            ServiceIndexView<WorkOrder> result = woServ.GetIndexView(
                    CI,
                    search,
                    empID,
                    status,
                    descending,
                    displayStart, 
                    displayLength,
                    sortColName
                );

            //
            //Assert
            IEnumerable<WorkOrder> query = result.query.ToList();
            Assert.IsNotNull(result, "ServiceIndexView is Null");
            Assert.IsNotNull(query, "ServiceIndexView.query is null");
            Assert.IsTrue(query.Count() == 6, "Expected 6, but GetIndexView returned {0} records", query.Count());

        }
        [TestMethod]
        public void Integration_WO_Service_GetWorkOrders_returns_all()
        {
            // Arrange
            IEnumerable<WorkOrder> result = woServ.GetWorkOrders().ToList();
            //
            int count = MacheteDB.WorkOrders.Count();
            //
            Assert.IsTrue(result.Count() == 6, "Expected record count of 6, received {0}", result.Count());
            Assert.AreEqual(result.Count(), count, "GetWorkOrders() doesn't return all orders");            
        }
    }
}
