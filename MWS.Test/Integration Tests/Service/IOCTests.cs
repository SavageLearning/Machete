using Machete.Data;
using Machete.Service;
using Machete.Test;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWS.Core;
using MWS.Service;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Transactions;

namespace MWS.Test.Integration_Tests.Service
{
    [TestClass]
    public class IOCTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            //frb = new FluentRecordBase();
            //frb.Initialize(new MacheteInitializer(), "macheteConnection");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //frb.Dispose();
            //frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Build_returns_container()
        {
            // Arrange
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
            var bootstrapper = new InstallBootstrapper();
            IUnityContainer container = bootstrapper.Build();
            // Act
            IEnumerable<Installer> result = container.ResolveAll<Installer>();
            
            // Assert
            Assert.AreEqual(2, result.ToArray().Count(), "InstallBootstrapper for service does not return expected number of objects");
            frb.Dispose();
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Program_Main_gets_to_service()
        {
            var mws = new MacheteWindowsService();
            var result = mws.instances;
            Assert.AreEqual(2,result.Count());
        }
    }
}
