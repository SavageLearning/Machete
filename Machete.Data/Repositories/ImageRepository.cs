using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IImageRepository : IRepository<Image>
    {
    }
}
