using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWS.Core;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Configuration;



namespace MWS.Test.Integration_Tests.Core
{
    [TestClass]
    public class ConfigurationTests
    {
        MacheteWindowsServiceConfiguration cfg;

        [TestInitialize]
        public void TestInitialize()
        {
            cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
        }

        [TestCleanup]
        public void TestCleanup() { }

        [Ignore] //MWS deprecated
        [TestMethod]
        public void Integration_MWSConfiguration_check_instances()
        {
            var instances = cfg.Instances;
            Assert.AreEqual(2, instances.Count);
        }

        [Ignore] //MWS deprecated
        [TestMethod]
        public void Integration_MWSConfiguration_check_instance_name()
        {
            InstanceCollection instances = cfg.Instances;
            var instance = instances.First();
            Assert.AreEqual("CasaLatinaSeattle", instance.Name);
        }

        [Ignore] //MWS deprecated
        [TestMethod]
        public void Integration_MWSConfiguration_check_EmailQueue()
        {
            var queue = cfg.Instances.First().EmailQueue;
            Assert.AreEqual(10, queue.TimerIntervalSeconds);
        }

        [Ignore] //MWS deprecated
        [TestMethod]
        public void Integration_MWSConfiguration_check_EmailServer()
        {
            var server = cfg.Instances.First().EmailQueue.EmailServer;
            Assert.AreEqual("Google", server.Name);
        }
    }
}
