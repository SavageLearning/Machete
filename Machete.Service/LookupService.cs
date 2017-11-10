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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface ILookupService : IService<Lookup>
    {
        IEnumerable<DTO.ConfigList> GetIndexView(viewOptions o);
    }

    // Business logic for Lookup record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class LookupService : ServiceBase<Lookup>, ILookupService
    {
        private readonly ILookupRepository lrepo;
        private readonly IMapper map;
        public LookupService(ILookupRepository lRepo,
                             IMapper map,
                             IUnitOfWork unitOfWork)
            : base(lRepo, unitOfWork)
        {
            this.lrepo = lRepo;
            this.map = map;
            this.logPrefix = "Lookup";
        }

        public IEnumerable<DTO.ConfigList> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Lookup> q = repo.GetAllQ();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            if (!string.IsNullOrEmpty(o.category)) IndexViewBase.byCategory(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            return q.ProjectTo<DTO.ConfigList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
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

        public void populateStaticIds()
        {
            #region WORKERS
            Worker.iActive = getByKeys(LCategory.memberstatus, LMemberStatus.Active);
            Worker.iSanctioned = getByKeys(LCategory.memberstatus, LMemberStatus.Sanctioned);
            Worker.iExpelled = getByKeys(LCategory.memberstatus, LMemberStatus.Expelled);
            Worker.iExpired = getByKeys(LCategory.memberstatus, LMemberStatus.Expired);
            Worker.iInactive = getByKeys(LCategory.memberstatus, LMemberStatus.Inactive);
            //
            #endregion  
            #region WORKORDERS
            WorkOrder.iActive = getByKeys(LCategory.orderstatus, LOrderStatus.Active);
            WorkOrder.iPending = getByKeys(LCategory.orderstatus, LOrderStatus.Pending);
            WorkOrder.iCompleted = getByKeys(LCategory.orderstatus, LOrderStatus.Completed);
            WorkOrder.iCancelled = getByKeys(LCategory.orderstatus, LOrderStatus.Cancelled);
            WorkOrder.iExpired = getByKeys(LCategory.orderstatus, LOrderStatus.Expired);
            #endregion
            #region EMAILS
            Email.iReadyToSend = getByKeys(LCategory.emailstatus, LEmailStatus.ReadyToSend);
            Email.iSent = getByKeys(LCategory.emailstatus, LEmailStatus.Sent);
            Email.iSending = getByKeys(LCategory.emailstatus, LEmailStatus.Sending);
            Email.iPending = getByKeys(LCategory.emailstatus, LEmailStatus.Pending);
            Email.iTransmitError = getByKeys(LCategory.emailstatus, LEmailStatus.TransmitError);
            #endregion
        }

        // copied from lookupcache because lookupcache will eventually die
        private int getByKeys(string category, string key)
        {
            try
            {
                return base.Get(s => s.category == category && s.key == key).ID;
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to Lookup Category: " + category + ", text: " + key);
            }
        }
    }
}