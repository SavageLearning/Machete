using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Machete.Api.ViewModel;
using System.Collections.Generic;

namespace Machete.Api.Controllers
{
    public class EventController : MacheteApiController<Event, EventVM, EventListVM>
    {
        private new readonly IEventService service;
        private readonly IImageService _imageServ;

        public EventController(IEventService serv,
            IImageService imageServ,
            IMapper map) : base(serv, map)
        {
            service = serv;
            _imageServ = imageServ;
        }

        [HttpGet, Authorize(Roles = "Manager, Administrator")]
        public new ActionResult<IEnumerable<EventListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams
            )
        {
            return base.Get(apiRequestParams);
        }

        // GET api/values/5
        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager")]
        public new ActionResult<EventVM> Get(int id) { return base.Get(id); }

        //
        // POST: /Event/Create
        [HttpPost, Authorize(Roles = "Manager, Administrator")]
        public new ActionResult<EventVM> Post(EventVM evnt)
        {
            return base.Post(evnt);
        }

        // POST: /Event/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{id}")]
        public new ActionResult<EventVM> Put(int id, [FromBody] EventVM evnt) { return base.Put(id, evnt); }


        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<ActionResult> AddImage(int id, string userName, IFormFile imagefile)
        {
            if (imagefile == null) throw new MacheteNullObjectException("AddImage called with null imagefile");

            var joiner = new JoinEventImage();

            var evnt = service.Get(id);

            var image = new Image();
            image.ImageMimeType = imagefile.ContentType;
            image.parenttable = "Events";
            image.filename = imagefile.FileName;
            image.recordkey = id.ToString();
            using (var stream = new MemoryStream())
            {
                await imagefile.CopyToAsync(stream);
                image.ImageData = stream.ToArray();
            }

            // This should be unnecessary; image will have ID
            Image newImage = _imageServ.Create(image, userName);

            joiner.ImageID = newImage.ID;
            joiner.EventID = evnt.ID;
            joiner.datecreated = DateTime.Now;
            joiner.dateupdated = DateTime.Now;
            joiner.updatedby = userName;
            joiner.createdby = userName;

            service.JoinEventImages(evnt, joiner, userName);

            service.Save(evnt, userName);

            return Ok();
        }

        // GET: /Event/Delete/5
        [HttpDelete("{id}"), Authorize(Roles = "Administrator, Manager")]
        public new ActionResult Delete(int id) { return base.Delete(id); }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteImage(int evntID, int jeviID)
        {
            var evnt = service.Get(evntID);
            var joinEventImage = evnt.JoinEventImages.Single(e => e.ID == jeviID);
            var deletedImageId = joinEventImage.ID;
            _imageServ.Delete(joinEventImage.ImageID, UserEmail);
            evnt.JoinEventImages.Remove(joinEventImage);

            return Ok();
        }
    }
}
