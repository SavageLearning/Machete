using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Test.Integration;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class OnlineOrdersServiceTest
    {
        [TestClass]
        public class ReportsV2ServiceTests
        {
            DTO.SearchOptions o;
            FluentRecordBase frb;

            [ClassInitialize]
            public static void ClassInitialize(TestContext c)
            {

            }

            [TestInitialize]
            public void TestInitialize()
            {
                frb = new FluentRecordBase();
            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            public void AutoMapper_OnlineOrder()
            {
                //
                //Arrange
                var wo = frb.CloneOnlineOrder();
                var serv = frb.ToServOnlineOrders();
                var map = frb.ToApiMapper();
                //
                //Act
                var result = map.Map<Api.ViewModel.WorkOrder,  Machete.Domain.WorkOrder>(wo);
                //
                //Assert
                Assert.IsNotNull(result, "DTO.WorkOrderList is Null");
                Assert.IsTrue(result.GetType() == typeof(Machete.Domain.WorkOrder));
            }

            public void CreateOnlineOrder_Succeeds()
            {
                //
                // Arrange
                var wo = frb.CloneWorkOrder();
                wo.workAssignments.Add(frb.CloneWorkAssignment());
                var serv = frb.ToServOnlineOrders();

                // 
                // Act
                var result = serv.Create(wo, "CreateOnlineOrder_Succeeds");
                //
                // Assert
                Assert.IsNotNull(result);

            }
        }
    }
}
