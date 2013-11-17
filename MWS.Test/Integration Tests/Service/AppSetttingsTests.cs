using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using MWS.Core;

namespace MWS.Test.Integration_Tests.Service
{
    [TestClass]
    public class AppSetttingsTests
    {
        public MacheteWindowsServiceConfiguration config;

        [TestInitialize]
        public void Initialize()
        {
            config = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;

        }

        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_count()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var count = instances.Count();
            Assert.AreEqual(2, count);
        }
        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_email_queue()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var result = instances.First().EmailQueue;
            Assert.IsInstanceOfType(result, typeof(EmailQueue));
        }
        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_email_server()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var result = instances.First().EmailQueue.EmailServer;
            Assert.IsInstanceOfType(result, typeof(EmailServer));
        }
        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_outgoing_account()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var result = instances.First().EmailQueue.EmailServer.OutgoingAccount;
            Assert.AreEqual(result, "CLSeattle@gmail.com");
        }
        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_port()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var result = instances.First().EmailQueue.EmailServer.Port;
            Assert.AreEqual(result, 587);
        }
        [TestMethod]
        public void AppSettings_MacheteWindowsServiceConfiguration_basic_load_returns_enableSSL()
        {
            var instances = config.Instances.Cast<Instance>().AsEnumerable();
            var result = instances.First().EmailQueue.EmailServer.EnableSSL;
            Assert.AreEqual(result, true);
        }
    }
}
