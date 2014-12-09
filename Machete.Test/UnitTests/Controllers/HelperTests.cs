#region COPYRIGHT
// File:     HelperTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Machete.Web.Helpers;
using Moq;
using Machete.Service;
using Machete.Data.Infrastructure;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class HelperTests
    {
        Mock<ILookupCache> lcache;
        Mock<IDatabaseFactory> dbfactory;

        [TestInitialize]
        public void TestInitialize()
        {
            lcache = new Mock<ILookupCache>();
            dbfactory = new Mock<IDatabaseFactory>();
            Lookups.Initialize(lcache.Object, dbfactory.Object);
        }

        [TestMethod]
        public void Call_Mapper_autoValidate()
        {
            MacheteMapper.Initialize();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
