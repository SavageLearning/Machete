#region COPYRIGHT
// File:     EmployerService.cs
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
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IEmployerService : IService<Employer>
    {
        dataTableResult<DTO.EmployersList> GetIndexView(viewOptions o);
        Employer Get(string guid);
    }

    public class EmployerService : ServiceBase<Employer>, IEmployerService
    {
        private readonly IWorkOrderService woServ;
        private readonly IMapper map;
        new IEmployerRepository repo;
        public EmployerService(IEmployerRepository repo, 
                               IWorkOrderService woServ,
                               IUnitOfWork unitOfWork,
                               IMapper map)
                : base(repo, unitOfWork)
        {
            this.woServ = woServ;
            this.map = map;
            this.logPrefix = "Employer";
            this.repo = repo;
        }

        public Employer Get(string guid)
        {
            return repo.GetBySubject(guid);
        }

        public override Employer Create(Employer record, string user)
        {
            var result = base.Create(record, user);
            uow.Commit();
            return result;
        }

        public override void Save(Employer record, string user)
        {
            base.Save(record, user);
            uow.Commit();
        }

        public override void Delete(int id, string user)
        {
            base.Delete(id, user);
            uow.Commit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dataTableResult<DTO.EmployersList> GetIndexView(viewOptions o)
        {
            var result = new dataTableResult<DTO.EmployersList>();
            //Get all the records
            IQueryable<Employer> q = repo.GetAllQ();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.sSearch)) IndexViewBase.search(o, ref q);

            if (o.onlineSource == true) IndexViewBase.filterOnlineSource(o, ref q);
            //Sort the Persons based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            result.filteredCount = q.Count();
            result.totalCount = repo.GetAllQ().Count();
            result.query = q.ProjectTo<DTO.EmployersList>(map.ConfigurationProvider)
                .Skip(o.displayStart)
                .Take(o.displayLength)
                .AsEnumerable();
            return result;            
        }
    }
}