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
    public interface IEventService
    {
        IEnumerable<Event> GetEvents();
        Event GetEvent(int id);
        Event CreateEvent(Event evnt, string user);
        void DeleteEvent(int id, string user);
        void SaveEvent(Event evnt, string user);
    }

    // Business logic for Event record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EventService", "");
        private Event _event;
        //
        public EventService(IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            this.eventRepository = eventRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Event> GetEvents()
        {
            var events = eventRepository.GetAll();
            return events;
        }

        public Event GetEvent(int id)
        {
            var evnt = eventRepository.GetById(id);
            return evnt;
        }

        public Event CreateEvent(Event evnt, string user)
        {
            evnt.createdby(user);
            _event = eventRepository.Add(evnt);
            unitOfWork.Commit();
            _log(evnt.ID, user, "Event created");
            return _event;
        }

        public void DeleteEvent(int id, string user)
        {
            var evnt = eventRepository.GetById(id);
            eventRepository.Delete(evnt);
            _log(id, user, "Event deleted");
            unitOfWork.Commit();
        }

        public void SaveEvent(Event evnt, string user)
        {
            evnt.updatedby(user);
            _log(evnt.ID, user, "Event edited");
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
