using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Machete.Domain;
using Machete.Data.Helpers;
using Machete.Data.DTO;
using System.Data.Entity;
using System.Linq;

namespace Machete.Test.Integration.Data
{
    [TestClass]
    public class ReportsRepositoryTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void Initialize()
        {
            frb = new FluentRecordBase();
            //frb.AddDBFactory(connStringName: "MacheteConnection");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Data), TestCategory(TC.Reports)]
        public void getSimpleAggregate_returns_list()
        {
            // arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
                .AddWorkAssignment(skill: 63); // known skill ID from machete lookup initializer

            // act
            var result = frb.ToRepoReports()
                .getSimpleAggregate(1, DateTime.Parse("2013/1/1"), 
                                        DateTime.Parse("2014/1/1"));
            // assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
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
            var context = frb.ToFactory().Get();
            var reports = frb.ToFactory().Get().ReportDefinitions.AsQueryable();

            foreach (var r in reports)
            {
                var result = SqlServerUtils.getMetadata(context, r.sqlquery);
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
            var ctxt = frb.ToFactory().Get();
            var list = repo.GetAllQ();
            foreach (var l in list)
            {
                l.columnsJson = SqlServerUtils.getUIColumnsJson(ctxt, l.sqlquery);
            }
            frb.ToFactory().Get().SaveChanges();

        }
    }
}
