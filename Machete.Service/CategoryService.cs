using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;

namespace Machete.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        void CreateCategory(Category category);
        void DeleteCategory(int id);
        void SaveCategory();
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }  
        #region ICategoryService Members

        public IEnumerable<Category> GetCategories()
        {
            var categories = categoryRepository.GetAll();
            return categories;
        }

        public Category GetCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            return category;
        }

        public void CreateCategory(Category category)
        {
            categoryRepository.Add(category);
            unitOfWork.Commit();
        }

        public void DeleteCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            categoryRepository.Delete(category);
            unitOfWork.Commit();
        }

        public void SaveCategory()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
