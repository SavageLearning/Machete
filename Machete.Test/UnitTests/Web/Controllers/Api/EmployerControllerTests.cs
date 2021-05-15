using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers.Api;
using Machete.Web.Maps.Api;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employer = Machete.Domain.Employer;

namespace Machete.Test.UnitTests.Controllers.Api
{
    [TestClass]
    public class EmployerControllerTests
    {
        private Mock<IEmployerService> _employerServ;
        private Mock<IWorkOrderService> _workerServ;
        private EmployersController _controller;
        private List<Service.DTO.EmployersList> _fakeEmployers = new List<Service.DTO.EmployersList>();
        private Employer _fakeEmployer;
        private Employer _savedEmployer;
        private Service.DTO.EmployersList _fakeEmployersListObject;
        private IMapper _mapper;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _savedEmployer = new Employer();
            _fakeEmployer = new Employer()
            {
                ID = 1,
                name = "Jimmy Hendrix",
                address1 = "123 Yellow Sub",
                phone = "123-123-1234",
                city = "seattle",
                state = "wa",
                email = "jciispam@gmail.com"
            };
            _fakeEmployersListObject = new Service.DTO.EmployersList()
            {
                active = true,
                address1 = "123 Machete ave",
                city = "Seattle",
                name = "Dolores Huerta"
            };
            _fakeEmployers.Add(_fakeEmployersListObject);
            _fakeEmployers.Add(new Service.DTO.EmployersList()
            {
                active = true,
                address1 = "123 Test ave",
                city = "Seattle",
                name = "Cesar Chavez"
            });
            
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.ConfigureApi();
                // config.AddProfile<Web.Maps.EmployerProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _employerServ = new Mock<IEmployerService>();
            _workerServ = new Mock<IWorkOrderService>();

            _employerServ.Setup(s => s.GetIndexView(It.IsAny<viewOptions>()))
                .Returns(new dataTableResult<Service.DTO.EmployersList>() {query = _fakeEmployers.AsEnumerable()});
            _employerServ.Setup(s => s.Get(1000))
                .Returns((Employer) null);
            _employerServ.Setup(s => s.Get(1))
                .Returns(_fakeEmployer);
            _employerServ.Setup(s => s.Create(It.IsAny<Employer>(), It.IsAny<string>()))
                .Returns(_fakeEmployer);
            _employerServ.Setup(s =>
                    s.Save(It.Is<Employer>(employer => employer.name == "Jimmy Hendrix"), It.IsAny<string>()))
                .Callback((Employer e, string s) => _savedEmployer = e);

            _controller = new EmployersController(_employerServ.Object, _workerServ.Object, _mapper);
        }

