using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Domain;
using Machete.Test.Integration;

namespace Machete.Test
{
    [TestClass]
    public class FluentRecordTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Employers)]
        public void FluentRecordBase_AddRepoEmployer()
        {
            var result = frb.ToEmployer();
            Assert.IsInstanceOfType(result, typeof(Employer));
        }

        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.WorkOrders)]
        public void FluentRecordBase_AddRepoWorkOrder()
        {
            var result = frb.ToWorkOrder();
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.WAs)]
        public void FluentRecordBase_AddRepoWorkAssignment()
        {
            var result = frb.ToWorkAssignment();
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.WSIs)]
        public void FluentRecordBase_AddRepoWorkerSignin()
        {
            var result = frb.ToWorkerSignin();
            Assert.IsInstanceOfType(result, typeof(WorkerSignin));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.WorkOrders)]
        public void FluentRecordBase_AddRepoWorkerRequest()
        {
            var result = frb.ToWorkerRequest();
            Assert.IsInstanceOfType(result, typeof(WorkerRequest));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Persons)]
        public void FluentRecordBase_AddRepoPerson()
        {
            var result = frb.AddPerson(testID: "FluentRecordBase_AddRepoPerson").ToPerson();
            Assert.IsInstanceOfType(result, typeof(Person));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Workers)]
        public void FluentRecordBase_AddRepoWorker()
        {
            var result = frb.AddPerson(testID: "FluentRecordBase_AddRepoWorker").ToWorker();
            Assert.IsInstanceOfType(result, typeof(Worker));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Activities)]
        public void FluentRecordBase_AddRepoActivity()
        {
            var result = frb.AddPerson(testID: "FluentRecordBase_AddRepoActivity").ToActivity();
            Assert.IsInstanceOfType(result, typeof(Activity));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Activities)]
        public void FluentRecordBase_AddRepoActivitySignin()
        {
            var result = frb.ToActivitySignin();
            Assert.IsInstanceOfType(result, typeof(ActivitySignin));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Images)]
        public void FluentRecordBase_AddRepoImage()
        {
            var result = frb.ToImage();
            Assert.IsInstanceOfType(result, typeof(Image));
        }
        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT)]
        public void FluentRecordBase_AddRepoLookup()
        {
            var result = frb.ToLookup();
            Assert.IsInstanceOfType(result, typeof(Lookup));
        }

        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.Events)]
        public void FluentRecordBase_AddRepoEvent()
        {
            var result = frb.ToEvent();
            Assert.IsInstanceOfType(result, typeof(Event));
        }

        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.TransportRules)]
        public void FluentRecordBase_AddRepoTransportRule()
        {
            var result = frb.ToTransportRule();
            Assert.IsInstanceOfType(result, typeof(TransportRule));
        }


        [TestMethod, TestCategory(TC.Fluent), TestCategory(TC.IT), TestCategory(TC.ScheduleRules)]
        public void FluentRecordBase_AddScheduleRule()
        {
            var result = frb.ToEvent();
            Assert.IsInstanceOfType(result, typeof(Event));
        }
    }
}
