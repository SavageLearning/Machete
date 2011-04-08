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
    public interface IWorkerService
    {
        IEnumerable<Worker> GetWorkers(bool inactive);
        Worker GetWorker(int id);
        Worker CreateWorker(Worker worker, string user);
        void DeleteWorker(int id, string user);
        void SaveWorker(Worker worker, string user);
    }
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository workerRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerService", "");

        public WorkerService(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            this.workerRepository = workerRepository;
            this.unitOfWork = unitOfWork;
        }
        #region IWorkerService Members

        public IEnumerable<Worker> GetWorkers(bool showInactive)
        {
            IEnumerable<Worker> workers;
            //TODO unit test this
            if (showInactive == false)
            {
                workers = workerRepository.GetAll().Where(w => w.active == true);
            }
            else
            {
                workers = workerRepository.GetAll();
            }
            return workers;
        }

        public Worker GetWorker(int id)
        {
            var worker = workerRepository.GetById(id);
            return worker;
        }

        public Worker CreateWorker(Worker worker, string user)
        {
            worker.createdby(user);
            worker.Person.createdby(user);
            if (worker.Person == null) throw new MissingReferenceException("Worker object is missing a Person reference.");
            Worker _worker = workerRepository.Add(worker);
            unitOfWork.Commit();
            _log(worker.ID, user, "Worker created");
            return _worker;

        }

        public void DeleteWorker(int id, string user)
        {
            var worker = workerRepository.GetById(id);
            _log(worker.ID, user, "Worker Deleted");
            workerRepository.Delete(worker);
            unitOfWork.Commit();
        }

        public void SaveWorker(Worker worker, string user)
        {
            worker.updatedby(user);
            _log(worker.ID, user, "Worker edited");
            unitOfWork.Commit();
        }

        private void _log(int ID, string user, string msg)
        {
            levent.Level = LogLevel.Info;
            levent.Message = msg;
            levent.Properties["RecordID"] = ID; //magic string maps to NLog config
            levent.Properties["username"] = user;
            log.Log(levent);
        }
        #endregion
    }
}