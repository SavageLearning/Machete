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
    public interface IEventService : IService<Event>
    {
        IQueryable<Event> GetEvents(int? PID);
        IEnumerable<Event> GetIndexView(viewOptions o);
    }

    // Business logic for Event record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class EventService : ServiceBase<Event>, IEventService
    {
        //
        public EventService(IEventRepository repo, IUnitOfWork unitOfWork) : base(repo, unitOfWork)
        {}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public IQueryable<Event> GetEvents(int? PID)
        {
            IQueryable<Event> events;
            if (PID == null)
            {
                events = repo.GetAllQ();
                return events;
            }
            events = repo.GetManyQ(e => e.PersonID == PID);
            return events;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Event> q = GetEvents(o.personID);
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search))
            {
                q = repo.GetAllQ()
                    .Where(p => p.notes.ToString().Contains(o.search));
            }

            //Sort the Persons based on column selection
            switch (o.sortColName)
            {
                case "dateupdated": q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
            }

            //Limit results to the display length and offset
            q = q.Skip(o.displayStart).Take(o.displayLength);
            return q;
        }
    }
}
