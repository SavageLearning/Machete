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

namespace Machete.Test
{
    [TestClass]
    public class WorkOrderServiceTest
    {
        WorkOrderRepository _woRepo;
        DatabaseFactory _dbFactory;
        WorkOrderService _service;
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

            //DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
            MacheteDB = new MacheteContext();
            _dbFactory = new DatabaseFactory();
            _woRepo = new WorkOrderRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _service = new WorkOrderService(_woRepo, _unitofwork);
        }

        [TestMethod]
        public void DbSet_WorkOrderService_Intergation_GetSummary()
        {
            //
            //Arrange

            //
            //Act
            var result = _service.GetSummary();

            //
            //Assert
            Assert.IsNotNull(result, "Person.ID is Null");
            //Assert.IsTrue(_person.ID == 1);
        }
    }
}
