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
using NLog;
using Machete.Web.Models;


namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EventController : Controller
    {
        private readonly IEventService _serv;
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EmployerController", "");
        //
        //
        public EventController(IEventService eventService)
        {
            this._serv = eventService;
        }
        //
        //
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {
            //Get all the records
            var allEvents = _serv.GetEvents();
            IEnumerable<Event> filteredEvents;
            IEnumerable<Event> sortedEvents;
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredEvents = _serv.GetEvents()
                    .Where(p => p.notes.ToString().ContainsOIC(param.sSearch));
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
                             tablabel = _getTabLabel(p),
                             recordid = Convert.ToString(p.ID),
                             notes = p.notes,
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
        private string _getTabLabel(Event evnt)
        {
            return evnt.ID.ToString();
        }
        //
        // GET: /Event/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            Event eventobj = new Event();
            return PartialView(eventobj);
        }

        //
        // POST: /Event/Create
        [HttpPost, UserNameFilter]
        public ActionResult Create(Event evnt, string userName)
        {
            if (!ModelState.IsValid)
            {
                //TODO: This probably wont work for tabs & partials
                return PartialView("Create", evnt);
            }
            Event newEvent = _serv.CreateEvent(evnt, userName);

            return Json(new
            {
                sNewRef = _getTabRef(newEvent),
                sNewLabel = _getTabLabel(newEvent),
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
            return PartialView("Event", evnt);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, FormCollection collection, string userName)
        {
            Event evnt = _serv.GetEvent(id);

            if (TryUpdateModel(evnt))
            {
                _serv.SaveEvent(evnt, userName);
                //_setLabel(event);
                return PartialView("Edit", evnt);
            }
            else
            {
                levent.Level = LogLevel.Error; levent.Message = "TryUpdateModel failed";
                levent.Properties["RecordID"] = evnt.ID; log.Log(levent);
                return PartialView("Edit", evnt);
            }
        }

        //
        // GET: /Event/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, FormCollection collection, string user)
        {
            _serv.DeleteEvent(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
