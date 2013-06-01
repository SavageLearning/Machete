using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Test;
using Machete.Service;
using System.Globalization;
using Machete.Data;
using MWS.Core;
using Machete.Domain;
using Microsoft.Practices.Unity;

namespace MWS.Test
{
    [TestClass]
    public class EmailManagerTests
    {
        FluentRecordBase frb;
        LookupCache cache;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
            // populates domain constants
            cache = new LookupCache(() => frb.ToFactory()); //Func<> to DB Factory

        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb.Dispose();
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_ProcessQueue_send_one_email()
        {
            // Arrange
            var eServ = frb.AddEmail(status: Email.iReadyToSend).ToServEmail();
            // Act
            var mgr = new EmailManager(eServ, frb.ToUOW());
            // Assert
            mgr.ProcessQueue();
            //Assert.AreEqual(1, mgr.sentStack.Count);
            Assert.AreEqual(0, mgr.exceptionStack.Count);
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
