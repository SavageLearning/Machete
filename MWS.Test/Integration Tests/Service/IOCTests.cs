using Machete.Test;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWS.Service;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;

namespace MWS.Test.Integration_Tests.Service
{
    [TestClass]
    public class IOCTests
    {
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Build_returns_container()
        {
            // Arrange
            var bootstrapper = new InstallBootstrapper();
            IUnityContainer container = bootstrapper.Build();
            // Act
            IEnumerable<Installer> result = container.ResolveAll<Installer>();
            
            // Assert
            Assert.AreEqual(2, result.ToArray().Count(), "InstallBootstrapper for service does not return expected number of objects");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Program_Main_gets_to_service()
        {
            MWS.Service.Program.Main();
        }
    }
}
