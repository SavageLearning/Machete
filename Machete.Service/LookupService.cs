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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface ILookupService : IService<Lookup>
    {
        IEnumerable<DTO.LookupList> GetIndexView(viewOptions o);
        Lookup GetByKey(string category, string key);
        string textByID(int ID, string locale);
        IEnumerable<string> GetTeachers();
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
            populateStaticIds();
        }

        public IEnumerable<DTO.LookupList> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Lookup> q = repo.GetAllQ();
            //
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);
            if (!string.IsNullOrEmpty(o.category)) IndexViewBase.byCategory(o, ref q);
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);

            return q.ProjectTo<DTO.LookupList>(map.ConfigurationProvider)
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
            Worker.iActive = GetByKey(LCategory.memberstatus, LMemberStatus.Active).ID;
            Worker.iSanctioned = GetByKey(LCategory.memberstatus, LMemberStatus.Sanctioned).ID;
            Worker.iExpelled = GetByKey(LCategory.memberstatus, LMemberStatus.Expelled).ID;
            Worker.iExpired = GetByKey(LCategory.memberstatus, LMemberStatus.Expired).ID;
            Worker.iInactive = GetByKey(LCategory.memberstatus, LMemberStatus.Inactive).ID;
            //
            #endregion  
            #region WORKORDERS
            WorkOrder.iActive = GetByKey(LCategory.orderstatus, LOrderStatus.Active).ID;
            WorkOrder.iPending = GetByKey(LCategory.orderstatus, LOrderStatus.Pending).ID;
            WorkOrder.iCompleted = GetByKey(LCategory.orderstatus, LOrderStatus.Completed).ID;
            WorkOrder.iCancelled = GetByKey(LCategory.orderstatus, LOrderStatus.Cancelled).ID;
            WorkOrder.iExpired = GetByKey(LCategory.orderstatus, LOrderStatus.Expired).ID;
            #endregion
            #region EMAILS
            Email.iReadyToSend = GetByKey(LCategory.emailstatus, LEmailStatus.ReadyToSend).ID;
            Email.iSent = GetByKey(LCategory.emailstatus, LEmailStatus.Sent).ID;
            Email.iSending = GetByKey(LCategory.emailstatus, LEmailStatus.Sending).ID;
            Email.iPending = GetByKey(LCategory.emailstatus, LEmailStatus.Pending).ID;
            Email.iTransmitError = GetByKey(LCategory.emailstatus, LEmailStatus.TransmitError).ID;
            #endregion
        }

        // copied from lookupcache because lookupcache will eventually die
        public Lookup GetByKey(string category, string key)
        {
            try
            {
                return lrepo.GetByKey(category, key);
            }
            catch(Exception e)
            {
                throw new MacheteIntegrityException("Unable to Lookup Category: " + category + ", text: " + key + "Exception:" + e.ToString());
            }
        }

        public string textByID(int ID, string locale)
        {
            Lookup record;
            if (ID == 0) return null;
            try
            {
                record = lrepo.GetById(ID);
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to find Lookup record " + ID);
            }
            if (locale == "es" || locale == "ES")
            {
                return record.text_ES;
            }
            return record.text_EN; ;  //defaults to English
        }

        public IEnumerable<string> GetTeachers()
        {
            return lrepo.GetTeachers();
        }
    }
}