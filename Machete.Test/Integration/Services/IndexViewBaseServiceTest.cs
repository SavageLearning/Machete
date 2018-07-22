#region COPYRIGHT
// File:     IndexViewBaseServiceTest.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Test
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Machete.Test.Integration.Service
{
    [TestClass]
    public class IvbFluentRecordBase
    {
        FluentRecordBase frb;
        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            frb = null;
        }
        /// <summary>
        /// 
        /// </summary>
        //[TestMethod]
        //public void Integration_WA_Service_GetIndexView_check_search_description()
        //{
        //    //
        //    //Act
        //    dOptions.search = "foostring1";
        //    dOptions.woid = 1;
        //    dOptions.orderDescending = true;
        //    var result = _waServ.GetIndexView(dOptions);
        //    //
        //    //Assert
        //    var tolist = result.query.ToList();
        //    Assert.IsNotNull(tolist, "return value is null");
        //    Assert.IsInstanceOfType(result, typeof(IEnumerable<WorkAssignment>));
        //    Assert.AreEqual("foostring1", tolist[0].description);
        //    Assert.AreEqual(1, result.filteredCount);
        //    Assert.AreEqual(10, result.totalCount);
        //}

        [TestMethod, TestCategory(TC.Fluent)]
        public void activity_getUnassociated()
        {
            //Arrange
            var worker = frb.ToWorker();
            frb.AddActivity().AddActivity();
            frb.AddActivitySignin(worker: worker);

            IQueryable<Activity> q = frb.ToRepoActivity().GetAllQ();
            var count = q.Count();
            //Act
            IndexViewBase.getUnassociated(worker.ID, ref q, frb.ToFactory().Get());
            //Assert
            var result = q.ToList();
            Assert.AreEqual(count - 1, result.Count());
        }
    }
}
