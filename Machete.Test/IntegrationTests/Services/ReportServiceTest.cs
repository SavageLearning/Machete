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
        public void Integration_ReportService_CountSignins()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            frb.AddWorker().AddWorkerSignin();
            frb.AddWorker().AddWorkerSignin();
            //Act
            var result = frb.ToServReports().CountSignins(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountUniqueSignins()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            //really not a whole lot else we can do here, given that
            //we can't manipulate dateforsignin
            frb.AddWorker().AddWorkerSignin();
            frb.AddWorker().AddWorkerSignin();
            //Act
            var result = frb.ToServReports().CountUniqueSignins(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountAssignments()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            var desc = "DESCRIPTION " + frb.RandomString(20);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            frb.AddWorkAssignment(desc: desc).AddWorkAssignment(desc: desc);
            //Act
            var result = frb.ToServReports().CountAssignments(beginDate, endDate).ToList();
            //Assert
            Assert.AreEqual(8, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountCancelled()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            frb.AddWorkOrder(beginDate, beginDate, beginDate, 1, WorkOrder.iActive);
            frb.AddWorkOrder(beginDate, beginDate, beginDate, 1, WorkOrder.iCancelled);
            frb.AddWorkOrder(beginDate, beginDate, beginDate, 1, WorkOrder.iCancelled);

            //Act
            var result = frb.ToServReports().CountCancelled(beginDate, endDate);

            //Assert
            Assert.AreEqual(2, result.Select(q => q.count));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_CountTypeofDispatch()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            frb.AddWorkAssignment(desc, 60, beginDate, beginDate, "user", true);
            frb.AddWorkAssignment(desc, 60, beginDate, beginDate, "user", true);
            frb.AddWorkAssignment(desc, 67, beginDate, beginDate, "user", true);

            //Act
            var result = frb.ToServReports().CountTypeofDispatch(beginDate, endDate);

            //Assert
            Assert.AreEqual(2, result.Select(q => q.dwcList));
            Assert.AreEqual(1, result.Select(q => q.hhhList));
            Assert.AreEqual(0, result.Select(q => q.dwcPropio));
            Assert.AreEqual(0, result.Select(q => q.hhhPropio));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_HourlyWageAverage()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            //this assumes that general labor is the lower wage, but it won't matter because they are averaged
            double? bottomWage = frb.ToLookupCache().getByID(60).wage;
            double? middleWage = frb.ToLookupCache().getByID(65).wage;
            double? topWage = frb.ToLookupCache().getByID(66).wage;

            double? avg = (bottomWage + middleWage + topWage) / 3;

            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 65, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 66, beginDate, endDate, "user", true);

            //Act
            var result = frb.ToServReports().HourlyWageAverage(beginDate, endDate);

            //Assert
            Assert.AreEqual(avg, result.Select(q => q.avg));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_ListJobs()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            // fod is "first or default" because that's what we'll
            // have to test against
            // (returned object is IQueryable of type ReportUnit)
            string fodEnText = frb.ToLookupCache().getByID(60).text_EN;

            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);
            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);

            //Act
            var result = frb.ToServReports().ListJobs(beginDate,endDate);


            //Assert
            Assert.AreEqual(4, result.Select(q => q.count));
            Assert.AreEqual(fodEnText, result.Select(q => q.info.FirstOrDefault()));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_ReportService_ListZipCodes()
        {
            //Arrange
            DateTime beginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 0, 0, 0);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 3, 23, 59, 59);

            frb.AddWorkOrder(beginDate, endDate, endDate, 3, WorkOrder.iActive);

            //pretty sure this is not right
            string zip = frb.ToWorkOrder().zipcode;
            //Act
            var result = frb.ToServReports().ListZipCodes(beginDate, endDate);

            //Assert
            Assert.AreEqual(zip, result.Select(q => q.info.FirstOrDefault()));
        }

        //ALL logic for the views contained at the service layer (i.e.,
        //DailyController etc. etc.) uses the above report units. Skipping
        //writing further integration tests for now to save time.
    }
}
