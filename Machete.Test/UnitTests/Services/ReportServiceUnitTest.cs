using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using Moq;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Domain;
using Machete.Test;

namespace Machete.Test.UnitTests.Services
{
    class ReportServiceUnitTest
    {
        Mock<IUnitOfWork> _uow;
        Mock<IWorkOrderRepository> _woRepo;
        Mock<IWorkAssignmentRepository> _waRepo;
        Mock<IWorkerRepository> _wRepo;
        Mock<IWorkerSigninRepository> _wsiRepo;
        Mock<IWorkerRequestRepository> _wrRepo;
        Mock<ILookupRepository> _lookRepo;
        Mock<ILookupCache> _lookCache;

        List<WorkerSignin> _signins;
        List<Worker> _workers;
        List<WorkOrder> _orders;
        List<WorkerRequest> _requests;

        ReportService _serv;
        
        WorkerSigninService _wsiServ;
        WorkAssignmentService _waServ;
        WorkOrderService _woServ;

        public ReportServiceUnitTest()
        {
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _uow = new Mock<IUnitOfWork>();

            _signins = new List<WorkerSignin>();
            _signins.Add(new WorkerSignin() { ID = 111, dwccardnum = 12345, dateforsignin = DateTime.Now });
            _signins.Add(new WorkerSignin() { ID = 112, dwccardnum = 24680, dateforsignin = DateTime.Now });

            _workers = new List<Worker>();
            _workers.Add(new Worker() { ID = 1, dwccardnum = 12345 });
            _workers.Add(new Worker() { ID = 2, dwccardnum = 24680 });
            _workers.Add(new Worker() { ID = 3, dwccardnum = 36925 });
            _workers.Add(new Worker() { ID = 4, dwccardnum = 48260 });

            _orders = new List<WorkOrder>();
            _requests = new List<WorkerRequest>();

            _wsiRepo = new Mock<IWorkerSigninRepository>();
            _wsiRepo.Setup(s => s.GetAll()).Returns(_signins);

            _wRepo = new Mock<IWorkerRepository>();
            _wRepo.Setup(s => s.GetAll()).Returns(_workers);

            _woRepo = new Mock<IWorkOrderRepository>();
            _woRepo.Setup(s => s.GetAll()).Returns(_orders);

            _wrRepo = new Mock<IWorkerRequestRepository>();
            _wrRepo.Setup(s => s.GetAll()).Returns(_requests);

            _waRepo = new Mock<IWorkAssignmentRepository>();
            _lookRepo = new Mock<ILookupRepository>();
            _lookCache = new Mock<ILookupCache>();

            _waServ = new WorkAssignmentService(_waRepo.Object, _wRepo.Object, _lookRepo.Object, _wsiRepo.Object, _lookCache.Object, _uow.Object);
            _woServ = new WorkOrderService(_woRepo.Object, _waServ, _uow.Object);
        }
        [TestMethod]
        public void CountSignins_Returns_signins_for_day()
        {
            DateTime date = DateTime.Now;

            //Note the <int, int> -- with Moq you have to specify return types in this fashion
            int currentSignins =
                _wsiRepo.Setup<int, int>(s => s
                    .GetAllQ()
                    .Where(p => EntityFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                    .AsEnumerable()
                    .Count());

            _signins.Add(new WorkerSignin() { ID = 113, dwccardnum = 36925, dateforsignin = DateTime.Now });
            _signins.Add(new WorkerSignin() { ID = 114, dwccardnum = 48620, dateforsignin = DateTime.Now });

            int newSignins =
                _wsiRepo.Setup<int, int>(s => s
                    .GetAllQ()
                    .Where(p => EntityFunctions.DiffDays(p.dateforsignin, date) == 0 ? true : false)
                    .AsEnumerable()
                    .Count());

            int signinDiff = newSignins - currentSignins;

            Assert.IsTrue(signinDiff == 2);
        }
    }
}
