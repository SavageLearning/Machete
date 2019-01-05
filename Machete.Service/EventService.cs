#region COPYRIGHT
// File:     EventService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Service
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Linq;

namespace Machete.Service
{
    public interface IEventService : IService<Event>
    {
        IQueryable<Event> GetEvents(int? PID);
        dataTableResult<DTO.EventList> GetIndexView(viewOptions o);
    }

    // Business logic for Event record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class EventService : ServiceBase<Event>, IEventService
    {
        private readonly IMapper map;
        private readonly ILookupRepository lRepo;
        //
        public EventService(IEventRepository repo,
                            IUnitOfWork unitOfWork,
                            ILookupRepository lRepo,
                            IMapper map) 
                : base(repo, unitOfWork)
        {
            this.map = map;
            this.lRepo = lRepo;
            this.logPrefix = "Event";
        }
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
        public dataTableResult<DTO.EventList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.EventList>();
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
            result.query = q.ProjectTo<DTO.EventList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            return result;
        }

        public override Event Create(Event record, string user)
        {
            record.eventTypeES = lRepo.GetById(record.eventTypeID).text_ES;
            record.eventTypeEN = lRepo.GetById(record.eventTypeID).text_EN;
            return base.Create(record, user);
        }

        public override void Save(Event record, string user)
        {
            record.eventTypeES = lRepo.GetById(record.eventTypeID).text_ES;
            record.eventTypeEN = lRepo.GetById(record.eventTypeID).text_EN;
            base.Save(record, user);
        }
    }
}
