using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using Moq;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Domain;
using Machete.Test;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for EmployerServiceUnitTests
    /// </summary>
    [TestClass]
    public class EmployerServiceUnitTests
    {
        Mock<IEmployerRepository> _repo;
        Mock<IUnitOfWork> _uow;
        Mock<IWorkOrderService> _woServ;

        public EmployerServiceUnitTests()
        {
        }

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


        [TestMethod]
        public void EmployerService_GetEmployers_returns_Enumerable()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //Act
            var result = _serv.GetEmployers(false);
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Employer>));
        }
        //TODO Test filter for employers
        [TestMethod]
        public void EmployerService_GetEmployer_returns_employer()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            int id = 3; //This matches Records._employer3 ID value
            _repo.Setup(r => r.GetById(id)).Returns(Records._employer3);
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //Act
            var result = _serv.GetEmployer(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Employer));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod]
        public void EmployerService_CreateEmployer_returns_employer()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            string user = "UnitTest";
            Records._employer1.datecreated = DateTime.MinValue;
            Records._employer1.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(Records._employer1)).Returns(Records._employer1);
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            var result = _serv.CreateEmployer(Records._employer1, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Employer));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod]
        public void EmployerService_DeleteEmployer()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>();
            _woServ = new Mock<IWorkOrderService>();
            string user = "UnitTest";
            int id = 1;
            Employer dp = new Employer();
            _repo.Setup(r => r.Delete(It.IsAny<Employer>())).Callback((Employer p) => { dp = p; });
            _repo.Setup(r => r.GetById(id)).Returns(Records._employer1);
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            _serv.DeleteEmployer(id, user);
            //
            //Assert
            Assert.AreEqual(dp, Records._employer1);
        }

        [TestMethod]
        public void EmployerService_SaveEmployer_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IEmployerRepository>();
            _uow = new Mock<IUnitOfWork>(); 
            _woServ = new Mock<IWorkOrderService>();
            string user = "UnitTest";
            Records._employer1.datecreated = DateTime.MinValue;
            Records._employer1.dateupdated = DateTime.MinValue;
            var _serv = new EmployerService(_repo.Object, _woServ.Object, _uow.Object);
            //
            //Act
            _serv.SaveEmployer(Records._employer1, user);
            //
            //Assert
            Assert.IsTrue(Records._employer1.Updatedby == user);
            Assert.IsTrue(Records._employer1.dateupdated > DateTime.MinValue);
        }
    }
}
