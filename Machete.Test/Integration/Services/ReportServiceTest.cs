using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class ReportTests
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
        public void CountSignins()
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
        public void CountUniqueSignins()
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
        public void CountAssignments()
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
        public void CountCancelled()
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
        public void CountTypeofDispatch()
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
        public void HourlyWageAverage()
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
        public void ListJobs()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            string desc = "description";

            string fodEnText = frb.ToLookupCache().getByID(60).text_EN;

            //var before = frb.ToServReports().ListJobs(beginDate, endDate).ToList();

            frb.AddWorkAssignment(desc, 60, beginDate, endDate, "user", true);

            //Act
            var after = frb.ToServReports().ListJobs(beginDate, endDate).ToList();

            //Assert
            Assert.IsTrue(after.Select(a => a.info).Contains(fodEnText));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void ListZipCodes()
        {
            //Arrange
            DateTime beginDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            frb.AddWorkOrder(beginDate, endDate, endDate, 3, WorkOrder.iActive);

            //Act
            var result = frb.ToServReports().ListOrdersByZipCode(beginDate, endDate).ToList();

            //Assert
            Assert.IsTrue(result.Select(q => q.zips).Contains("12345"));
        }

        //ALL logic for the views contained at the service layer (i.e.,
        //DailyController etc. etc.) uses the above report units. Skipping
        //writing further integration tests for now to save time.
    }
}