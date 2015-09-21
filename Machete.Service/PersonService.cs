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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using NLog;
using System.Globalization;

namespace Machete.Service
{
    public interface IPersonService : IService<Person>
    {
        dataTableResult<Person> GetIndexView(viewOptions o);
    }

    // Business logic for Person record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class PersonService : ServiceBase<Person>, IPersonService
    {
        private readonly ILookupCache lcache;

        public PersonService(IPersonRepository pRepo, 
                             IUnitOfWork unitOfWork,
                             ILookupCache _lcache) : base(pRepo, unitOfWork) 
        {
            this.logPrefix = "Person";
            this.lcache = _lcache;
        }  

        public dataTableResult<Person> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<Person>();
            //Get all the records
            IQueryable<Person> q = repo.GetAllQ();
            result.totalCount = q.Count();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            if (o.showWorkers == true) IndexViewBase.getWorkers(o, ref q);
            if (o.showNotWorkers == true) IndexViewBase.getNotWorkers(o, ref q);
            if (o.showExpiredWorkers == true) IndexViewBase.getExpiredWorkers(o, lcache.getByKeys(LCategory.memberstatus, LMemberStatus.Expired), ref q);
            if (o.showSExWorkers == true) IndexViewBase.getSExWorkers(o, lcache.getByKeys(LCategory.memberstatus, LMemberStatus.Sanctioned), lcache.getByKeys(LCategory.memberstatus, LMemberStatus.Expelled), ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            result.filteredCount = q.Count();
            result.query = q;//.Skip<Person>(o.displayStart).Take(o.displayLength);
            return result;
        }
    }
}