using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.IntegrationTests.Services
{
    [TestClass]
    public class ReportServiceTest
    {
        FluentRecordBase frb;
        viewOptions _dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            //frb.Initialize(new MacheteInitializer(), "macheteConnection");
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
        public void Integration_ReportService_CountSignins()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            //really not a whole lot else we can do here, given that
            //we can't manipulate dateforsignin
            var before = frb.ToServReports().CountSignins(beginDate, endDate).ToList();
            frb.AddWorker().AddWorkerSignin();
            //Act
            var after = frb.ToServReports().CountSignins(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(before.Select(q => q.count).FirstOrDefault(), after.Select(q => q.count).FirstOrDefault() - 1);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountUniqueSignins()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            //really not a whole lot else we can do here, given that
            //we can't manipulate dateforsignin
            var before = frb.ToServReports().CountUniqueSignins(beginDate, endDate).ToList();
            frb.AddWorker().AddWorkerSignin();
            //Act
            var after = frb.ToServReports().CountUniqueSignins(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(before.Select(q => q.count).FirstOrDefault(), after.Select(q => q.count).FirstOrDefault() - 1);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountAssignments()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            var desc = "DESCRIPTION " + frb.RandomString(20);
            var before = frb.ToServReports().CountAssignments(beginDate, endDate).ToList();
            frb.AddWorkAssignment(desc: desc); //only seems to add one no matter how many times I do this
            //Act
            var after = frb.ToServReports().CountAssignments(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(before.Select(q => q.count).FirstOrDefault(), after.Select(q => q.count).FirstOrDefault() - 1);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountCancelled()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            var before = frb.ToServReports().CountCancelled(beginDate, endDate).ToList();
            int beforeInt = before.Select(q => q.count).FirstOrDefault() ?? 0;

            frb.AddWorkOrder(beginDate, beginDate, beginDate, 1, WorkOrder.iCancelled);

            //Act
            var after = frb.ToServReports().CountCancelled(beginDate, endDate).ToList();

            //Assert
            Assert.AreEqual(beforeInt, after.Select(q => q.count).FirstOrDefault() - 1);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountTypeofDispatch()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            var before = frb.ToServReports().CountTypeofDispatch(beginDate, endDate).ToList();

            //frb.AddWorkAssignment();
            //There's just no good way to test for this report with the methods that exist (TODO: write more test methods)
            //Act
            var after = frb.ToServReports().CountTypeofDispatch(beginDate, endDate).ToList();

            //Assert
            Assert.AreEqual(before.Select(q => q.dwcList).FirstOrDefault(), after.Select(q => q.dwcList).FirstOrDefault());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_HourlyWageAverage()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            //this assumes that general labor is the lower wage, but it won't matter because they are averaged

            var before = frb.ToServReports().HourlyWageAverage(beginDate, endDate).ToList();
            frb.AddWorkAssignment(desc, 88, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 88, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 88, beginDate, endDate, "user", true);

            //Act
            var after = frb.ToServReports().HourlyWageAverage(beginDate, endDate).ToList();

            //Assert
            //with the current value for "wage" for lookup 88, it's pretty much impossible that these two would be equal...
            Assert.AreNotEqual(before.Select(q => q.avg), after.Select(q => q.avg));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_ListJobs()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            string fodEnText = frb.ToLookupCache().getByID(60).text_EN;

            //var before = frb.ToServReports().ListJobs(beginDate, endDate).ToList();

            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);

            //Act
            var after = frb.ToServReports().ListJobs(beginDate,endDate).ToList();

            //Assert
            Assert.IsTrue(after.Select(a => a.info).Contains(fodEnText));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_ListZipCodes()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            frb.AddWorkOrder(beginDate, endDate, endDate, 3, WorkOrder.iActive);

            //Act
            var result = frb.ToServReports().ListZipCodes(beginDate, endDate).ToList();

            //Assert
<<<<<<< HEAD
            Assert.AreEqual(zip, result.Select(q => q.info.FirstOrDefault()));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_NewlyEnrolled()
        {
            //Arrange
            DateTime beforeDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 4, 23, 59, 59);
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime middleDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 12, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);
            DateTime afterDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 2, 0, 0, 0);

            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                afterDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                middleDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                middleDate,
                endDate, afterDate, null);

            //Act
            var result = frb.ToServReports().NewlyEnrolled(beginDate, endDate);

            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_NewlyExpired()
        {
            //Arrange
            DateTime beforeDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 4, 23, 59, 59);
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime middleDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 12, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);
            DateTime afterDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 2, 0, 0, 0);

            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, beforeDate, null);
            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, middleDate, null);
            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, middleDate, null);

            //Act
            var result = frb.ToServReports().NewlyExpired(beginDate, endDate);

            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_StillEnrolled()
        {
            //Arrange
            DateTime beforeDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 4, 23, 59, 59);
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime middleDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 12, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);
            DateTime afterDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 2, 0, 0, 0);

            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                afterDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                beforeDate,
                endDate, afterDate, null);
            frb.AddWorker(null, null, null, null,
                middleDate,
                endDate, afterDate, null);


            //Act
            var result = frb.ToServReports().StillEnrolled(beginDate, endDate);

            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_ListZipCodes()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            // Creates a Work Order with a zip code of "12345"
            frb.AddWorkOrder(beginDate, endDate, endDate, 3, WorkOrder.iActive);

            //Act
            var result = frb.ToServReports().ListZipCodes(beginDate, endDate).ToList();

            //Assert
            Assert.IsTrue(result.Select(q => q.info).Contains("12345"));
        }

        //ALL logic for the views contained at the service layer (i.e.,
        //DailyController etc. etc.) uses the above report units. Skipping
        //writing further integration tests for now to save time.
    }
}