        #region GetMany

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void GetMany_Action_returns_Ok_Result()
        {
            // act
            var result = _controller.Get(new ApiRequestParams());
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void GetMany_with_existing_returns_all_records_of_type()
        {
            // Act
            var result = _controller.Get(new ApiRequestParams() {pageNumber = 1, pageSize = 10});
            var typedResult = (result.Result as ObjectResult).Value;
            var employersVMList = UnitTestExtensions.ExtractFromDataObject<IEnumerable<Service.DTO.EmployersList>>(typedResult);
            // Assert
            Assert.IsTrue(employersVMList.Count() == _fakeEmployers.Count);
            Assert.IsInstanceOfType(employersVMList, typeof(IEnumerable<Service.DTO.EmployersList>));
        }
        
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void GetMany_with_existing_returns_records_in_data_prop()
        {
            // Act
            var result = _controller.Get(new ApiRequestParams() {pageNumber = 1, pageSize = 10});
            var typedResult = (result.Result as ObjectResult).Value;
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;

            // Assert
            Assert.IsTrue(resultHasDataProp);
        }


        #endregion GetMany
        #region GetOne

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Get_by_unknown_ID_returns_not_found_result()
        {
            // Act
            var result = _controller.Get(1000);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Get_by_ID_existing_returns_OkResult()
        {
            // Act
            var result = _controller.Get(1);
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Get_by_existing_Id_returns_correct_item()
        {
            // Act
            var result = _controller.Get(1);
            var typedResult = (result.Result as ObjectResult).Value;
            var data = UnitTestExtensions.ExtractFromDataObject<EmployerVM>(typedResult);
            // assert
            Assert.IsInstanceOfType(data, typeof(EmployerVM));
            Assert.AreEqual(_fakeEmployer.ID, data.id);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Get_by_Id_pass_existing_id_returns_record_in_data_object()
        {
            // Act
            var result = _controller.Get(1);
            var typedResult = (result.Result as ObjectResult).Value;
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;

            // Assert
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion GetOne
        
        #region Post

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Post_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidRecord = new EmployerVM();
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Post(invalidRecord);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Post_valid_data_returns_created_at_route()
        {
            // arrange
            var validViewModel = new EmployerVM() { name = "Peter Parker"};
            // act
            var result = _controller.Post(validViewModel);
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Post_valid_data_returns_new_record_in_data_oject()
        {
            // arrange
            var validViewModel = new EmployerVM() {name = "Jimmy Hendrix"};
            // act
            var result = _controller.Post(validViewModel);
            var typedResult = (result.Result as ObjectResult).Value;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<EmployerVM>(typedResult);
            var resultHasDataProp = typedResult.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(returnedViewModel, typeof(EmployerVM));
            Assert.IsTrue(resultHasDataProp);
        }

        #endregion Post  
        
        #region PUT

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Put_invalid_data_returns_bad_request()
        {
            // Arrange
            var invalidViewModel = new EmployerVM();
            _controller.ModelState.AddModelError("value", "Required");
            // Act
            var result = _controller.Put(invalidViewModel.id, invalidViewModel);
            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Put_valid_data_returns_ok_result_and_updated_record()
        {
            // arrange
            var validViewModel = _mapper.Map<EmployerVM>(_fakeEmployer);
            validViewModel.id = 0;
            // act
            var result = _controller.Put(1, validViewModel);
            var parsedObject = (result.Result as ObjectResult)?.Value;
            var returnedViewModel = UnitTestExtensions.ExtractFromDataObject<EmployerVM>(parsedObject);
            var resultHasDataProp = parsedObject?.GetType().GetProperty("data") != null;
            //assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(_fakeEmployer, _savedEmployer);
            Assert.AreEqual("Jimmy Hendrix", _savedEmployer.name);
            Assert.AreEqual("123 Yellow Sub", _savedEmployer.address1);
            Assert.IsInstanceOfType(returnedViewModel, typeof(EmployerVM));
            Assert.IsTrue(resultHasDataProp);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Put_invalid_id_returns_not_found()
        {
            // arrange
            // act
            var deleteResult = _controller.Put(1000,  new EmployerVM());
            // assert
            Assert.IsInstanceOfType(deleteResult.Result, typeof(NotFoundResult));
        }

        #endregion PUT
        
        #region Delete

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Delete_non_existing_record_returns_not_found()
        {
            // arrange
            _employerServ.Setup(s => s.Delete(1000, It.IsAny<string>()))
                .Verifiable();
            // act
            var deleteResult = _controller.Delete(1000);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(NotFoundResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Delete_existing_record_returns_ok()
        {
            var deleteResult = _controller.Delete(1);
            // assert
            Assert.IsInstanceOfType(deleteResult, typeof(OkResult));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Employers)]
        public void Delete_existing_item_removes_record()
        {
            // act
            _controller.Delete(1);
            // assert
            _employerServ.Verify(s => s.Delete(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        #endregion Delete

        #region EmployerProfile
        private void ArrangeDifferentGuidsForUserAndEmployer()
        {
            // arrange
            var userGuidString = "9245fe4a-d402-451c-b9ed-9c1a04247482";
            var employerGuidString = "9245fe4a-d402-451c-b9ed-9c1a04268482";
            var fakeEmployersD = new List<Employer>();
            _fakeEmployer.onlineSigninID = employerGuidString;
            fakeEmployersD.Add(_fakeEmployer);
            _employerServ.Setup(s => s.Get(userGuidString))
                .Returns((Employer) null);
            _employerServ.Setup(s => s.GetMany(It.IsAny<Func<Employer, bool>>()))
                .Returns(fakeEmployersD);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "jciispam@gmail.com"),
                new Claim(ClaimTypes.NameIdentifier, (new Guid(userGuidString)).ToString()),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        private void ArrangeForEmployerFound()
        {
            // arrange
            var fakeEmployersD = new List<Employer>();
            fakeEmployersD.Add(_fakeEmployer);
            _employerServ.Setup(s => s.Get("9245fe4a-d402-451c-b9ed-9c1a04247482"))
                .Returns((Employer) null);
            _employerServ.Setup(s => s.GetMany(It.IsAny<Func<Employer, bool>>()))
                .Returns(fakeEmployersD);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "jciispam@gmail.com"),
                new Claim(ClaimTypes.NameIdentifier, (new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")).ToString()),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        private void ArrangeForEmployerNotFound()
        {
            // arrange
            var fakeEmployersD = new List<Employer>();
            _employerServ.Setup(s => s.Get("9245fe4a-d402-451c-b9ed-9c1a04247482"))
                .Returns((Employer) null);
            _employerServ.Setup(s => s.GetMany(It.IsAny<Func<Employer, bool>>()))
                .Returns(fakeEmployersD);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "jciispam@gmail.com"),
                new Claim(ClaimTypes.NameIdentifier, (new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482")).ToString()),
            }, "mock"));
            
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [TestMethod]
        public void ProfileGet_finds_no_employer_returns_not_found()
        {
            // arrange
            ArrangeForEmployerNotFound();
            // act
            var result = _controller.ProfileGet();
            // assert 
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        
        
        [TestMethod]
        public void ProfileGet_finds_employer_returns_profile()
        {
            // arrange
            ArrangeForEmployerFound();
            // act
            var result = _controller.ProfileGet();
            var typedResult = (result.Result as ObjectResult).Value;
            var data = UnitTestExtensions.ExtractFromDataObject<EmployerVM>(typedResult);
            
            // assert 
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.IsTrue(data.email == "jciispam@gmail.com");
        }
        
        [TestMethod]
        public void ProfileGet_finds_employer_with_different_onlineSigninID_returns_Conflict()
        {
            // arrange
            ArrangeDifferentGuidsForUserAndEmployer();
            // act
            var result = _controller.ProfileGet();
            
            // assert 
            Assert.IsInstanceOfType(result.Result, typeof(ConflictResult));
        }
        
        [TestMethod]
        public void ProfilePost_employer_exists_with_same_onlineid_returns_conflict()
        {
            // arrange
            ArrangeForEmployerFound();
            // act
            var result = _controller.ProfilePost(_mapper.Map<EmployerVM>(_fakeEmployer));
            
            // assert 
            Assert.IsInstanceOfType(result.Result, typeof(ConflictResult));
        }

        [TestMethod]
        public void ProfilePost_new_employer_returns_okResult()
        {
            // arrange
            ArrangeForEmployerNotFound();
            // act
            var result = _controller.ProfilePost(_mapper.Map<EmployerVM>(_fakeEmployer));
            // assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        }
        #endregion 
    }
}
