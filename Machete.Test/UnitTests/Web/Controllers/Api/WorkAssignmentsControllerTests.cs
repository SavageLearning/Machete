using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Api.Controllers;
using Machete.Api.Maps;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class WorkAssignmentsControllerTests
    {
        private Mock<IWorkAssignmentService> _waServ;
        private IMapper _mapper;
        private List<WorkAssignment> _fakeWorkAssignments;
        private WorkAssignment _fakeWorkAssignment;
        private WorkAssignment _savedWorkAssignment;
        private WorkAssignmentsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _savedWorkAssignment = new WorkAssignment();
            _fakeWorkAssignment = new WorkAssignment()
            {
                active = true,
                ID = 1,
                hourRange = 9,
                hours = 5,
                weightLifted = true,
                hourlyWage = 65,
                description = "asdfasdf"
            };
            _fakeWorkAssignments = new List<WorkAssignment>();
            _fakeWorkAssignments.Add(new WorkAssignment()
            {
                active = true,
                ID = 2,
                hourRange = 8,
                hours = 3,
                weightLifted = false,
                hourlyWage = 10,
                description = "rghj"
            });

            _waServ = new Mock<IWorkAssignmentService>();
            _waServ.Setup(s => s.GetAll())
                .Returns(_fakeWorkAssignments.AsQueryable);
            _waServ.Setup(s => s.Get(1))
                .Returns(_fakeWorkAssignment);
            _waServ.Setup(s => s.Get(1000))
                .Returns((WorkAssignment)null);
            _waServ.Setup(s => s.Create(It.IsAny<WorkAssignment>(), It.IsAny<string>()))
                .Returns(_fakeWorkAssignment);
            _waServ.Setup(s => s.Save(It.Is<WorkAssignment>(r => r.ID == 1), It.IsAny<string>()))
                .Callback((WorkAssignment wa, string user) => _savedWorkAssignment = wa);
            _waServ.Setup(s => s.Delete(1, It.IsAny<string>()))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new WorkAssignmentsController(_waServ.Object, _mapper);
        }

        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get(new ApiRequestParams());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void GetMany_with_existing_returns_all_records_of_type_in_data_object()
        {
            // Act
            var result = _controller.Get(new ApiRequestParams()).Result as ObjectResult;
            var viewModelList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<WorkAssignmentVM>>(result?.Value);
            // Assert
            Assert.IsTrue(viewModelList.Count() == _fakeWorkAssignments.Count);
            Assert.IsInstanceOfType(viewModelList, typeof(IEnumerable<WorkAssignmentVM>));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void GetMany_service_exception_returns_server_error()
        {
            // arrange
            _waServ.Setup(s => s.GetAll()).Throws(new Exception());
            // Act
            var result = _controller.Get(new ApiRequestParams()).Result as ObjectResult;
            // Assert
            Assert.IsTrue(result?.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Get_by_existing_Id_returns_correct_item_in_data_obj()
        {
            // Act
            var result = _controller.Get(1).Result as ObjectResult;
            var data = UnitTestExtensions.ExtractFromDataObject<WorkAssignmentVM>(result?.Value);
            // assert
            Assert.IsInstanceOfType(data, typeof(WorkAssignmentVM));
            Assert.AreEqual(_fakeWorkAssignment.ID, data.id);
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion GetOne
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new WorkAssignmentVM();
            _controller.ModelState.AddModelError("hourlyWage", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new WorkAssignmentVM();
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Post_valid_data_returns_new_record_in_data_object()
        {
            // arrange
            var validViewModel = new WorkAssignmentVM();
            // act
            var result = _controller.Post(validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<WorkAssignmentVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(WorkAssignmentVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        #endregion Post
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new WorkAssignmentVM();
            _controller.ModelState.AddModelError("hourlyWage", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<WorkAssignmentVM>(_fakeWorkAssignment);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel).Result as ObjectResult;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<WorkAssignmentVM>(result?.Value);
            //assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeWorkAssignment, _savedWorkAssignment);
            Assert.AreEqual(_fakeWorkAssignment.ID, _savedWorkAssignment.ID);
            Assert.AreEqual(_fakeWorkAssignment.hours, _savedWorkAssignment.hours);
            Assert.AreEqual(_fakeWorkAssignment.description, _savedWorkAssignment.description);
            Assert.IsInstanceOfType(returnedViewModel, typeof(WorkAssignmentVM));
            Assert.IsTrue(UnitTestExtensions.HasDataProperty(result));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Put_invalid_id_returns_not_found()
        {
            // act
            var result = _controller.Put(1000, new WorkAssignmentVM());
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WAs)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _waServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete
    }
}
