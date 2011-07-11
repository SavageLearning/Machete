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
    public interface IWorkAssignmentService
    {
        IEnumerable<WorkAssignment> GetMany(bool inactive);
        IEnumerable<WorkAssignment> GetManyByWO(int woID);
        IEnumerable<WorkAssignmentSummary> GetSummary();
        WorkAssignment Get(int id);
        WorkAssignment Create(WorkAssignment workAssignment, string user);
        void Delete(int id, string user);
        void Save(WorkAssignment workAssignment, string user);
    }

    // Business logic for WorkAssignment record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class WorkAssignmentService : IWorkAssignmentService
    {
        private readonly IWorkAssignmentRepository workAssignmentRepository;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "WorkAssignmentService", "");
        private WorkAssignment _workAssignment;
        //
        public WorkAssignmentService(IWorkAssignmentRepository workAssignmentRepository, IUnitOfWork unitOfWork)
        {
            this.workAssignmentRepository = workAssignmentRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WorkAssignment> GetMany(bool showInactive)
        {
            IEnumerable<WorkAssignment> workAssignments;
            //TODO Unit test this
            if (showInactive == false)
            {
                workAssignments = workAssignmentRepository.GetAll().Where(w => w.active == true);
            }
            else
            {
                workAssignments = workAssignmentRepository.GetAll();
            }
            return workAssignments;
        }

        public IEnumerable<WorkAssignment> GetManyByWO(int woID)
        {
               IEnumerable<WorkAssignment> workAssignments;
               workAssignments = workAssignmentRepository.GetAll().Where(w => w.workOrderID == woID);
               return workAssignments;
        }

        public WorkAssignment Get(int id)
        {
            var workAssignment = workAssignmentRepository.GetById(id);
            return workAssignment;
        }

        public IEnumerable<WorkAssignmentSummary> GetSummary()
        {
            var sum_query = from wa in workAssignmentRepository.GetAll()
                            group wa by new
                            {
                                dateSoW = wa.workOrder.dateTimeofWork.ToString("MM/dd/yyyy"),
                                wa.workOrder.status
                            } into dayGroup
                            select new WorkAssignmentSummary()
                            {
                                date = dayGroup.Key.dateSoW,
                                status = dayGroup.Key.status,
                                count = dayGroup.Count()
                            };

            return sum_query;
        }


        public WorkAssignment Create(WorkAssignment workAssignment, string user)
        {
            workAssignment.createdby(user);
            _workAssignment = workAssignmentRepository.Add(workAssignment);
            unitOfWork.Commit();
            _log(workAssignment.ID, user, "WorkAssignment created");
            return _workAssignment;
        }

        public void Delete(int id, string user)
        {
            var workAssignment = workAssignmentRepository.GetById(id);
            workAssignmentRepository.Delete(workAssignment);
            _log(id, user, "WorkAssignment deleted");
            unitOfWork.Commit();
        }

        public void Save(WorkAssignment workAssignment, string user)
        {
            workAssignment.updatedby(user);
            _log(workAssignment.ID, user, "WorkAssignment edited");
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
    }
}