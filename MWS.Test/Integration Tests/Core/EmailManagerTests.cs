using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Test;
using Machete.Service;
using System.Globalization;
using Machete.Data;
using MWS.Core;

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

        [TestMethod]
        public void TestMethod1()
        {
            var eServ = frb.ToServEmail();
            var mgr = new EmailManager(eServ);
            mgr.ProcessQueue();
        }
    }
}
