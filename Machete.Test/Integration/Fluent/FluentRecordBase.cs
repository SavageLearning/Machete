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
using Machete.Web.App_Start;
using Microsoft.Practices.Unity;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase :IDisposable
    {
        #region internal fields
        private IDatabaseFactory _dbFactory;
        private IReadOnlyContext _dbReadOnly;
        private IWorkerService _servW;
        private IImageService _servI;
        private IConfigService _servC;
        private IWorkerRequestService _servWR;
        private IActivityService _servA;
        private IActivitySigninService _servAS;
        private ReportService _servR;
        private ReportsV2Service _servRV2;
        //private EmailService _servEM;
        private IEventService _servEV;
        private ILookupService _servL;
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
        private IUnityContainer container;

        #endregion

        public FluentRecordBase() {
            container = UnityConfig.GetUnityContainer();
            AddDBFactory();
            ToServ<ILookupService>().populateStaticIds();
        }

        public FluentRecordBase AddDBFactory(string connStringName = "macheteConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            var initializer = new TestInitializer();
            Database.SetInitializer<MacheteContext>(initializer);
            _dbFactory = container.Resolve<IDatabaseFactory>();
            initializer.InitializeDatabase(_dbFactory.Get());

            AddDBReadonly(); // need to ceate the readonlylogin account
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

        public IDatabaseFactory ToFactory()
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
            _servW = container.Resolve<IWorkerService>();
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
            _w.dwccardnum = Records.GetNextMemberID(ToFactory().Get().Workers);
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

        public FluentRecordBase AddWorkerRequest(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servWR = container.Resolve<IWorkerRequestService>();
            if (_wo == null) AddWorkOrder();
            if (_w == null) AddWorker();
            //
            // ARRANGE
            _wr = (WorkerRequest)Records.request.Clone();
            //_wr.workOrder = (WorkOrder)_wo.Clone();
            //_wr.workerRequested = (Worker)_w.Clone();
            _wr.workOrder = _wo;
            _wr.workerRequested = _w;
            if (datecreated != null) _wr.datecreated = (DateTime)datecreated;
            if (dateupdated != null) _wr.dateupdated = (DateTime)dateupdated;
            //
            // ACT
            var Wentry = _dbFactory.Get().Entry<Worker>(_w);
            var WOentry = _dbFactory.Get().Entry<WorkOrder>(_wo);
            var WRentry = _dbFactory.Get().Entry<WorkerRequest>(_wr);
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

        public FluentRecordBase AddImage(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servI = container.Resolve<IImageService>();
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
        
        //public ILookupService ToServ<ILookupService>()
        //{
        //    return container.Resolve<ILookupService>();
        //}

        public FluentRecordBase AddLookup(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servL = container.Resolve<ILookupService>();
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


        public T ToServ<T>()
        {
            return container.Resolve<T>();
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
            IActivityService servA = container.Resolve<IActivityService>();
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
            servA.Create(_a, _user);
            return this;
        }

        public Activity ToActivity()
        {
            if (_a == null) AddActivity();
            return _a;
        }

        #endregion

        #region ActivitySignins

        public FluentRecordBase AddActivitySignin(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            Worker worker = null
        )
        {
            //
            // DEPENDENCIES
            if (_a == null) AddActivity();
            _servAS = container.Resolve<IActivitySigninService>();
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

        public IReportsRepository ToRepoReports()
        {
            return container.Resolve<IReportsRepository>();
        }

        #endregion

        #region Emails

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
            IEmailService _servEM = container.Resolve<IEmailService>();
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

        public FluentRecordBase AddEvent(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servEV = ToServ<IEventService>();
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

        public FluentRecordBase AddConfig()
        {
            //
            // DEPENDENCIES
            _servC = ToServ<IConfigService>();

            //
            // ARRANGE
            _config.updatedby = _user;
            _config.createdby = _user;
            //
            // ACT
            _servC.Create(_config, _user);
            return this;
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
