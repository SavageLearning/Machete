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
    public interface IImageService
    {
        IEnumerable<Image> GetImages();
        Image GetImage(int id);
        Image CreateImage(Image image, string user);
        void DeleteImage(int id, string user);
        void SaveImage(Image image, string user);
    }

    // Business logic for Image record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "ImageService", "");
        private Image _image;
        //
        public ImageService(IImageRepository imageRepository, IUnitOfWork unitOfWork)
        {
            this.imageRepository = imageRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Image> GetImages()
        {
            var images = imageRepository.GetAll();
            return images;
        }

        public Image GetImage(int id)
        {
            var image = imageRepository.GetById(id);
            return image;
        }

        public Image CreateImage(Image image, string user)
        {
            image.createdby(user);
            _image = imageRepository.Add(image);
            unitOfWork.Commit();
            _log(image.ID, user, "Image created");
            return _image;
        }

        public void DeleteImage(int id, string user)
        {
            var image = imageRepository.GetById(id);
            imageRepository.Delete(image);
            _log(id, user, "Image deleted");
            unitOfWork.Commit();
        }

        public void SaveImage(Image image, string user)
        {
            image.updatedby(user);
            _log(image.ID, user, "Image edited");
            unitOfWork.Commit();
        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
    }
}
