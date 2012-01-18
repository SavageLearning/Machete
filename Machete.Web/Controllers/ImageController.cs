using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Helpers;
using Machete.Service;
using Machete.Web.Helpers;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class ImageController : Controller
    {
        private readonly IImageService _serv;
        //
        // GET: /Image/
        public ImageController(IImageService imageService)
        {
            this._serv = imageService;
        }
        public ActionResult Index()
        {
            var images = _serv.GetImages();
            return View(images);
        }

        public FileContentResult GetImage(int ID)
        {
            if (ID != 0)
            {
                Image image = _serv.GetImage(ID);
                return File(image.ImageData, image.ImageMimeType);
            }
            else
            {
                return null; //File(new byte [1], "");
            }
        }

        //
        // GET: /Image/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Image/Create

        public ActionResult Create()
        {
            Image imageobj = new Image();
            return View(imageobj);
        } 

        //
        // POST: /Image/Create

        [HttpPost]
        public ActionResult Create(Image image, HttpPostedFileBase imagefile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imagefile != null)
                    {
                        image.ImageMimeType = imagefile.ContentType;
                        image.ImageData = new byte[imagefile.ContentLength];
                        imagefile.InputStream.Read(image.ImageData, 
                                                   0, 
                                                   imagefile.ContentLength);
                        _serv.CreateImage(image, this.User.Identity.Name);        
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Image/Edit/5
 
        public ActionResult Edit(int id)
        {
            var image = _serv.GetImage(id);
            return View(image);
        }

        //
        // POST: /Image/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //
        // POST: /Image/Delete/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {
            string status = null;
            try
            {
                _serv.DeleteImage(id, user);
            }
            catch (Exception e)
            {
                status = RootException.Get(e, "ImageService");
            }

            return Json(new
            {
                status = status ?? "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
