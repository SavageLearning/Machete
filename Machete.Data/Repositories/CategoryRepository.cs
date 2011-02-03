using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
namespace Machete.Data
{
    public class CategoryRepository: RepositoryBase<Category>, ICategoryRepository
        {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }           
        }
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}