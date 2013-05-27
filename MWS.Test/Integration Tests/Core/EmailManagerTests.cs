using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Test;
using Machete.Service;
using System.Globalization;
using Machete.Data;
using MWS.Core;
using Machete.Domain;

namespace MWS.Test
{
    [TestClass]
    public class EmailManagerTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
            LookupCache.Initialize(frb.DB);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb.Dispose();
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_ProcessQueue()
        {
            // Arrange
            var eServ = frb.AddEmail(status: Email.iReadyToSend).ToServEmail();
            // Act
            var mgr = new EmailManager(eServ, frb.ToUOW());
            // Assert
            mgr.ProcessQueue();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_GetEmailsToSend()
        {
            // Arrange
            var eServ = frb.AddEmail(status: Email.iReadyToSend).ToServEmail();
            // Act
            var emaillist = eServ.GetEmailsToSend();
            foreach (var foo in emaillist)
            {
                
            }
            // Assert
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_Load_EmailConfig()
        {
            // ARRANGE
            var eServ = frb.AddEmail(status: Email.iReadyToSend).ToServEmail();
            var mgr = new EmailManager(eServ, frb.ToUOW());
            // ACT
            var cfg = mgr.LoadEmailConfig();
            // ASSERT
            Assert.IsNotNull(cfg);
            Assert.IsNotNull(cfg.host);
            Assert.IsTrue(cfg.port > 0);
            Assert.IsNotNull(cfg.userName);
            Assert.IsNotNull(cfg.password);
        }
    }
}
