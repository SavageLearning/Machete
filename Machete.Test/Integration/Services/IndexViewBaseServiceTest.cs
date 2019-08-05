#region COPYRIGHT
// File:     IndexViewBaseServiceTest.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Test.Old
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

using System.Linq;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class IvbFluentRecordBase
    {
        FluentRecordBase frb;
        [TestInitialize]
        public void TestInitialize()
        {
            frb = FluentRecordBaseFactory.Get();
        }

        [TestMethod, TestCategory(TC.Fluent)]
        public void activity_getUnassociated()
        {
            //Arrange
            var worker = frb.AddWorker();
            frb.AddActivity().AddActivity();
            frb.AddActivitySignin(worker);

            IQueryable<Activity> q = frb.ToFactory().Activities;
            var count = q.Count();
            //Act
            IndexViewBase.getUnassociated(worker.ID, ref q, frb.ToFactory());
            //Assert
            var result = q.ToList();
            Assert.AreEqual(count - 1, result.Count);
        }
        
//        [TestCleanup]
//        public void TestCleanup()
//        {
//            frb = null;
//        }
    }
}
