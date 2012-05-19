using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;

namespace Machete.Service
{
    public interface IImageService : IService<Image> {}
    public class ImageService : ServiceBase<Image>, IImageService
    {
        //
        public ImageService(IImageRepository iRepo, IUnitOfWork uow) : base(iRepo, uow)
        {
            this.logPrefix = "Image";
        }
    }
}
