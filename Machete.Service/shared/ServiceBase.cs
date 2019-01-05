#region COPYRIGHT
// File:     ServiceBase.cs
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
using Machete.Data.Infrastructure;
using Machete.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IService<T> where T : Record
    {
        T Get(int id);
        //T Get(Func<T, bool> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Func<T, bool> where);
        T Create(T record, string user);
        void Delete(int id, string user);
        void Save(T record, string user);
        int TotalCount();
        IRepository<T> GetRepo();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ServiceBase<T> where T : Record
    {
        protected readonly IRepository<T> repo;
        protected readonly IUnitOfWork uow;
        protected Logger nlog = LogManager.GetCurrentClassLogger();
        protected LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "Service", "");
        /// <summary>
        /// replace with service-specific string for logging
        /// </summary>
        protected string logPrefix = "ServiceBase";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="uow"></param>
        protected ServiceBase(IRepository<T> repo, IUnitOfWork uow)
        {
            this.repo = repo;
            this.uow = uow;
        }
        public int TotalCount()
        {
            return repo.GetAllQ().Count();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return repo.GetAllQ();
        }

        public IEnumerable<T> GetMany(Func<T, bool> where)
        {
            return repo.GetManyQ(where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(int id)
        {
            return repo.GetById(id);
        }

        //public virtual T Get(Func<T, bool> where)
        //{
        //    return repo.Get(where);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual T Create(T record, string user)
        {
            record.createdByUser(user);
            T created = repo.Add(record);
            uow.Save();
            log(record.ID, user, logPrefix + " created");
            return created;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public virtual void Delete(int id, string user)
        {
            T record = repo.GetById(id);
            repo.Delete(record);
            uow.Save();
            log(id, user, logPrefix + " deleted");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="user"></param>
        public virtual void Save(T record, string user)
        {
            record.updatedByUser(user);
            uow.Save();
            log(record.ID, user, logPrefix + " edited");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        protected void log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            nlog.Log(levent);
        }
        public IRepository<T> GetRepo()
        {
            return this.repo;
        }

    }
    /// <summary>
    /// Case insensitive on iEnumerable LINQ joins (L2E handles iQueryable)
    /// </summary>
    public static class String
    {
        public static bool ContainsOIC(this string source, string toCheck)
        {
            if (toCheck == null || source == null) return false;
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

    }
    /// <summary>
    /// Returns query for dataTables consumption. Includes counts for table display.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class dataTableResult<T>
    {
        public IEnumerable<T> query { get; set; }
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
    }
}
