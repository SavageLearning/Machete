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
        dataTableResult<Event> GetIndexView(viewOptions o);
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
        public dataTableResult<Event> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Event>();
            //Get all the records
            IQueryable<Event> q = GetEvents(o.personID);
            result.totalCount = q.Count();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch))
            {
                q = GetEvents(o.personID)
                    .Where(p => p.notes.Contains(o.sSearch));
            }

            //Sort the Persons based on column selection
            switch (o.sortColName)
            {
                case "dateupdated": q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
                default: q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated); break;
            }

            //Limit results to the display length and offset
            result.filteredCount = q.Count();            
            result.query = q.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}
