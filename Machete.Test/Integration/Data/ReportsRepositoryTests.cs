using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data.DTO;
using System.Linq;
using Machete.Data;
using Machete.Test.Integration.Fluent;

namespace Machete.Test.Integration.Data
{
    [TestClass]
    public class ReportsRepositoryTests // TODO "duplicate value" bug??
    {
        private FluentRecordBase frb;
        private string connectionString { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            frb = FluentRecordBaseFactory.Get();
            connectionString = "Server=localhost,1433; Database=machete_db; User=readonlylogin; Password=@testPassword1;";
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data), TestCategory(TC.Reports)]
        public void getDynamicQuery_returns_list()
        {
            // arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"), status: 42)
                .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer
            var result = frb.ToRepoReports()
                .getDynamicQuery(1, new SearchOptions {
                    beginDate = DateTime.Parse("2013/1/1"),
                    endDate = DateTime.Parse("2013/3/1"),
                    dwccardnum = 0
                });
            // act
            // assert
            Assert.AreEqual("20130101-20130301-DispatchesByJob-63", result[0].id);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data), TestCategory(TC.Reports)]
        public void getDynamicQuery_test_all_metadata()
        {
            // arrange
            var reports = frb.ToFactory().ReportDefinitions.AsQueryable();

            foreach (var r in reports)
            {
                var result = MacheteAdoContext.getMetadata(r.sqlquery, connectionString);
                Assert.IsTrue(result.Count > 2);
            }
            // act
            // assert
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data), TestCategory(TC.Reports)]
        public void getDefaultURL()
        {
            // arrange
            // act
            var result = frb.ToRepoReports()
                .getList();
            // assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data), TestCategory(TC.Reports)]
        public void Analyze_columns()
        {
            var repo = frb.ToRepoReports();
            var list = repo.GetAllQ();
            foreach (var l in list)
            {
                l.columnsJson = MacheteAdoContext.getUIColumnsJson(l.sqlquery, connectionString);
            }
            frb.ToFactory().SaveChanges();
        }
    }
}
