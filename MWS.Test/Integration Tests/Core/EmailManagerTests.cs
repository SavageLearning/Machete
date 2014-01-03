using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Test;
using Machete.Service;
using System.Globalization;
using Machete.Data;
using MWS.Core;
using Machete.Domain;
using Microsoft.Practices.Unity;
using System.Data.Entity.Infrastructure;
using System.Configuration;

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
            // populates domain constants
            cache = new LookupCache(frb.ToFactory()); //Func<> to DB Factory

        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_ProcessQueue_send_one_email()
        {
            // Arrange
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var eServ = frb.ToServEmail();
            var em = new EmailQueueManager(eServ, frb.ToUOW());
            em.ProcessQueue(cfg.Instances[0].EmailQueue.EmailServer); // clear queue
            frb.AddEmail(status: Email.iReadyToSend);
            var mgr = new EmailQueueManager(eServ, frb.ToUOW());
            // Act
            mgr.ProcessQueue(cfg.Instances[0].EmailQueue.EmailServer);
            // Assert
            Assert.AreEqual(1, mgr.sentStack.Count);
            Assert.AreEqual(0, mgr.exceptionStack.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_ProcessQueue_send_one_email_with_attachent()
        {
            // Arrange
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var eServ = frb.ToServEmail();
            var em = new EmailQueueManager(eServ, frb.ToUOW());
            em.ProcessQueue(cfg.Instances[0].EmailQueue.EmailServer); // clear queue
            frb.AddEmail(status: Email.iReadyToSend, 
                attachment: frb.ValidAttachment,
                attachmentType: System.Net.Mime.MediaTypeNames.Text.Html);
            var mgr = new EmailQueueManager(eServ, frb.ToUOW());
            // Act
            mgr.ProcessQueue(cfg.Instances[0].EmailQueue.EmailServer);
            // Assert
            Assert.AreEqual(1, mgr.sentStack.Count);
            Assert.AreEqual(0, mgr.exceptionStack.Count);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Integration_Email_EF_Test_concurrency_exception()
        {
            // Arrange
            var db1 = new MacheteContext();
            var e1 = new Email()
            {
                emailFrom = "changeme@gmail.com",
                emailTo = "changeme@gmail.com",
                subject = "testing",
                body = "testing",
                statusID = Email.iReadyToSend,
                datecreated = DateTime.Now,
                dateupdated = DateTime.Now
            };
            db1.Emails.Add(e1);
            db1.SaveChanges();   // initial save, context 1
            var db2 = new MacheteContext();
            var e2 = db2.Emails.Find(e1.ID);
            e2.statusID = Email.iSending;
            db2.SaveChanges();  // context 2 saves on top of context 1
            e1.statusID = Email.iPending;
            // Act
            db1.SaveChanges(); // context 1 tries to save again, throws exception
        }
        [Ignore]
        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_Email_MWS_Load_EmailConfig()
        {
            //// ARRANGE
            //var eServ = frb.AddEmail(status: Email.iReadyToSend).ToServEmail();
            //var mgr = new EmailQueueManager(eServ, frb.ToUOW());
            //// ACT
            //var cfg = mgr.LoadEmailConfig();
            //// ASSERT
            //Assert.IsNotNull(cfg);
            //Assert.IsNotNull(cfg.HostName);
            //Assert.IsTrue(cfg.Port > 0);
            //Assert.IsNotNull(cfg.OutgoingAccount);
            //Assert.IsNotNull(cfg.OutgoingPassword);
        }
    }
}
