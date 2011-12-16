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
    public class WorkerSigninServiceTest
    {

        WorkerSigninRepository _wsiRepo;
        WorkerRepository _wRepo;
        PersonRepository _pRepo;
        WorkOrderRepository _woRepo;
        WorkAssignmentRepository _waRepo;
        WorkerRequestRepository _wrRepo;
        ILookupRepository _lRepo;
        ImageRepository _iRepo;
        DatabaseFactory _dbFactory;
        WorkerSigninService _wsiServ;
        WorkerService _wServ;
        PersonService _pServ;
        ImageService _iServ;
        WorkerRequestService _wrServ;
        WorkOrderService _woServ;
        WorkAssignmentService _waServ;
        IUnitOfWork _unitofwork;
        //MacheteContext MacheteDB;
        DispatchOptions _dOptions;
        //CultureInfo CI;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context) 
        {
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            Records.Initialize(new MacheteContext());
            WorkerCache.Initialize(new MacheteContext());
            LookupCache.Initialize(new MacheteContext());            
            Lookups.Initialize(LookupCache.getCache());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dbFactory = new DatabaseFactory();
            _iRepo = new ImageRepository(_dbFactory);
            _wRepo = new WorkerRepository(_dbFactory);
            _woRepo = new WorkOrderRepository(_dbFactory);
            _wrRepo = new WorkerRequestRepository(_dbFactory);
            _waRepo = new WorkAssignmentRepository(_dbFactory);
            _wsiRepo = new WorkerSigninRepository(_dbFactory);
            _lRepo = new LookupRepository(_dbFactory);
            _pRepo = new PersonRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _pServ = new PersonService(_pRepo, _unitofwork);
            _iServ = new ImageService(_iRepo, _unitofwork);
            _wrServ = new WorkerRequestService(_wrRepo, _unitofwork);
            _waServ = new WorkAssignmentService(_waRepo, _wRepo, _lRepo, _wsiRepo,_wrRepo, _unitofwork);
            _wServ = new WorkerService(_wRepo, _unitofwork);
            _woServ = new WorkOrderService(_woRepo, _waServ, _unitofwork);
            _wsiServ = new WorkerSigninService(_wsiRepo, _wRepo, _pRepo, _iRepo, _wrRepo, _unitofwork);
            _dOptions = new DispatchOptions
            {
                CI = new CultureInfo("en-US", false),
                search = "",
                date = DateTime.Parse("8/10/2011"),
                dwccardnum = null,
                woid = null,
                orderDescending = true,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }
        [TestMethod]
        public void DbSet_WorkerSignin_GetView()
        {
            DateTime date = Convert.ToDateTime("08/10/2011");
            IEnumerable<WorkerSigninView> filteredWSI = _wsiServ.getView(date);
            IEnumerable<WorkerSigninView> foo = filteredWSI.ToList();
            Assert.IsNotNull(filteredWSI, "Person.ID is Null");
            Assert.IsNotNull(foo, "Person.ID is Null");
        }
        //
        /// <summary>
        /// Filters WSI IndexView based on dwccardnum option. should return all records.
        /// </summary>
        [TestMethod]
        public void DbSet_WorkerSigninService_Intergation_GetIndexView_check_search_dwccardnum()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            ServiceIndexView<WorkerSigninView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<WorkerSigninView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkerSigninView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(3, result.filteredCount);
            Assert.AreEqual(5, result.totalCount);
        }
        /// <summary>
        /// Filter on requested grouping
        /// </summary>
        [TestMethod]
        public void DbSet_WorkerSigninService_Intergation_GetIndexView_workerRequested()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            _dOptions.wa_grouping = "requested";
            ServiceIndexView<WorkerSigninView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<WorkerSigninView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkerSigninView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(1, result.filteredCount);
            Assert.AreEqual(5, result.totalCount);
        }
        [TestMethod]
        public void DbSet_TestMethod5()
        {

            IEnumerable<WorkerSignin> testing = _wsiServ.GetSigninsForAssignment(Convert.ToDateTime("08/02/2011"),
                                                        "Jose",
                                                        "asc",
                                                        null,
                                                        null);
            Assert.IsNotNull(testing, "null");
        }
    }
}