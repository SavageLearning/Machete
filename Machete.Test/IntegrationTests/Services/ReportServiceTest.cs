using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.IntegrationTests.Services
{
    public class ReportServiceTest
    {
        FluentRecordBase frb;
        viewOptions _dOptions;

        //we may not need all of these options here
        //copied from WorkerSigninServiceTest
        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
            _dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = true,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountDailySignins_First_Overload_Method()
        {
            DateTime beginDate = _dOptions.date ?? DateTime.Today;
            var result = frb.ToReportServ().CountDailySignins(beginDate);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountDailySignins_Second_Overload_Method()
        {
            DateTime beginDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;
            var result = frb.ToReportServ().CountDailySignins(beginDate, endDate);
        }
    }
}
