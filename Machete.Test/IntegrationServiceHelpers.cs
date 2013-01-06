#region COPYRIGHT
// File:     IntegrationServiceHelpers.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
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
        public WorkerSigninRepository _wsiRepo;
        public WorkerRepository _wRepo;
        public PersonRepository _pRepo;
        public WorkOrderRepository _woRepo;
        public WorkAssignmentRepository _waRepo;
        public WorkerRequestRepository _wrRepo;
        public ILookupRepository _lRepo;
        public ImageRepository _iRepo;
        public DatabaseFactory _dbFactory;
        public WorkerSigninService _wsiServ;
        public WorkerService _wServ;
        public PersonService _pServ;
        public ImageService _iServ;
        public WorkerRequestService _wrServ;
        public WorkOrderService _woServ;
        public WorkAssignmentService _waServ;
        public ActivityRepository _aRepo;
        public ActivitySigninRepository _asRepo;
        public ActivityService _aServ;
        public ActivitySigninService _asServ;
        public IUnitOfWork _unitofwork;
        public MacheteContext DB;
        public ServiceTest() { }

        protected void Initialize()
        {
            _init(new TestInitializer(), "macheteConnection");
        }
        public ServiceTest LoadContext()
        {
            _makeContext();
            return this;
        }
        public void Initialize(IDatabaseInitializer<MacheteContext> initializer, string connection)
        {
            _init(initializer, connection);
        }
        private void _init(IDatabaseInitializer<MacheteContext> initializer, string connection)
        {
            Database.SetInitializer<MacheteContext>(initializer);
            DB = new MacheteContext(connection);
            DB.Database.Delete();
            DB.Database.Initialize(true);
            Records.Initialize(DB);
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            Lookups.Initialize();
            _makeContext();
        }
        private void _makeContext()
        {
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
