using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class TransportSchedulingTests
    {
        FluentRecordBase frb;

        [ClassInitialize]
        public static void ClassInitialize(TestContext c) { }

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void VehicleSchedule_CreateSucceeds()
        {
            var tvs = frb.ToTransportVehicleSchedule();

            Assert.IsNotNull(tvs);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void VehicleSchedule_PopulateSchedule()
        {
            frb.ToServ<TransportVehiclesScheduleService>().populateScheduleFor(DateTime.Now, "Testy McTestface");

            //Assert.IsNotNull(tvs);
        }
    }
}
