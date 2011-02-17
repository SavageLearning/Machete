using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web;
using Machete.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace Machete.Test
{
    [TestFixture]
    public class CategoryControllerTest
    {
        Mock<ICategoryRepository> _repository;
        ICategoryService _service;
        IUnitOfWork _unitofwork;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repository.Object, _unitofwork);
        }

        [Test]
        public void List_Categories()
        {
            IQueryable<Category> fakeCategories = new List<Category> {
                new Category { Name = "Test1", Description="Test1Desc"},
                new Category { Name = "Test2", Description="Test2Desc"},
                new Category { Name = "Test2", Description="Test2Desc"}
            }.AsQueryable();

            
            _repository.Setup(x => x.GetAll()).Returns(fakeCategories);
            
            CategoryController controller = new CategoryController(_service);
            // Act
            //ViewResult result = controller.List(null) as ViewResult;
            // Assert
            //Assert.IsNotNull(result, "View Result is null");
            //Assert.IsInstanceOf(typeof(PagedList<Category>), result.ViewData.Model, "Wrong ViewModel");
            //var categories = result.ViewData.Model as PagedList<Category>;
            //Assert.AreEqual(3, categories.Count, "Got wrong number of Categories");
            //Assert.AreEqual(0, (int)categories.PageIndex, "Wrong page Index");
            //Assert.AreEqual(1, (int)categories.PageNumber, "Wrong  page Number");
        
        }
    }
}
