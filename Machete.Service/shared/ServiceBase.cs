using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data.Infrastructure;
using Machete.Domain;
using NLog;

namespace Machete.Service
{
    public interface IService<T> where T : Record
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        T Create(T record, string user);
        void Delete(int id, string user);
        void Save(T record, string user);
    }
    public abstract class ServiceBase<T> where T : Record
    {
        protected readonly IRepository<T> repo;
        protected readonly IUnitOfWork uow;
        protected Logger nlog = LogManager.GetCurrentClassLogger();
        protected LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EmployerService", "");
        protected string logPrefix = "ServiceBase";

        protected ServiceBase(IRepository<T> repo, IUnitOfWork uow)
        {
            this.repo = repo;
            this.uow = uow;
        }

        public IEnumerable<T> GetAll() 
        {
            return repo.GetAll();
        }

        public T Get(int id)
        {
            return repo.GetById(id);
        }

        public T Create(T record, string user)
        {
            record.createdby(user);
            T created = repo.Add(record);
            uow.Commit();
            log(record.ID, user, logPrefix + " created");
            return created;
        }

        public void Delete(int id, string user)
        {
            T record = repo.GetById(id);
            repo.Delete(record);
            log(id, user, logPrefix + " deleted");
            uow.Commit();
        }

        public void Save(T record, string user)
        {
            record.updatedby(user);
            log(record.ID, user, logPrefix + " edited");
            uow.Commit();
        }

        protected void log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            nlog.Log(levent);
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
}
