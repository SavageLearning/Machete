using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;

namespace Machete.Service
{
    public interface IWorkerSigninService
    {
        IEnumerable<WorkerSignin> GetWorkerSignins();
        WorkerSignin GetWorkerSignin(int id);
        void CreateWorkerSignin(WorkerSignin workerSignin);
        void DeleteWorkerSignin(int id);
        void SaveWorkerSignin();
    }
    public class WorkerSigninService : IWorkerSigninService
    {
        private readonly IWorkerSigninRepository workerSigninRepository;
        private readonly IUnitOfWork unitOfWork;
        public WorkerSigninService(IWorkerSigninRepository workerSigninRepository, IUnitOfWork unitOfWork)
        {
            this.workerSigninRepository = workerSigninRepository;
            this.unitOfWork = unitOfWork;
        }
        #region IWorkerSigninService Members

        public IEnumerable<WorkerSignin> GetWorkerSignins()
        {
            var categories = workerSigninRepository.GetAll();
            return categories;
        }

        public WorkerSignin GetWorkerSignin(int id)
        {
            var workerSignin = workerSigninRepository.GetById(id);
            return workerSignin;
        }

        public void CreateWorkerSignin(WorkerSignin workerSignin)
        {
            workerSigninRepository.Add(workerSignin);
            unitOfWork.Commit();
        }

        public void DeleteWorkerSignin(int id)
        {
            var workerSignin = workerSigninRepository.GetById(id);
            workerSigninRepository.Delete(workerSignin);
            unitOfWork.Commit();
        }

        public void SaveWorkerSignin()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
