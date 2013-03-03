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
using Machete.Domain;

namespace Machete.Test
{
    public class FluentRecordBase
    {
        #region internal fields
        private ActivityRepository _repoA;
        private ActivitySigninRepository _repoAS; 
        private WorkerSigninRepository _repoWSI;
        private WorkerRepository _repoW;
        private PersonRepository _repoP;
        private WorkOrderRepository _repoWO;
        private WorkAssignmentRepository _repoWA;
        private WorkerRequestRepository _repoWR;
        private LookupRepository _repoL;
        private ImageRepository _repoI;
        private EmployerRepository _repoE;
        private DatabaseFactory _dbFactory;
        private WorkerSigninService _servWSI;
        private WorkerService _servW;
        private PersonService _servP;
        private ImageService _servI;
        private WorkerRequestService _servWR;
        private WorkOrderService _servWO;
        private WorkAssignmentService _servWA;
        private ActivityService _servA;
        private ActivitySigninService _servAS;
        private EmployerService _servE;
        private LookupService _servL;
        private IUnitOfWork _uow;
        public MacheteContext DB { get; set; }
        private Employer _emp;
        private WorkOrder _wo;
        private WorkAssignment _wa;
        private Person _p;
        private Worker _w;
        private WorkerRequest  _wr;
        private WorkerSignin  _wsi;
        private Activity  _a;
        private ActivitySignin _as;
        private Event _e;
        private Lookup _l;
        private Image _i;
        private string _user = "FluentRecordBase";
        private System.Random _random = new System.Random((int)DateTime.Now.Ticks);

        #endregion

        public FluentRecordBase() { }

        public FluentRecordBase(string user)
        {
            _user = user;
        }

        public FluentRecordBase Initialize(IDatabaseInitializer<MacheteContext> initializer, string connection)
        {
            Database.SetInitializer<MacheteContext>(initializer);
            DB = new MacheteContext(connection);
            WorkerCache.Initialize(DB);
            LookupCache.Initialize(DB);
            Lookups.Initialize();
            _dbFactory = new DatabaseFactory();
            _dbFactory.Set(DB);
            return this;
        }

        public FluentRecordBase AddDBFactory()
        {
            Initialize(new TestInitializer(), "macheteConnection");
            _uow = new UnitOfWork(_dbFactory);
            return this;
        }

        #region Employers

        public FluentRecordBase AddRepoEmployer()
        {
            if (_dbFactory == null) AddDBFactory();
            _repoE = new EmployerRepository(_dbFactory);
            return this;
        }

        public EmployerRepository ToRepoEmployer()
        {
            if (_repoE == null) AddRepoEmployer();
            return _repoE;
        }

        public FluentRecordBase AddServEmployer()
        {
            //
            // DEPENDENCIES
            if (_repoE == null) AddRepoEmployer();
            if (_servWO == null) AddServWorkOrder();
            if (_uow == null) AddUOW();
            _servE = new EmployerService(_repoE, _servWO, _uow);
            return this;
        }

        public EmployerService ToServEmployer()
        {
            if (_servE == null) AddServEmployer();
            return _servE;
        }

        public FluentRecordBase AddEmployer(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servE == null) AddServEmployer();
            //
            // ARRANGE
            _emp = (Employer)Records.employer.Clone();
            if (datecreated != null) _emp.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _emp.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servE.Create(_emp, _user);
            return this;
        }

        public Employer ToEmployer()
        {
            if (_emp == null) AddEmployer();
            return _emp;
        }

        public Employer CloneEmployer()
        {
            var e = (Employer)Records.employer.Clone();
            e.name = RandomString(10);
            e.email = e.name + "@random.com";
            return e;
        }

        #endregion  

        #region WorkOrders

