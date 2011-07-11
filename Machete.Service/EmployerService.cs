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
    public interface IEmployerService
    {
        IEnumerable<Employer> GetEmployers(bool inactive);

        Employer GetEmployer(int id);
        IEnumerable<WorkOrder> GetOrders(int id);
        Employer CreateEmployer(Employer employer, string user);
        void     DeleteEmployer(int id, string user);
        void     SaveEmployer(Employer employer, string user);
    }

    // Business logic for Employer record management
    // Ïf I made a non-web app, would I still need the code? If yes, put in here.
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository employerRepository;
        private readonly IWorkOrderService _woServ;
        private readonly IUnitOfWork unitOfWork;
        //
        private Logger log = LogManager.GetCurrentClassLogger();
        private LogEventInfo levent = new LogEventInfo(LogLevel.Debug, "EmployerService", "");
        private Employer _employer;
        //
        public EmployerService(IEmployerRepository employerRepository, 
                               IWorkOrderService workorderService, 
                               IUnitOfWork unitOfWork)
        {
            this._woServ = workorderService;
            this.employerRepository = employerRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Employer> GetEmployers(bool showInactive)
        {
            IEnumerable<Employer> employers;
            //TODO Unit test this
            if (showInactive == false)
            {
                employers = employerRepository.GetAll().Where(w => w.active == true);
            }
            else
            {
                employers = employerRepository.GetAll();
            }
            return employers;
        }

        public Employer GetEmployer(int id)
        {
            var employer = employerRepository.GetById(id);
            return employer;
        }
        public IEnumerable<WorkOrder> GetOrders(int id)
        {
            return _woServ.GetWorkOrders(id);
        }

        public Employer CreateEmployer(Employer employer, string user)
        {
            employer.createdby(user);
            _employer = employerRepository.Add(employer);
            unitOfWork.Commit();
            _log(employer.ID, user, "Employer created");
            return _employer;
        }

        public void DeleteEmployer(int id, string user)
        {
            var employer = employerRepository.GetById(id);
            employerRepository.Delete(employer);
            _log(id, user, "Employer deleted");
            unitOfWork.Commit();
        }

        public void SaveEmployer(Employer employer, string user)
        {
            employer.updatedby(user);
            _log(employer.ID, user, "Employer edited");
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