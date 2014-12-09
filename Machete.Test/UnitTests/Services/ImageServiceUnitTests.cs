#region COPYRIGHT
// File:     ImageServiceUnitTests.cs
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
using Machete.Data;
using Moq;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Domain;
using Machete.Test;

namespace Machete.Test.UnitTests.Services
{
    /// <summary>
    /// Summary description for ImageServiceUnitTests
    /// </summary>
    [TestClass]
    public class ImageServiceUnitTests
    {
        Mock<IImageRepository> _repo;
        Mock<IUnitOfWork> _uow;
        
        public ImageServiceUnitTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Images)]
        public void ImageService_GetImages_returns_Enumerable()
        {
            //
            //Arrange
            _repo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
            var _serv = new ImageService(_repo.Object, _uow.Object);
            //Act
            var result = _serv.GetAll();
            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Image>));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Images)]
        public void ImageService_GetImage_returns_image()
        {
            //
            //Arrange
            _repo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
            int id = 1;
            Image _img = new Image() {ID = id};
         
            _repo.Setup(r => r.GetById(_img.ID)).Returns(_img);
            var _serv = new ImageService(_repo.Object, _uow.Object);
            //Act
            var result = _serv.Get(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(Image));
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Images)]
        public void ImageService_CreateImage_returns_image()
        {
            //
            //Arrange
            _repo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Image _img = new Image();
            _img.datecreated = DateTime.MinValue;
            _img.dateupdated = DateTime.MinValue;
            _repo.Setup(r => r.Add(_img)).Returns(_img);
            var _serv = new ImageService(_repo.Object, _uow.Object);
            //
            //Act
            var result = _serv.Create(_img, user);
            //
            //Assert
            Assert.IsInstanceOfType(result, typeof(Image));
            Assert.IsTrue(result.Createdby == user);
            Assert.IsTrue(result.Updatedby == user);
            Assert.IsTrue(result.datecreated > DateTime.MinValue);
            Assert.IsTrue(result.dateupdated > DateTime.MinValue);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Images)]
        public void ImageService_DeleteImage()
        {
            //
            //Arrange
            _repo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            int id = 1;
            Image di = new Image();
            Image _rtrn_img = new Image();
            _repo.Setup(r => r.Delete(It.IsAny<Image>())).Callback((Image p) => { di = p; });
            _repo.Setup(r => r.GetById(id)).Returns(_rtrn_img);
            var _serv = new ImageService(_repo.Object, _uow.Object);
            //
            //Act
            _serv.Delete(id, user);
            //
            //Assert
            Assert.AreEqual(di, _rtrn_img);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Images)]
        public void ImageService_SaveImage_updates_timestamp()
        {
            //
            //Arrange
            _repo = new Mock<IImageRepository>();
            _uow = new Mock<IUnitOfWork>();
            string user = "UnitTest";
            Image _rtrn_img = new Image();
            _rtrn_img.datecreated = DateTime.MinValue;
            _rtrn_img.dateupdated = DateTime.MinValue;
            var _serv = new ImageService(_repo.Object, _uow.Object);
            //
            //Act
            _serv.Save(_rtrn_img, user);
            //
            //Assert
            Assert.IsTrue(_rtrn_img.Updatedby == user);
            Assert.IsTrue(_rtrn_img.dateupdated > DateTime.MinValue);
        }
    }
}