        public FluentRecordBase AddRepoWorkOrder()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWO = new WorkOrderRepository(_dbFactory);
            return this;
        }

        public WorkOrderRepository ToRepoWorkOrder()
        {
            if (_repoWO == null) AddRepoWorkOrder();
            return _repoWO;
        }

        public FluentRecordBase AddServWorkOrder()
        {
            //
            // DEPENDENCIES
            if (_repoWO == null) AddRepoWorkOrder();
            if (_servWA == null) AddServWorkAssignment();
            if (_uow == null) AddUOW();
            _servWO = new WorkOrderService(_repoWO, _servWA, _uow);
            return this;
        }

        public WorkOrderService ToServWorkOrder()
        {
            if (_servWO == null) AddServWorkOrder();
            return _servWO;
        }

        public FluentRecordBase AddWorkOrder(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_emp == null) AddEmployer();
            //
            // ARRANGE
            _wo = (WorkOrder)Records.order.Clone();
            _wo.Employer = _emp;
            if (datecreated != null) _wo.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wo.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servWO.Create(_wo, _user);
            return this;
        }

        public WorkOrder ToWorkOrder()
        {
            if (_wo == null) AddWorkOrder();
            return _wo;
        }

        public WorkOrder CloneWorkOrder()
        {
            var wo = (WorkOrder)Records.order.Clone();
            wo.contactName = RandomString(10);
            return wo;
        }

        public void Reload<T>(T entity) where T : Record
        {
            if (DB == null) AddDBFactory();
             DB.Entry<T>(entity).Reload();
        }

        #endregion 

        #region WorkAssignments

        public FluentRecordBase AddRepoWorkAssignment()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWA = new WorkAssignmentRepository(_dbFactory);
            return this;
        }

        public WorkAssignmentRepository ToRepoWorkAssignment()
        {
            if (_repoWA == null) AddRepoWorkAssignment();
            return _repoWA;
        }

        public FluentRecordBase AddServWorkAssignment()
        {
            //
            // DEPENDENCIES
            if (_repoWA == null) AddRepoWorkAssignment();
            if (_repoW == null) AddRepoWorker();
            if (_repoL == null) AddRepoLookup();
            if (_repoWSI == null) AddRepoWorkerSignin();
            if (_uow == null) AddUOW();
            _servWA = new WorkAssignmentService(_repoWA, _repoW, _repoL, _repoWSI, _uow);
            return this;
        }

        public WorkAssignmentService ToServWorkAssignment()
        {
            if (_servWA == null) AddServWorkAssignment();
            return _servWA;
        }

        public FluentRecordBase AddWorkAssignment(
            string desc = null,
            int? skill = null,
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_wo == null) AddWorkOrder();
            //
            // ARRANGE
            _wa = (WorkAssignment)Records.assignment.Clone();
            _wa.workOrder = _wo;
            if (datecreated != null) _wa.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wa.dateupdated = (DateTime)dateupdated;
            if (desc != null) _wa.description = desc;
            if (skill != null) _wa.skillID = (int)skill;
            //
            // ACT
            _servWA.Create(_wa, _user);
            return this;
        }

        public WorkAssignment ToWorkAssignment()
        {
            if (_wa == null) AddWorkAssignment();
            return _wa;
        }

        public WorkAssignment CloneWorkAssignment()
        {
            var wa = (WorkAssignment)Records.assignment.Clone();
            wa.description = RandomString(10);
            return wa;
        }

        #endregion 

        #region WorkerSignins

        public FluentRecordBase AddRepoWorkerSignin()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWSI = new WorkerSigninRepository(_dbFactory);
            return this;
        }

        public WorkerSigninRepository ToRepoWorkerSignin()
        {
            if (_repoWSI == null) AddRepoWorkerSignin();
            return _repoWSI;
        }

        public FluentRecordBase AddServWorkerSignin()
        {
            //
            // DEPENDENCIES
            if (_repoWSI == null) AddRepoWorkerSignin();
            if (_repoW == null) AddRepoWorker();
            if (_repoL == null) AddRepoImage();
            if (_repoWSI == null) AddRepoWorkerRequest();
            if (_uow == null) AddUOW();
            _servWSI = new WorkerSigninService(_repoWSI, _repoW, _repoI, _repoWR, _uow);
            return this;
        }

        public WorkerSigninService ToServWorkerSignin()
        {
            if (_servWSI == null) AddServWorkerSignin();
            return _servWSI;
        }

        public FluentRecordBase AddWorkerSignin(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servWSI == null) AddServWorkerSignin();
            if (_w == null) AddWorker();
            //
            // ARRANGE
            _wsi = (WorkerSignin)Records.signin.Clone();
            if (datecreated != null) _wsi.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wsi.dateupdated = (DateTime)dateupdated;
            _wsi.dwccardnum = _w.dwccardnum;
            //
            // ACT
            _servWSI.Create(_wsi, _user);
            return this;
        }

        public WorkerSignin ToWorkerSignin()
        {
            if (_wsi == null) AddWorkerSignin();
            return _wsi;
        }

        #endregion 

        #region Persons

        public FluentRecordBase AddRepoPerson()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoP = new PersonRepository(_dbFactory);
            return this;
        }

        public PersonRepository ToRepoPerson()
        {
            if (_repoP == null) AddRepoPerson();
            return _repoP;
        }

        public FluentRecordBase AddServPerson()
        {
            //
            // DEPENDENCIES
            if (_repoP == null) AddRepoPerson();
            if (_uow == null) AddUOW();
            _servP = new PersonService(_repoP, _uow);
            return this;
        }

        public PersonService ToServPerson()
        {
            if (_servP == null) AddServPerson();
            return _servP;
        }

        public FluentRecordBase AddPerson(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servP == null) AddServPerson();
            //
            // ARRANGE
            _p = (Person)Records.person.Clone();
            if (datecreated != null) _p.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _p.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servP.Create(_p, _user);
            return this;
        }

        public Person ToPerson()
        {
            if (_p == null) AddPerson();
            return _p;
        }

        #endregion 

        #region Workers

        public FluentRecordBase AddRepoWorker()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoW = new WorkerRepository(_dbFactory);
            return this;
        }

        public WorkerRepository ToRepoWorker()
        {
            if (_repoW == null) AddRepoWorker();
            return _repoW;
        }

        public FluentRecordBase AddServWorker()
        {
            //
            // DEPENDENCIES
            if (_repoW == null) AddRepoWorker();
            if (_uow == null) AddUOW();
            _servW = new WorkerService(_repoW, _uow);
            return this;
        }

        public WorkerService ToServWorker()
        {
            if (_servW == null) AddServWorker();
            return _servW;
        }

        public FluentRecordBase AddWorker(
            int? skill1 = null,
            int? skill2 = null,
            int? skill3 = null,
            int? status = null,
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_p == null) AddPerson();
            if (_servW == null) AddServWorker();
            //
            // ARRANGE
            _w = (Worker)Records.worker.Clone();
            _w.Person = _p;
            if (skill1 != null) _w.skill1 = skill1;
            if (skill2 != null) _w.skill2 = skill2;
            if (skill3 != null) _w.skill3 = skill3;
            if (status != null) _w.memberStatus = (int)status;
            if (datecreated != null) _w.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _w.dateupdated = (DateTime)dateupdated;
            // kludge
            _w.dwccardnum = Records.GetNextMemberID(DB.Workers);
            //
            // ACT
            _servW.Create(_w, _user);
            return this;
        }

        public int GetNextMemberID()
        {
            if (_dbFactory == null) AddDBFactory();
            return Records.GetNextMemberID(DB.Workers);
        }

        public Worker ToWorker()
        {
            if (_w == null) AddWorker();
            return _w;
        }

        #endregion 

        #region WorkerRequests

        public FluentRecordBase AddRepoWorkerRequest()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoWR = new WorkerRequestRepository(_dbFactory);
            return this;
        }

        public WorkerRequestRepository ToRepoWorkerRequest()
        {
            if (_repoWR == null) AddRepoWorkerRequest();
            return _repoWR;
        }

        public FluentRecordBase AddServWorkerRequest()
        {
            //
            // DEPENDENCIES
            if (_repoWR == null) AddRepoWorkerRequest();
            if (_uow == null) AddUOW();
            _servWR = new WorkerRequestService(_repoWR, _uow);
            return this;
        }

        public WorkerRequestService ToServWorkerRequest()
        {
            if (_servWR == null) AddServWorkerRequest();
            return _servWR;
        }

        public FluentRecordBase AddWorkerRequest(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servWR == null) AddServWorkerRequest();
            if (_wo == null) AddWorkOrder();
            if (_w == null) AddWorker();
            //
            // ARRANGE
            _wr = (WorkerRequest)Records.request.Clone();
            _wr.workOrder = _wo;
            _wr.WorkerID = _w.ID;
            if (datecreated != null) _wr.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wr.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servWR.Create(_wr, _user);
            return this;
        }

        public WorkerRequest ToWorkerRequest()
        {
            if (_wr == null) AddWorkerRequest();
            return _wr;
        }

        #endregion 

        #region Images

        public FluentRecordBase AddRepoImage()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoI = new ImageRepository(_dbFactory);
            return this;
        }

        public ImageRepository ToRepoImage()
        {
            if (_repoI == null) AddRepoImage();
            return _repoI;
        }

        public FluentRecordBase AddServImage()
        {
            //
            // DEPENDENCIES
            if (_repoI == null) AddRepoImage();
            if (_uow == null) AddUOW();
            _servI = new ImageService(_repoI, _uow);
            return this;
        }

        public ImageService ToServImage()
        {
            if (_servI == null) AddServImage();
            return _servI;
        }

        public FluentRecordBase AddImage(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servI == null) AddServImage();
            //
            // ARRANGE
            _i = (Image)Records.image.Clone();
            if (datecreated != null) _i.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _i.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servI.Create(_i, _user);
            return this;
        }

        public Image ToImage()
        {
            if (_i == null) AddImage();
            return _i;
        }

        #endregion 

        #region Lookups

        public FluentRecordBase AddRepoLookup()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoL = new LookupRepository(_dbFactory);
            return this;
        }

        public LookupRepository ToRepoLookup()
        {
            if (_repoL == null) AddRepoLookup();
            return _repoL;
        }

        public FluentRecordBase AddServLookup()
        {
            //
            // DEPENDENCIES
            if (_repoL == null) AddRepoLookup();
            if (_uow == null) AddUOW();
            _servL = new LookupService(_repoL, _uow);
            return this;
        }

        public LookupService ToServLookup()
        {
            if (_servL == null) AddServLookup();
            return _servL;
        }

        public FluentRecordBase AddLookup(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servL == null) AddServLookup();
            //
            // ARRANGE
            _l = (Lookup)Records.lookup.Clone();
            if (datecreated != null) _l.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _l.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            _servL.Create(_l, _user);
            return this;
        }

        public Lookup ToLookup()
        {
            if (_l == null) AddLookup();
            return _l;
        }

        #endregion 

        #region Activitys

        public FluentRecordBase AddRepoActivity()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoA = new ActivityRepository(_dbFactory);
            return this;
        }

        public ActivityRepository ToRepoActivity()
        {
            if (_repoA == null) AddRepoActivity();
            return _repoA;
        }

        public FluentRecordBase AddServActivity()
        {
            //
            // DEPENDENCIES
            if (_repoA == null) AddRepoActivity();
            if (_servAS == null) AddServActivitySignin();
            if (_uow == null) AddUOW();
            _servA = new ActivityService(_repoA, _servAS, _uow);
            return this;
        }

        public ActivityService ToServActivity()
        {
            if (_servA == null) AddServActivity();
            return _servA;
        }

        public FluentRecordBase AddActivity(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            DateTime? startTime = null,
            DateTime? endTime = null
        )
        {
            //
            // DEPENDENCIES
            if (_servA == null) AddServActivity();
            //
            // ARRANGE
            _a = (Activity)Records.activity.Clone();
            if (datecreated != null) _a.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _a.dateupdated = (DateTime)dateupdated;
            if (startTime != null) _a.dateStart = (DateTime)startTime;
            if (endTime != null) _a.dateEnd = (DateTime)endTime;
            //
            // ACT
            _servA.Create(_a, _user);
            return this;
        }

        public Activity ToActivity()
        {
            if (_a == null) AddActivity();
            return _a;
        }

        #endregion 

        #region ActivitySignins

        public FluentRecordBase AddRepoActivitySignin()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoAS = new ActivitySigninRepository(_dbFactory);
            return this;
        }

        public ActivitySigninRepository ToRepoActivitySignin()
        {
            if (_repoAS == null) AddRepoActivitySignin();
            return _repoAS;
        }

        public FluentRecordBase AddServActivitySignin()
        {
            //
            // DEPENDENCIES
            if (_repoAS == null) AddRepoActivitySignin();
            if (_repoW == null) AddRepoWorker();
            if (_repoL == null) AddRepoImage();
            if (_repoAS == null) AddRepoWorkerRequest();
            if (_uow == null) AddUOW();
            _servAS = new ActivitySigninService(_repoAS, _repoW, _repoP, _repoI, _repoWR, _uow);
            return this;
        }

        public ActivitySigninService ToServActivitySignin()
        {
            if (_servAS == null) AddServActivitySignin();
            return _servAS;
        }

        public FluentRecordBase AddActivitySignin(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_a == null) AddActivity();
            if (_w == null) AddWorker();
            //
            // ARRANGE
            _as = (ActivitySignin)Records.activitysignin.Clone();
            _as.Activity = _a;
            if (datecreated != null) _as.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _as.dateupdated = (DateTime)dateupdated;
            _as.dwccardnum = _w.dwccardnum;
            //
            // ACT
            _servAS.Create(_as, _user);
            return this;
        }

        public ActivitySignin ToActivitySignin()
        {
            if (_as == null) AddActivitySignin();
            return _as;
        }

        #endregion 


        public FluentRecordBase AddUOW()
        {
            _uow = new UnitOfWork(_dbFactory);
            return this;
        }

        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
