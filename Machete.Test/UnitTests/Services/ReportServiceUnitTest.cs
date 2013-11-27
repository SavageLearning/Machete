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
    public class ReportServiceUnitTest
    {
        Mock<IWorkOrderRepository> _woRepo;
        Mock<IWorkAssignmentRepository> _waRepo;
        Mock<IWorkerRepository> _wRepo;
        Mock<IWorkerSigninRepository> _wsiRepo;
        Mock<IWorkerRequestRepository> _wrRepo;
        Mock<ILookupRepository> _lRepo;
        Mock<ILookupCache> _lCache;

        ReportService _serv;
        
        /// <summary>
        /// An empty constructor...
        /// </summary>
        public ReportServiceUnitTest()
        {
        }

        /// <summary>
        /// Some test context magic
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
            _woRepo = new Mock<IWorkOrderRepository>();
            _waRepo = new Mock<IWorkAssignmentRepository>();
            _wRepo = new Mock<IWorkerRepository>();
            _wsiRepo = new Mock<IWorkerSigninRepository>();
            _wrRepo = new Mock<IWorkerRequestRepository>();
            _lRepo = new Mock<ILookupRepository>();
            _lCache = new Mock<ILookupCache>();
            //Because the preceding are Mock objects, you have to call the Object feature on the object's method.
            _serv = new ReportService(_woRepo.Object,_waRepo.Object,_wRepo.Object,_wsiRepo.Object,_wrRepo.Object,_lRepo.Object,_lCache.Object);
        }

        [TestMethod]
        public void CountSignins_Returns_signins_for_day()
        {
            //Arrange
            DateTime beginDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            int myInt = 3;

            _wsiRepo.Setup(fn => fn.GetAllQ().Count()).Returns(myInt);
            
            //Act
            var myAction = _serv.CountSignins(beginDate, endDate);

            //Assert
            Assert.AreEqual(myInt, myAction.FirstOrDefault().count);
        }
    }
}
