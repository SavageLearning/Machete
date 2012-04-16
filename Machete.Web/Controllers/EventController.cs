using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
using Machete.Data;
using Machete.Service;
using Machete.Web.Helpers;
using NLog;
using Machete.Web.Models;
using System.Web.Routing;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EventController : MacheteController
    {
        private readonly IEventService _serv;
        private readonly IImageService iServ;
        System.Globalization.CultureInfo CI;
        //
        //
        public EventController(IEventService eventService, IImageService imageServ)
        {
            this._serv = eventService;
            this.iServ = imageServ;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (System.Globalization.CultureInfo)Session["Culture"];
        }
        //
        //
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            System.Globalization.CultureInfo CI = (System.Globalization.CultureInfo)Session["Culture"];
            //Get all the records
            var allEvents = _serv.GetEvents();
            IEnumerable<Event> filteredEvents;
            IEnumerable<Event> sortedEvents;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredEvents = _serv.GetEvents()
                    .Where(p => p.notes.ToString().Contains(param.sSearch));
            }
            else
            {
                filteredEvents = allEvents;
            }
            //Sort the Persons based on column selection
            var sortColIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Event, string> orderingFunction = (p => p.dateupdated.ToBinary().ToString());

            var sortDir = Request["sSortDir_0"];
            if (sortDir == "asc")
                sortedEvents = filteredEvents.OrderBy(orderingFunction);
            else
                sortedEvents = filteredEvents.OrderByDescending(orderingFunction);

            //Limit results to the display length and offset
            var displayEvents = sortedEvents.Skip(param.iDisplayStart)
                                                .Take(param.iDisplayLength);

            //return what's left to datatables
            var result = from p in displayEvents
                         select new
                         {
                             tabref = _getTabRef(p),
                             tablabel = _getTabLabel(p, CI.TwoLetterISOLanguageName),
                             recordid = p.ID,
                             notes = p.notes,
                             datefrom = p.dateFrom.ToShortDateString(),
                             dateto = p.dateTo == null ? "" : ((DateTime)p.dateTo).ToShortDateString(),
                             fileCount = p.JoinEventImages.Count(),
                             type = LookupCache.byID(p.eventType, CI.TwoLetterISOLanguageName),
                             dateupdated = Convert.ToString(p.dateupdated),
                             Updatedby = p.Updatedby
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allEvents.Count(),
                iTotalDisplayRecords = filteredEvents.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        //
        private string _getTabRef(Event evnt)
        {
            return "/Event/Edit/" + Convert.ToString(evnt.ID);
        }
        //
        //
        private string _getTabLabel(Event evnt, string locale)
        {
            return evnt.dateFrom.ToShortDateString() + " " + LookupCache.byID(evnt.eventType, locale);
        }
        //
        // GET: /Event/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(int PersonID)
        {
            Event eventobj = new Event();
            eventobj.dateFrom = DateTime.Today;
            eventobj.dateTo = DateTime.Today;
            eventobj.PersonID = PersonID;
            return PartialView(eventobj);
        }

        //
        // POST: /Event/Create
        [HttpPost, UserNameFilter]
        public ActionResult Create(Event evnt, string userName)
        {
            UpdateModel(evnt);
            Event newEvent = _serv.CreateEvent(evnt, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newEvent),
                sNewLabel = _getTabLabel(newEvent, CI.TwoLetterISOLanguageName),
                iNewID = newEvent.ID
            },
            JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Event/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var evnt = _serv.GetEvent(id);
            return PartialView("Edit", evnt);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, string user)
        {
            Event evnt = _serv.GetEvent(id);
            UpdateModel(evnt);
            _serv.SaveEvent(evnt, user);
  
            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }
        //
        // AddImage
        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AddImage(int id, string user, HttpPostedFileBase imagefile)
        {
            if (imagefile == null) throw new MacheteNullObjectException("AddImage called with null imagefile");
            JoinEventImage joiner = new JoinEventImage();
            Image image = new Image();
            Event evnt = _serv.GetEvent(id);
            image.ImageMimeType = imagefile.ContentType;
            image.parenttable = "Events";
            image.filename = imagefile.FileName;
            image.recordkey = id.ToString();
            image.ImageData = new byte[imagefile.ContentLength];
            imagefile.InputStream.Read(image.ImageData,
                                       0,
                                       imagefile.ContentLength);
            Image newImage = iServ.CreateImage(image, user);
            joiner.ImageID = newImage.ID;
            joiner.EventID = evnt.ID;
            joiner.datecreated = DateTime.Now;
            joiner.dateupdated = DateTime.Now;
            joiner.Updatedby = user;
            joiner.Createdby = user;
            evnt.JoinEventImages.Add(joiner);
            _serv.SaveEvent(evnt, user);
            var foo = iServ.GetImage(newImage.ID).ImageData;
            //_serv.GetEvent(evnt.ID);
            
            return Json(new
            {
                status = "OK"                
            },
            JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Event/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {   
            _serv.DeleteEvent(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteImage(int evntID, int jeviID, string user)
        {
            int deletedJEVI = 0;
            Event evnt = _serv.GetEvent(evntID);
            JoinEventImage jevi = evnt.JoinEventImages.Single(e => e.ID == jeviID);
            deletedJEVI = jevi.ID;
            iServ.DeleteImage(jevi.ImageID, user);
            evnt.JoinEventImages.Remove(jevi);

            return Json(new
            {
                status = "OK",
                deletedID = deletedJEVI
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
