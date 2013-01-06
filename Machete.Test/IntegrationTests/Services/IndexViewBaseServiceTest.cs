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

namespace Machete.Test
{
    [TestClass]
    public class IVBServiceTest : ServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {

            Database.SetInitializer<MacheteContext>(new TestInitializer());
            base.Initialize();
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

        [TestMethod]
        public void Integration_IVB_activity_getUnassociated()
        {
            //Arrange
            int id = 1;
            
            IQueryable<Activity> q = _aRepo.GetAllQ();
            //Act
            IndexViewBase.getUnassociated(id, ref q, _aRepo, _asRepo);
            //Assert
            var result = q.ToList();
            Assert.IsTrue(result.Count() == 2, "Expected 2 unassociated activities, received {0}");
        }
    }
}
