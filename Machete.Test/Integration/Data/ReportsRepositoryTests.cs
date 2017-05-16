using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Machete.Domain;
using Machete.Data.Helpers;

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
                .getDynamicQuery(1, DateTime.Parse("2013/1/1"),
                                        DateTime.Parse("2013/3/1"));
            // act
            // assert
            Assert.AreEqual("general labor", result[0].label);
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
            var list = repo.GetAll();
            foreach (var l in list)
            {
                l.columnsJson = SqlServerUtils.getColumnJson(ctxt, l.sqlquery);
            }
            frb.ToFactory().Get().Commit();

        }
    }
}
