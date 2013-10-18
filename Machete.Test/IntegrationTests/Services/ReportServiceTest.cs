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
            var result = frb.ToServReports().CountSignins(beginDate);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountDailySignins_Second_Overload_Method()
        {
            DateTime beginDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;
            var result = frb.ToServReports().CountSignins(beginDate, endDate);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_DailyCasaLatina()
        {
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);
            var result = frb.ToServReports().CountTypeofDispatch(beginDate, endDate);
        }

        [TestMethod]//, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_Reports_Service_GetSummary()
        {
            ////
            ////Arrange
            //using (var ctx = MacheteContext
            //    .CreateTracingContext("macheteDevTest", 
            //                            x => {
            //                                Console.WriteLine(x.ToFlattenedTraceString());
            //                            }, 
            //                            true, 
            //                            @"c:\MyContext.output")
            //)
            //{
            //    var result = frb.AddWorkOrder().ToServReports().wecView(DateTime.Now).totalCount;

            //}
            ////
            ////Act
            ////
            ////Assert

        }

        [TestMethod]
        public void Integration_Reports_Service_ListZipCodes()
        {
            //Arrange
            //Act
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);
            var result = frb.ToServReports().ListZipCodes(beginDate, endDate);
            Console.WriteLine(result);
            //Assert
        }


    }
}
