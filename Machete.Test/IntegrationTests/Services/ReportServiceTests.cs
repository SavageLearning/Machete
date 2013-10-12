using Machete.Data;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;


namespace Machete.Test
{
    [TestClass]
    public class ReportServiceTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteDevTest");
            dOptions = new viewOptions
            {
                CI = new CultureInfo("en-US", false),
                sSearch = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = false,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }

        [TestMethod]//, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_Reports_Service_GetSummary()
        {
            //
            //Arrange
            using (var ctx = MacheteContext
                .CreateTracingContext("macheteDevTest", 
                                        x => {
                                            Console.WriteLine(x.ToFlattenedTraceString());
                                        }, 
                                        true, 
                                        @"c:\MyContext.output")
            )
            {
                var result = frb.AddWorkOrder().ToServReports().wecView(DateTime.Now).totalCount;

            }
            //
            //Act
            //
            //Assert

        }
    }
}
