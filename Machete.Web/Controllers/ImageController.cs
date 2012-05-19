using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
//using Machete.Helpers;
using Machete.Service;
using Machete.Web.Helpers;
using System.Web.Routing;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ImageController : MacheteController
    {
        private readonly IImageService _serv;
        //
        // GET: /Image/
        public ImageController(IImageService imageService)
        {
            this._serv = imageService;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public FileContentResult GetImage(int ID)
        {
            if (ID != 0)
            {
                Image image = _serv.Get(ID);
                return File(image.ImageData, image.ImageMimeType, image.filename);
            }
            else
            {
                return null; //File(new byte [1], "");
            }
        }
    }
}
