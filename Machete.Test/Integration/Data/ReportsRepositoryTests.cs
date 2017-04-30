using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
        public void getJobDispatchedCount_returns_list()
        {
            // arrange
            frb.AddWorkOrder(dateTimeOfWork: DateTime.Parse("1/2/2013"))
                .AddWorkAssignment(skill: 61); // known skill ID from machete lookup initializer

            // act
            var result = frb.ToRepoReports()
                .getSimpleAggregate(1, DateTime.Parse("2013/1/1"), 
                                        DateTime.Parse("2014/1/1"));
            // assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
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
        public void getFoo()
        {
            // arrange

            // act

            // assert

        }
    }
}
