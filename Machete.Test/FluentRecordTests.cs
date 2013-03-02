using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Domain;

namespace Machete.Test
{
    [TestClass]
    public class FluentRecordTests
    {
        [TestMethod]
        public void FluentRecordBase_AddRepoEmployer()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToEmployer();
            Assert.IsInstanceOfType(result, typeof(Employer));
        }

        [TestMethod]
        public void FluentRecordBase_AddRepoWorkOrder()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToWorkOrder();
            Assert.IsInstanceOfType(result, typeof(WorkOrder));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoWorkAssignment()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToWorkAssignment();
            Assert.IsInstanceOfType(result, typeof(WorkAssignment));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoWorkerSignin()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToWorkerSignin();
            Assert.IsInstanceOfType(result, typeof(WorkerSignin));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoWorkerRequest()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToWorkerRequest();
            Assert.IsInstanceOfType(result, typeof(WorkerRequest));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoPerson()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToPerson();
            Assert.IsInstanceOfType(result, typeof(Person));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoWorker()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToWorker();
            Assert.IsInstanceOfType(result, typeof(Worker));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoActivity()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToActivity();
            Assert.IsInstanceOfType(result, typeof(Activity));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoActivitySignin()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToActivitySignin();
            Assert.IsInstanceOfType(result, typeof(ActivitySignin));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoImage()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToImage();
            Assert.IsInstanceOfType(result, typeof(Image));
        }
        [TestMethod]
        public void FluentRecordBase_AddRepoLookup()
        {
            var frb = new FluentRecordBase();
            var result = frb.ToLookup();
            Assert.IsInstanceOfType(result, typeof(Lookup));
        }

    }
}
