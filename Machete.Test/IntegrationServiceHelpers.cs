using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Data;
using Machete.Service;
using Machete.Data.Infrastructure;
using Machete.Web.Helpers;
using System.Data.Entity;

namespace Machete.Test
{
    public class ServiceTest
    {
        protected WorkerSigninRepository _wsiRepo;
        protected WorkerRepository _wRepo;
        protected PersonRepository _pRepo;
        protected WorkOrderRepository _woRepo;
        protected WorkAssignmentRepository _waRepo;
        protected WorkerRequestRepository _wrRepo;
        protected ILookupRepository _lRepo;
        protected ImageRepository _iRepo;
        protected DatabaseFactory _dbFactory;
        protected WorkerSigninService _wsiServ;
        protected WorkerService _wServ;
        protected PersonService _pServ;
        protected ImageService _iServ;
        protected WorkerRequestService _wrServ;
        protected WorkOrderService _woServ;
        protected WorkAssignmentService _waServ;
        protected ActivityRepository _aRepo;
        protected ActivitySigninRepository _asRepo;
        protected ActivityService _aServ;
        protected ActivitySigninService _asServ;
        protected IUnitOfWork _unitofwork;
        protected MacheteContext DB;

        protected void Initialize()
        {
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            DB = new MacheteContext();
            DB.Database.Delete();
            DB.Database.Initialize(true);
            Records.Initialize(DB);
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            Lookups.Initialize();
            _dbFactory = new DatabaseFactory();
            _iRepo = new ImageRepository(_dbFactory);
            _wRepo = new WorkerRepository(_dbFactory);
            _woRepo = new WorkOrderRepository(_dbFactory);
            _wrRepo = new WorkerRequestRepository(_dbFactory);
            _waRepo = new WorkAssignmentRepository(_dbFactory);
            _wsiRepo = new WorkerSigninRepository(_dbFactory);
            _lRepo = new LookupRepository(_dbFactory);
            _pRepo = new PersonRepository(_dbFactory);
            _aRepo = new ActivityRepository(_dbFactory);
            _asRepo = new ActivitySigninRepository(_dbFactory);
            _unitofwork = new UnitOfWork(_dbFactory);
            _pServ = new PersonService(_pRepo, _unitofwork);
            _iServ = new ImageService(_iRepo, _unitofwork);
            _aServ = new ActivityService(_aRepo, _asServ, _unitofwork);
            _asServ = new ActivitySigninService(_asRepo, _wRepo, _pRepo, _iRepo, _wrRepo, _unitofwork);
            _wrServ = new WorkerRequestService(_wrRepo, _unitofwork);
            _waServ = new WorkAssignmentService(_waRepo, _wRepo, _lRepo, _wsiRepo, _unitofwork);
            _wServ = new WorkerService(_wRepo, _unitofwork);
            _woServ = new WorkOrderService(_woRepo, _waServ, _unitofwork);
            _wsiServ = new WorkerSigninService(_wsiRepo, _wRepo, _iRepo, _wrRepo, _unitofwork);

        }
    }


}
