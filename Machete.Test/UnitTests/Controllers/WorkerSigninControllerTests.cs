#region COPYRIGHT
// File:     WorkerSigninControllerTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
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
using Moq;
using Machete.Data;
using Machete.Service;
using Machete.Data.Infrastructure;

namespace Machete.Test.Unit.Controller
{
    [TestClass]
    public class WorkerSigninTests
    {
        Mock<IWorkerSigninService> _sserv ;
        Mock<IWorkerService> _wserv;
        Mock<IPersonService> _pserv;

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.WSIs)]
        public void getView_finds_joined_records()
        {
            // TODO: Make this do something
            //arrange
            _sserv = new Mock<IWorkerSigninService>();
            _wserv = new Mock<IWorkerService>();
            _pserv = new Mock<IPersonService>();
            //_service = new WorkerSigninService(_signinRepo.Object, _unitofwork);

        }
    }
}
