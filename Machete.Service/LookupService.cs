#region COPYRIGHT
// File:     LookupService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
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
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface ILookupService : IService<Lookup>
    {
        IEnumerable<Lookup> GetIndexView(viewOptions o);
    }

    // Business logic for Lookup record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class LookupService : ServiceBase<Lookup>, ILookupService
    {
        private readonly ILookupRepository lrepo;
        public LookupService(ILookupRepository lRepo,
                             IUnitOfWork unitOfWork)
            : base(lRepo, unitOfWork)
        {
            this.lrepo = lRepo;
            this.logPrefix = "Lookup";
        }

        public IEnumerable<Lookup> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Lookup> q = repo.GetAllQ();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            if (!string.IsNullOrEmpty(o.category)) IndexViewBase.byCategory(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            q = q.Skip<Lookup>(o.displayStart).Take(o.displayLength);
            return q;
        }
        public override Lookup Create(Lookup record, string user)
        {
            // Only one record can be true in a given category
            if (record.selected == true)
            {
                lrepo.clearSelected(record.category);
                record.selected = true;
            }

            return base.Create(record, user);
        }
        public override void Save(Lookup record, string user)
        {
            // Only one record can be true in a given category
            if (record.selected == true)
            {
                lrepo.clearSelected(record.category);
                record.selected = true;
            }
            base.Save(record, user);
        }
    }
}