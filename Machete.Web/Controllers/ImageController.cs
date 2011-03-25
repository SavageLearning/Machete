using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;


namespace Machete.Web.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        MacheteContext DB = new MacheteContext();
        public ActionResult Index()
        {
            var images = DB.Images.AsEnumerable();
            return View(images);
        }

        public FileContentResult GetImage(int ID)
        {
            Image image = (from i in DB.Images.AsEnumerable()
                           where i.ID == ID
                           select i).First();
            return File(image.ImageData, image.ImageMimeType);
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
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (imagefile != null)
                    {
                        image.ImageMimeType = imagefile.ContentType;
                        image.ImageData = new byte[imagefile.ContentLength];
                        imagefile.InputStream.Read(image.ImageData, 
                                                   0, 
                                                   imagefile.ContentLength);
                        image.createdby(this.User.Identity.Name);
                        DB.Images.Add(image);
                        DB.SaveChanges();

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
            var image = DB.Images.Find(id);
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
        // GET: /Image/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Image/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
