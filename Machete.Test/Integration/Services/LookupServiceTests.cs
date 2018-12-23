using Machete.Service;
using DTO = Machete.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class LookupTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;
        [ClassInitialize]
        public static void ClassInitialize(TestContext c)
        {
            //EntityFrameworkProfiler.Initialize();

        }

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
                displayLength = 20,
                category = "activityName"
            };
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Lookups)]
        public void search_for_any_result()
        {
            //
            // Arrange
            
            // Act
            IEnumerable<DTO.LookupList> result = frb.ToServ<ILookupService>().GetIndexView(dOptions);
            // Assert
            Assert.IsTrue(result.Count() > 0, "LookupService.GetIndexReview returned null");

        }
    }
}
