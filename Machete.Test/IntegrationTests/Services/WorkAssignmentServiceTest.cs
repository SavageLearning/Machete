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
using System.Data.Entity.Validation;
using System.Globalization;

namespace Machete.Test
{
    [TestClass]
    public class WorkAssignmentServiceTest
    {
        WorkAssignmentRepository waRepo;
        WorkOrderRepository woRepo;
        WorkerRepository wRepo;
        WorkerSigninRepository wsiRepo;
        PersonRepository pRepo;
        ImageRepository iRepo;
        DatabaseFactory dbFactory;
        WorkAssignmentService waServ;
        WorkOrderService woServ;
        WorkerService wServ;
        PersonService pServ;
        ImageService iServ;
        WorkerSigninService wsiServ;
        IUnitOfWork unitofwork;
        ILookupRepository lRepo;
        WorkerRequestRepository wrRepo;
        MacheteContext MacheteDB;
        CultureInfo CI;
        DispatchOptions dOptions;
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DbDatabase.SetInitializer<MacheteContext>(new TestInitializer());
        }


        [TestInitialize]
        public void TestInitialize()
        {
            //DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
            MacheteDB = new MacheteContext();
            dbFactory = new DatabaseFactory();
            waRepo = new WorkAssignmentRepository(dbFactory);
            woRepo = new WorkOrderRepository(dbFactory);
            wsiRepo = new WorkerSigninRepository(dbFactory);
            unitofwork = new UnitOfWork(dbFactory);
            wRepo = new WorkerRepository(dbFactory);
            lRepo = new LookupRepository(dbFactory);
            wrRepo = new WorkerRequestRepository(dbFactory);
            waServ = new WorkAssignmentService(waRepo, wRepo, lRepo, wsiRepo, wrRepo, unitofwork);
            woServ = new WorkOrderService(woRepo, waServ, unitofwork);
            wsiServ = new WorkerSigninService(wsiRepo, wRepo, pRepo, iRepo, unitofwork);
            dOptions = new DispatchOptions
            {
                CI = new CultureInfo("en-US", false),
                search = "",
                date = DateTime.Parse("8/10/2011"),
                dwccardnum = null,
                woid = null,
                orderDescending = false,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
            
            LookupCache.Initialize(new MacheteContext());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_basic()
        {       
            //
            //Act
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(result, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(result.filteredCount, 10);
            Assert.AreEqual(result.totalCount, 10);            
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_checkwoidfilter()
        {
            //Act
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(3, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_paperordernum()
        {            
            //
            //Act
            dOptions.search = "12420";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(12420, tolist[0].workOrder.paperOrderNum);
            Assert.AreEqual(3, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_description()
        {
            //
            //Act
            dOptions.search = "foostring1";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual("foostring1", tolist[0].description);
            Assert.AreEqual(1, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_Updatedby()
        {
            dOptions.search = "foostring1";
            dOptions.woid = 1;
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual("foostring2", tolist[0].Updatedby);
            Assert.AreEqual(1, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_search_skill()
        {
            dOptions.search = "Digging";
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(70, tolist[0].skillID);
            Assert.AreEqual(1, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_searchdateTimeofWork()
        {
            //
            //Act
            dOptions.search = "8/10/2011 9:00";
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(70, tolist[0].skillID);
            Assert.AreEqual(3, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_searchdwccardnum()
        {
            //
            //Act
            dOptions.dwccardnum = 30040;
            dOptions.orderDescending = true;
            var result = waServ.GetIndexView(dOptions);
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(10, result.filteredCount);
            Assert.AreEqual(10, result.totalCount);
        }
        [TestMethod]
        public void Integration_WA_Service_Assign_updates_WSI_and_WA()
        {
            WorkerSignin wsi1 = wsiServ.GetWorkerSignin(1);
            WorkAssignment wa1 = waServ.Get(1);
            var result = waServ.Assign(wa1, wsi1);
            WorkerSignin wsi2 = wsiServ.GetWorkerSignin(1);
            WorkAssignment wa2 = waServ.Get(1);
            Assert.IsNotNull(result);
            Assert.IsNotNull(wa2.workerAssignedID);
            Assert.IsNotNull(wa2.workerSigninID);
            Assert.IsNotNull(wsi2.WorkAssignmentID);
            Assert.IsNotNull(wsi2.WorkerID);
        }
        [TestMethod]
        public void Integration_WA_Service_GetSummary()
        {
            var result = waServ.GetSummary("");
            Assert.IsNotNull(result, "Person.ID is Null");
        }

        [TestMethod]
        public void Integration_WA_Service_GetSummary2()
        {
            //
            //Arrange

            //
            //Act
            var woresults = woServ.GetSummary("8/10/2011");
            var waresults = waServ.GetSummary("8/10/2011");

            var joined = woServ.GetSummary("8/10/2011").Join(waServ.GetSummary("8/10/2011"),
                                        wo => new {wo.date, wo.status},
                                        wa => new {wa.date, wa.status},
                                        (wo, wa) => new
                                        {
                                            wo.date,
                                            wo.status,
                                            wo_count = wo.count,
                                            wa_count = wa.count
                                        }).OrderBy(a => a.date)
                        .GroupBy(gb => gb.date)
                        .Select(g => new { 
                            date = g.Key,
                            pending_wo = g.Where(c => c.status == 43).Sum(d => d.wo_count),
                            pending_wa = g.Where(c => c.status == 43).Sum(d => d.wa_count),
                            active_wo = g.Where(c => c.status == 42).Sum(d => d.wo_count),
                            active_wa = g.Where(c => c.status == 42).Sum(d => d.wa_count),
                            completed_wo = g.Where(c => c.status == 44).Sum(d => d.wo_count),
                            completed_wa = g.Where(c => c.status == 44).Sum(d => d.wa_count),
                            cancelled_wo = g.Where(c => c.status == 45).Sum(d => d.wo_count),
                            cancelled_wa = g.Where(c => c.status == 45).Sum(d => d.wa_count),
                            expired_wo = g.Where(c => c.status == 46).Sum(d => d.wo_count),
                            expired_wa = g.Where(c => c.status == 46).Sum(d => d.wa_count)
                        });


            //
            //Assert
            Assert.IsNotNull(joined, "Person.ID is Null");
            //Assert.IsTrue(_person.ID == 1);
        }
    }
}