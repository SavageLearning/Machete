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
        WorkAssignmentRepository _waRepo;
        WorkOrderRepository _woRepo;
        WorkerRepository _wRepo;
        WorkerSigninRepository _wsiRepo;
        PersonRepository _pRepo;
        ImageRepository _iRepo;
        DatabaseFactory _dbFactory;
        WorkAssignmentService _waServ;
        WorkOrderService _woServ;
        WorkerService _wServ;
        PersonService _pServ;
        ImageService _iServ;
        WorkerSigninService _wsiServ;
        IUnitOfWork _unitofwork;
        ILookupRepository _lRepo;
        MacheteContext MacheteDB;
        CultureInfo CI;

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
            _dbFactory = new DatabaseFactory();
            _waRepo = new WorkAssignmentRepository(_dbFactory);
            _woRepo = new WorkOrderRepository(_dbFactory);
            _wsiRepo = new WorkerSigninRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _wRepo = new WorkerRepository(_dbFactory);
            _lRepo = new LookupRepository(_dbFactory);
            _waServ = new WorkAssignmentService(_waRepo, _wRepo, _lRepo, _wsiRepo, _unitofwork);
            _woServ = new WorkOrderService(_woRepo, _waServ, _unitofwork);
            _wsiServ = new WorkerSigninService(_wsiRepo, _wRepo, _pRepo, _iRepo, _unitofwork);
            CI = new CultureInfo("en-US", false);
            LookupCache.Initialize(new MacheteContext());
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_basic()
        {       
            //
            //Act
            var result = _waServ.GetIndexView(CI,""/*search str*/,DateTime.Parse("8/10/2011"),null/*dwccardnum*/,null/*woid */,true, /*desc(true), asc(false)*/0, 20, "WOID");
            //
            //Assert
            var tolist = result.query.ToList();
            Assert.IsNotNull(result, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkAssignment>));
            Assert.AreEqual(result.filteredCount, 10);
            Assert.AreEqual(result.totalCount, 10);            
        }
        [TestMethod]
        public void Integration_WA_Service_GetIndexView_check_woidfilter()
        {
            //Act
            var result = _waServ.GetIndexView(CI, ""/*search str*/, DateTime.Parse("8/10/2011"), null/*dwccardnum*/, 1/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
            var result = _waServ.GetIndexView(CI, "12420"/*search str*/, DateTime.Parse("8/10/2011"), null/*dwccardnum*/, 1/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
            var result = _waServ.GetIndexView(CI, "foostring1"/*search str*/, DateTime.Parse("8/10/2011"), null/*dwccardnum*/, 1/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
            //
            //Act
            var result = _waServ.GetIndexView(CI, "foostring1"/*search str*/, DateTime.Parse("8/10/2011"), null/*dwccardnum*/, 1/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
            //
            //Act
            var result = _waServ.GetIndexView(CI, "Digging"/*search str*/, DateTime.Parse("8/10/2011"), null/*dwccardnum*/, null/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
        public void Integration_WA_Service_GetIndexView_check_search_dateTimeofWork()
        {
            //
            //Act
            var result = _waServ.GetIndexView(CI, "8/10/2011 9:00"/*search str*/, null, null/*dwccardnum*/, null/*woid */, true, /*desc(true), asc(false)*/0, 20, "WOID");
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
        public void Integration_WA_Service_GetIndexView_check_search_dwccardnum()
        {
            //
            //Act
            var result = _waServ.GetIndexView(CI, 
                                            ""/*search str*/, 
                                            null,
                                            30040/*dwccardnum*/, 
                                            null/*woid */, 
                                            true, /*desc(true), asc(false)*/
                                            0, 
                                            20, 
                                            "WOID");
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
            var result = _waServ.Assign(1, 1);
            WorkerSignin wsi2 = _wsiServ.GetWorkerSignin(1);
            WorkAssignment wa2 = _waServ.Get(1);
            Assert.IsNotNull(result);
            Assert.IsNotNull(wa2.workerAssignedID);
            Assert.IsNotNull(wa2.workerSigninID);
            Assert.IsNotNull(wsi2.WorkAssignmentID);
            Assert.IsNotNull(wsi2.WorkerID);
        }
        [TestMethod]
        public void Integration_WA_Service_GetSummary()
        {
            var result = _waServ.GetSummary("");
            Assert.IsNotNull(result, "Person.ID is Null");
        }

        [TestMethod]
        public void Integration_WA_Service_GetSummary2()
        {
            //
            //Arrange

            //
            //Act
            var woresults = _woServ.GetSummary("8/10/2011");
            var waresults = _waServ.GetSummary("8/10/2011");

            var joined = _woServ.GetSummary("8/10/2011").Join(_waServ.GetSummary("8/10/2011"),
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