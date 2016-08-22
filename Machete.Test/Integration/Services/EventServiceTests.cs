using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class EventTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = false,
                sortColName = "",
                displayStart = 0,
                displayLength = 20
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Events)]
        public void Add_event()
        {
            var _ev = frb.ToEvent();

            // ACT
            // ASSERT
            Assert.IsNotNull(_ev);
        }
    }
}
