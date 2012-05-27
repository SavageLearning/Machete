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
    public interface IWorkerService : IService<Worker>
    {
        Worker GetWorkerByNum(int dwccardnum);
        IEnumerable<Worker> GetIndexView(viewOptions o);
    }
    public class WorkerService : ServiceBase<Worker>, IWorkerService
    {
        public WorkerService(IWorkerRepository wRepo, IUnitOfWork uow) : base(wRepo, uow)
        {
            this.logPrefix = "Worker";
        }
        public Worker GetWorkerByNum(int dwccardnum)
        {
            Worker worker = repo.Get(w => w.dwccardnum == dwccardnum);
            return worker;
        }
        public IEnumerable<Worker> GetIndexView(viewOptions o)
        {
            //Get all the records
            IQueryable<Worker> q = repo.GetAllQ();
            //Search based on search-bar string 
            if (!string.IsNullOrEmpty(o.search)) IndexViewBase.search(o, ref q);
            //ORDER BY based on column selection
            IndexViewBase.sortOnColName(o.sortColName, o.orderDescending, ref q);
            //Limit results to the display length and offset
            q = q.Skip(o.displayStart).Take(o.displayLength);
            return q;
        }
    }
}