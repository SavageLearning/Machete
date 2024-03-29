#region COPYRIGHT
// File:     PersonService.cs
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
using Machete.Service.Infrastructure;
using Machete.Domain;
using Machete.Service.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace Machete.Service
{
    public interface IPersonService : IService<Person>
    {
        dataTableResult<PersonList> GetIndexView(viewOptions o);
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : ServiceBase2<Person>, IPersonService
    {
        public PersonService(IDatabaseFactory db, IMapper map) : base(db, map) {}

        public new IQueryable<Person> GetAll()
        {
            return dbset.Include(a => a.Worker).AsNoTracking().AsQueryable();
        }
        public dataTableResult<DTO.PersonList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.PersonList>();
            //Get all the records
            IQueryable<Person> q = GetAll();
            result.totalCount = q.Count();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            if (o.showWorkers == true) IndexViewBase.getWorkers(o, ref q);
            if (o.showNotWorkers == true) IndexViewBase.getNotWorkers(o, ref q);
            if (o.showExpiredWorkers == true) {
                IndexViewBase.getExpiredWorkers(o, 
                    WorkOrder.iExpired,
                    ref q);
            }
            if (o.showSExWorkers == true) {
                IndexViewBase.getSExWorkers(o,
                    Worker.iSanctioned,
                    Worker.iExpelled, 
                    ref q);

            }
            if (o.showActiveWorkers == true) {
                IndexViewBase.GetActiveWorkers(Worker.iActive,
                    ref q);
            }
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            result.filteredCount = q.Count();
            result.totalCount = GetAll().Count();
            result.query = q.ProjectTo<DTO.PersonList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            return result;
        }

        public override Person Create(Person record, string user)
        {
            updateComputedValues(ref record);
            return base.Create(record, user);
        }

        public override void Save(Person record, string user)
        {
            updateComputedValues(ref record);
            base.Save(record, user);
        }

        private void updateComputedValues(ref Person r)
        {
            var name = new StringBuilder(r.firstname1).Append(" ");
            if (r.firstname2 != null) name.Append(r.firstname2).Append(" ");
            name.Append(r.lastname1);
            if (r.lastname2 != null) name.Append(" ").Append(r.lastname2);
            r.fullName = name.ToString();
        }
    }
}
