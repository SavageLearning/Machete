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
            dataTableResult<Event> result = new dataTableResult<Event>();
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
                case "dateupdated":
                {
                    q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated);
                    break;
                }
                default:
                {
                    q = o.orderDescending ? q.OrderByDescending(p => p.dateupdated) : q.OrderBy(p => p.dateupdated);
                    break;
                }
            }

            //Limit results to the display length and offset
            result.filteredCount = q.Count();            
            result.query = q.Skip(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}
