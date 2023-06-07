using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Domain;
using Machete.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    public class ImageController : MacheteApiController<Image, ImageVM, ImageListVM>
    {
        // GET: /Image/
        public ImageController(IImageService serv, IMapper map) : base(serv, map)
        {
        }

        /// <summary>
        /// Gets an image from the database as file content.
        /// </summary>
        /// <param name="ID">The database ID of the image to be retrieved.</param>
        /// <returns></returns>
        [HttpGet, Authorize(Roles = "PhoneDesk, Manager, Teacher, Administrator, Check-in")]
        public new FileContentResult Get(int ID)
        {
            if (ID == 0) return null;

            var image = service.Get(ID);
            return File(image.ImageData, image.ImageMimeType, image.filename);
        }
    }
}
