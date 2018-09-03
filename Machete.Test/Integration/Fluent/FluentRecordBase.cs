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
using System.IO;
using AutoMapper;
using System.Data;
using System.Data.SqlClient;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase :IDisposable
    {
        #region internal fields
        private ActivityRepository _repoA;
        private ActivitySigninRepository _repoAS;
        private ConfigRepository _repoC;
        private WorkerRepository _repoW;
        private WorkerRequestRepository _repoWR;
        private ReportsRepository _repoR;
        private LookupRepository _repoL;
        private ImageRepository _repoI;
        private EmailRepository _repoEM;
        private EventRepository _repoEV;
        private DatabaseFactory _dbFactory;
        private ReadOnlyContext _dbReadOnly;
        private WorkerService _servW;
        private ImageService _servI;
        private ConfigService _servC;
        private WorkerRequestService _servWR;
        private ActivityService _servA;
        private ActivitySigninService _servAS;
        private ReportService _servR;
        private ReportsV2Service _servRV2;
        private EmailService _servEM;
        private EventService _servEV;
        private LookupService _servL;
        private IUnitOfWork _uow;
        private IEmailConfig _emCfg;
        private Email _email;
        private Event _event;
        private Config _config = null;
        private Worker _w;
        private WorkerRequest  _wr;
        private Activity  _a;
        private ActivitySignin _as;
        //private Event _e;
        private Lookup _l;
        private Image _i;
        private string _user = "FluentRecordBase";
        private Random _random = new Random((int)DateTime.Now.Ticks);
        private IMapper _webMap;
        private IMapper _apiMap;

        #endregion

        public FluentRecordBase() { }

        public FluentRecordBase(string user)
        {

            _user = user;
            ToServLookup().populateStaticIds();
        }


        public FluentRecordBase AddDBFactory(string connStringName = "MacheteConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            var initializer = new TestInitializer();
            Database.SetInitializer<MacheteContext>(initializer);
            _dbFactory = new DatabaseFactory(connStringName);
            initializer.InitializeDatabase(_dbFactory.Get());
            _uow = new UnitOfWork(_dbFactory);
            _uow.Commit();

            AddServLookup();
            return this;
        }

        private void AddDBReadonly(string connStringName = "readonlyConnection")
        {
            if (_dbFactory == null) throw new InvalidOperationException("You must first initialize the database.");
            var db = _dbFactory.Get();
            var connection = (db as DbContext).Database.Connection;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "sp_executesql";
                command.CommandType = CommandType.StoredProcedure;
                var param = command.CreateParameter();
                param.ParameterName = "@statement";
                param.Value = @"
CREATE LOGIN readonlyLogin WITH PASSWORD='@testPassword1'
CREATE USER readonlyUser FROM LOGIN readonlyLogin
EXEC sp_addrolemember 'db_datareader', 'readonlyUser';
                    ";
                command.Parameters.Add(param);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {                               // user already exists
                    if (ex.Errors[0].Number.Equals(15025)) { } else throw ex;
                }
            }

            _dbReadOnly = new ReadOnlyContext(connStringName);
        }

        public void Dispose()
        {
            if (_dbFactory == null) AddDBFactory();
            _dbFactory.Dispose();
        }

        public DatabaseFactory ToFactory()
        {
            if (_dbFactory == null) AddDBFactory();
            return _dbFactory;
        }

        public void Reload<T>(T entity) where T : Record
        {
            if (_dbFactory == null) AddDBFactory();
            _dbFactory.Get().Entry<T>(entity).Reload();
        }

        #region Persons
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
            if (_repoWO == null) AddRepoWorkOrder();
            if (_repoP == null) AddRepoPerson();

            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            if (_repoL == null) AddRepoLookup();
            _servW = new WorkerService(_repoW, _repoL, _uow, _repoWA, _repoWO, _repoP, _webMap);
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
            DateTime? dateupdated = null,
            DateTime? memberexpirationdate = null,
            DateTime? memberReactivateDate = null,
            string testID = null
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
            _w.ID = _p.ID; // mimics MVC UI behavior. the POST to create worker includes the person record's ID
            if (skill1 != null) _w.skill1 = skill1;
            if (skill2 != null) _w.skill2 = skill2;
            if (skill3 != null) _w.skill3 = skill3;
            if (status != null) _w.memberStatusID = (int)status;
            if (datecreated != null) _w.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _w.dateupdated = (DateTime)dateupdated;
            if (memberexpirationdate != null) _w.memberexpirationdate = (DateTime)memberexpirationdate;
            if (memberReactivateDate != null) _w.memberReactivateDate = (DateTime)memberReactivateDate;
            if (testID != null) _w.Person.firstname2 = testID;
            // kludge
            _w.dwccardnum = Records.GetNextMemberID(_dbFactory.Get().Workers);
            //
            // ACT
            _servW.Create(_w, _user);
            return this;
        }

        public int GetNextMemberID()
        {
            if (_dbFactory == null) AddDBFactory();
            return Records.GetNextMemberID(_dbFactory.Get().Workers);
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
            _wr.workerRequested = _w;
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
            if (_webMap == null) AddMapper();
            if (_uow == null) AddUOW();
            _servL = new LookupService(_repoL, _webMap, _uow);
            _servL.populateStaticIds();
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
            if (_servL == null) AddServLookup();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            _servA = new ActivityService(_repoA, _servAS, _servL, _uow, _webMap);
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
            DateTime? endTime = null,
            string    teacher = null
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
            if (teacher != null) _a.teacher = teacher;
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
            if (_servW == null) AddServWorker();
            if (_servL == null) AddServImage();
            if (_servAS == null) AddServWorkerRequest();
            if (_uow == null) AddUOW();
            if (_webMap == null) AddMapper();
            if (_servC == null) AddServConfig();

            _servAS = new ActivitySigninService(_repoAS, _servW, _servP, _servI, _servWR, _uow, _webMap, _servC);
            return this;
        }

        public ActivitySigninService ToServActivitySignin()
        {
            if (_servAS == null) AddServActivitySignin();
            return _servAS;
        }

        public FluentRecordBase AddActivitySignin(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            Worker worker = null
        )
        {
            //
            // DEPENDENCIES
            if (_a == null) AddActivity();
            if (worker != null) _w = worker;
            if (_w == null) AddWorker();
            //
            // ARRANGE
            _as = (ActivitySignin)Records.activitysignin.Clone();
            _as.Activity = _a;
            _as.activityID = _a.ID;
            if (datecreated != null) _as.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _as.dateupdated = (DateTime)dateupdated;
            _as.dwccardnum = _w.dwccardnum;
            _as.dateforsignin = DateTime.Now;
            //
            // ACT
            _servAS.CreateSignin(_as, _user);
            return this;
        }

        public ActivitySignin ToActivitySignin()
        {
            if (_as == null) AddActivitySignin();
            return _as;
        }

        #endregion

        #region Reports

        public FluentRecordBase AddServReports()
        {
            //
            // DEPENDENCIES
            if (_repoWO == null) AddRepoWorkOrder();
            if (_repoWA == null) AddRepoWorkAssignment();
            if (_repoW == null) AddRepoWorker();
            if (_repoWSI == null) AddRepoWorkerSignin();
            if (_repoWR == null) AddRepoWorkerRequest();
            if (_repoL == null) AddRepoLookup();
            if (_repoAS == null) AddRepoActivitySignin();
            _servR = new ReportService(_repoWO, _repoWA, _repoW, _repoWSI, _repoWR, _repoL, _repoE, _repoAS);
            return this;
        }

        public ReportService ToServReports()
        {
            if (_servR == null) AddServReports();
            return _servR;
        }

        public FluentRecordBase AddServReportsV2()
        {
            // DEPENDENCIES
            if (_repoWO == null) AddRepoWorkOrder();
            if (_repoWA == null) AddRepoWorkAssignment();
            if (_repoW == null) AddRepoWorker();
            if (_repoR == null) AddRepoReports();
            if (_uow == null) AddUOW();
            if (_apiMap == null) AddMapper();

            _servRV2 = new ReportsV2Service(_repoR, _uow, _apiMap);
            return this;
        }

        public FluentRecordBase AddRepoReports()
        {
            if (_dbFactory == null) AddDBFactory();
            if (_dbReadOnly == null) AddDBReadonly();

            _repoR = new ReportsRepository(_dbFactory, _dbReadOnly);
            return this;
        }

        public ReportsRepository ToRepoReports()
        {
            if (_repoR == null) AddRepoReports();
            return _repoR;
        }

        public ReportsV2Service ToServReportsV2()
        {
            if (_servRV2 == null) AddServReportsV2();
            return _servRV2;
        }



        #endregion

        #region Emails

        public FluentRecordBase AddRepoEmail()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoEM = new EmailRepository(_dbFactory);
            return this;
        }

        public EmailRepository ToRepoEmail()
        {
            if (_repoEM == null) AddRepoEmail();
            return _repoEM;
        }

        public FluentRecordBase AddServEmail()
        {
            //
            // DEPENDENCIES
            if (_repoEM == null) AddRepoEmail();
            if (_servWO == null) AddServWorkOrder();
            if (_uow == null) AddUOW();
            if (_emCfg == null) AddEmailConfig();
            _servEM = new EmailService(_repoEM, _servWO, _uow, _emCfg);
            return this;
        }

        public EmailService ToServEmail()
        {
            if (_servEM == null) AddServEmail();
            return _servEM;
        }

        public FluentRecordBase AddEmail(
            int? status = null,
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            string attachment = null,
            string attachmentType = null
        )
        {
            //
            // DEPENDENCIES
            if (_servEM == null) AddServEmail();
            //
            // ARRANGE
            _email = (Email)Records.email.Clone();
            if (datecreated != null) _email.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _email.dateupdated = (DateTime)dateupdated;
            if (status != null) _email.statusID = (int)status;
            if (attachment != null) _email.attachment = attachment;
            if (attachmentType != null) _email.attachment = attachmentType;
            //
            // ACT
            _servEM.Create(_email, _user);
            return this;
        }

        public Email ToEmail()
        {
            if (_email == null) AddEmail();
            return _email;
        }

        public Email CloneEmail()
        {
            var p = (Email)Records.email.Clone();
            p.emailFrom = "joe@foo.com";
            p.emailTo = "foo@joe.com";
            p.subject = RandomString(5);
            p.body = RandomString(8);
            return p;
        }

        #endregion

        #region Events

        public FluentRecordBase AddRepoEvent()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoEV = new EventRepository(_dbFactory);
            return this;
        }

        public EventRepository ToRepoEvent()
        {
            if (_repoEV == null) AddRepoEvent();
            return _repoEV;
        }

        public FluentRecordBase AddServEvent()
        {
            //
            // DEPENDENCIES
            if (_repoEV == null) AddRepoEvent();
            if (_webMap == null) AddMapper();
            if (_repoL == null) AddRepoLookup();
            if (_uow == null) AddUOW();
            _servEV = new EventService(_repoEV, _uow, _repoL, _webMap);
            return this;
        }

        public EventService ToServEvent()
        {
            if (_servEV == null) AddServEvent();
            return _servEV;
        }

        public FluentRecordBase AddEvent(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            if (_servEV == null) AddServEvent();
            if (_p == null) AddPerson();
            //
            // ARRANGE
            _event = (Event)Records.event1.Clone();
            _event.PersonID = _p.ID;
            if (datecreated != null) _event.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _event.dateupdated = (DateTime)dateupdated;
            _event.updatedby = _user;
            _event.createdby = _user;
            //
            // ACT
            _servEV.Create(_event, _user);
            return this;
        }

        public Event ToEvent()
        {
            if (_event == null) AddEvent();
            return _event;
        }
        #endregion

        #region Configs


        public FluentRecordBase AddRepoConfig()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoC = new ConfigRepository(_dbFactory);
            return this;
        }

        public ConfigRepository ToRepoConfig()
        {
            if (_repoC == null) AddRepoConfig();
            return _repoC;
        }

        public FluentRecordBase AddServConfig()
        {
            //
            // DEPENDENCIES
            if (_repoC == null) AddRepoConfig();
            if (_webMap == null) AddMapper();
            if (_uow == null) AddUOW();
            _servC = new ConfigService(_repoC, _uow, _webMap);
            return this;
        }

        public ConfigService ToServConfig()
        {
            if (_servC == null) AddServConfig();
            return _servC;
        }

        public FluentRecordBase AddConfig()
        {
            //
            // DEPENDENCIES
            if (_servC == null) AddServConfig();

            //
            // ARRANGE
            _config.updatedby = _user;
            _config.createdby = _user;
            //
            // ACT
            _servC.Create(_config, _user);
            return this;
        }

        public Event ToConfig()
        {
            if (_event == null) AddConfig();
            return _event;
        }

        #endregion

        public FluentRecordBase AddMapper()
        {
            _webMap = new Machete.Web.MapperConfig().getMapper();
            _apiMap = new Machete.Api.MapperConfig().getMapper();

            return this;
        }

        public IMapper ToWebMapper()
        {

            if (_webMap == null) AddMapper();
            return _webMap;
        }

        public IMapper ToApiMapper()
        {
            if (_apiMap == null) AddMapper();
            return _apiMap;
        }

        public FluentRecordBase AddUOW()
        {
            _uow = new UnitOfWork(_dbFactory);
            return this;
        }

        public IUnitOfWork ToUOW()
        {
            if (_uow == null) AddUOW();

            return _uow;
        }

        public FluentRecordBase AddEmailConfig()
        {
            if (_servC == null) AddServConfig();
            _emCfg = new EmailConfig(_servC);
            return this;
        }

        public IEmailConfig ToEmailConfig()
        {
            if (_emCfg == null) AddEmailConfig();
            return _emCfg;
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

        public string ValidAttachment
        {
            get
            {
                return @"<!DOCTYPE html>
<html>
<body>

<h1>My First Heading</h1>

<p>My first paragraph.</p>

</body>
</html>";
            }
        }
    }
}
