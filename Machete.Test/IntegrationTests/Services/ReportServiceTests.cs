using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using Machete.Web.Helpers;

namespace Machete.Test
{
    [TestClass]
    class ReportServiceTests
    {
        viewOptions dOptions;
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
            frb.Initialize(new MacheteInitializer(), "macheteConnection");
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

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.Reports)]
        public void Integration_Reports_Service_GetSummary()
        {
            //
            //Arrange
            using (var ctx = MacheteContext
                .CreateTracingContext("macheteStageProd", 
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
