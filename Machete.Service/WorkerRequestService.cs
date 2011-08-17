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
    public interface IWorkerRequestService
    {
        IEnumerable<WorkerRequest> GetWorkerRequests();
        WorkerRequest GetWorkerRequest(int id);
        WorkerRequest CreateWorkerRequest(WorkerRequest workerRequest, string user);
        void DeleteWorkerRequest(int id, string user);
        void SaveWorkerRequest(WorkerRequest workerRequest, string user);
        WorkerRequest GetWorkerRequestsByNum(int woid, int wrid);
    }
    public class WorkerRequestService : IWorkerRequestService
    {
        private readonly IWorkerRequestRepository workerRequestRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkerRequestService", "");

        public WorkerRequestService(IWorkerRequestRepository workerRequestRepository, IUnitOfWork unitOfWork)
        {
            this.workerRequestRepository = workerRequestRepository;
            this.unitOfWork = unitOfWork;
        }
        #region IWorkerRequestService Members

        public IEnumerable<WorkerRequest> GetWorkerRequests()
        {
            IEnumerable<WorkerRequest> workerRequests;
            workerRequests = workerRequestRepository.GetAll();
            return workerRequests;
        }

        public WorkerRequest GetWorkerRequestsByNum(int woid, int wkrid)
        {
            return workerRequestRepository.Get(wr => wr.WorkOrderID == woid && wr.WorkerID == wkrid);
        }

        public WorkerRequest GetWorkerRequest(int id)
        {
            var workerRequest = workerRequestRepository.GetById(id);
            return workerRequest;
        }

        public WorkerRequest CreateWorkerRequest(WorkerRequest workerRequest, string user)
        {
            workerRequest.createdby(user);
            //if (workerRequest.Person == null) throw new MissingReferenceException("WorkerRequest object is missing a Person reference.");
            WorkerRequest _workerRequest = workerRequestRepository.Add(workerRequest);
            unitOfWork.Commit();
            _log(workerRequest.ID, user, "WorkerRequest created");
            return _workerRequest;

        }

        public void DeleteWorkerRequest(int id, string user)
        {
            var workerRequest = workerRequestRepository.GetById(id);
            _log(workerRequest.ID, user, "WorkerRequest Deleted");
            workerRequestRepository.Delete(workerRequest);
            unitOfWork.Commit();
        }

        public void SaveWorkerRequest(WorkerRequest workerRequest, string user)
        {
            workerRequest.updatedby(user);
            _log(workerRequest.ID, user, "WorkerRequest edited");
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