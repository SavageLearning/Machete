using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using NLog;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace Machete.Service
{
    public abstract class ServiceBase2<T> where T : Record
    {
        protected readonly MacheteContext db;
        protected readonly DbSet<T> dbset;
        protected Logger nlog = LogManager.GetCurrentClassLogger();
        protected LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "Service2", "");
        protected readonly IMapper map;
        /// <summary>
        /// replace with service-specific string for logging
        /// </summary>
        protected string logPrefix;

        protected ServiceBase2(IDatabaseFactory dbf, IMapper map)
        {
            this.map = map;
            this.db = dbf.Get();
            this.dbset = db.Set<T>();
            this.logPrefix = typeof(T).ToString();
        }

        public int TotalCount()
        {
            return dbset.AsNoTracking().Count();
        }

        public IEnumerable<T> GetAll()
        {
            return dbset.AsNoTracking().AsQueryable();
        }

        public IEnumerable<T> GetMany(Func<T, bool> where)
        {
            return dbset.Where(where).AsQueryable();
        }

        public virtual T Get(int id)
        {
            return dbset.Find(id);
        }

        public virtual T Create(T record, string user)
        {
            record.createdByUser(user);
            T created = dbset.Add(record).Entity;
            db.SaveChanges();
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
            T record = dbset.Find(id);
            dbset.Remove(record);
            db.SaveChanges();
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
            dbset.Update(record);
            db.SaveChanges();
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

        protected string lookupTextByID(int ID, string locale)
        {
            Lookup record;
            if (ID == 0) return null;
            try
            {
                record = db.Set<Lookup>().Single(s => s.ID == ID);
            }
            catch
            {
                throw new MacheteIntegrityException("Unable to find Lookup record " + ID);
            }
            if (locale == "es" || locale == "ES")
            {
                return record.text_ES;
            }
            //defaults to English
            return record.text_EN;
        }
    }
}
