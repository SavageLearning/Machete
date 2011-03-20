using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;

namespace Machete.Service
{
    public interface IWorkerService
    {
        IEnumerable<Worker> GetWorkers();
        Worker GetWorker(int id);
        void CreateWorker(Worker worker);
        void DeleteWorker(int id);
        void SaveWorker();
    }
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository workerRepository;
        private readonly IUnitOfWork unitOfWork;
        public WorkerService(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            this.workerRepository = workerRepository;
            this.unitOfWork = unitOfWork;
        }
        #region IWorkerService Members

        public IEnumerable<Worker> GetWorkers()
        {
            var workers = workerRepository.GetAll();
            return workers;
        }

        public Worker GetWorker(int id)
        {
            var worker = workerRepository.GetById(id);
            return worker;
        }

        public void CreateWorker(Worker worker)
        {
            if (worker.Person == null) throw new MissingReferenceException("Worker object is missing a Person reference.");
            workerRepository.Add(worker);
            unitOfWork.Commit();
        }

        public void DeleteWorker(int id)
        {
            var worker = workerRepository.GetById(id);
            workerRepository.Delete(worker);
            unitOfWork.Commit();
        }

        public void SaveWorker()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}