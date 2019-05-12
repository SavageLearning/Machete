#region COPYRIGHT
// File:     IntegrationServiceHelpers.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17
// License:  GPL v3
// Project:  Machete.Test.Old
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
using System.IO;
using System.Text;
using AutoMapper;
using Machete.Data;
using Machete.Data.Repositories;
using Machete.Domain;
using Machete.Service;
using Machete.Web;
using Machete.Web.Maps;
using Machete.Web.Maps.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Machete.Test.Integration.Fluent
{
    public partial class FluentRecordBase : IDisposable
    {
        private IWorkerService _servW;
        private IImageService _servI;
        private IWorkerRequestService _servWR;
        private IActivityService _servA;
        private IActivitySigninService _servAS;
        private IEmailService _servEM;
        private IEventService _servEV;
        private ILookupService _servL;
        private Email _email;
        private Event _event;
        private Worker _w;
        private WorkerRequest  _wr;
        private Activity  _a;
        private ActivitySignin _as;
        private Lookup _l;
        private Image _i;
        private string _user = "FluentRecordBase";
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private IMapper _webMap;
        private IMapper _apiMap;
        private readonly IServiceProvider container;

        public FluentRecordBase() {
            var webHost = new WebHostBuilder()
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");

                    if (host.HostingEnvironment.IsDevelopment())
                        config.AddUserSecrets<Startup>();
                })
                .UseKestrel()
                .ConfigureLogging((app, logging) =>
                {
                    logging.AddConfiguration(app.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>().Build().CreateOrMigrateDatabase();//.Run()
                
            var serviceScope = webHost.Services.CreateScope();
            
            container = serviceScope.ServiceProvider;

            ToServ<ILookupService>().populateStaticIds();
        }

        public void Dispose()
        {
//            if (_dbContext == null) _dbContext = container.GetRequiredService<MacheteContext>();
//            _dbContext.Dispose();
        }

        public MacheteContext ToFactory()
        {
//            return _dbContext ?? (_dbContext = container.GetRequiredService<MacheteContext>());
            return container.GetRequiredService<MacheteContext>();
        }

        public T ToServ<T>()
        {
            return container.GetRequiredService<T>();
            //return default(T);
        }
        
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
            // ARRANGE
            if (_p == null) AddPerson();
            _servW = container.GetRequiredService<IWorkerService>();
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
            _w.dwccardnum = GetNextMemberID();

            // ACT
            _servW.Create(_w, _user);
            return this;
        }

        public int GetNextMemberID()
        {
            var dbContext = container.GetRequiredService<MacheteContext>();
            return Records.GetNextMemberID(dbContext.Workers);
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
            _servWR = container.GetRequiredService<IWorkerRequestService>();
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
            
            // ACT //huh?
//            var Wentry = _dbContext.Entry(_w);
//            var WOentry = _dbContext.Entry(_wo);
//            var WRentry = _dbContext.Entry(_wr);
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
            _servI = container.GetRequiredService<IImageService>();
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
       

        public FluentRecordBase AddLookup(
            DateTime? datecreated = null,
            DateTime? dateupdated = null
        )
        {
            //
            // DEPENDENCIES
            _servL = container.GetRequiredService<ILookupService>();
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

        #region Activities
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
            _servA = container.GetRequiredService<IActivityService>();
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

        public FluentRecordBase AddActivitySignin(
            DateTime? datecreated = null,
            DateTime? dateupdated = null,
            Worker worker = null
        )
        {
            //
            // DEPENDENCIES
            if (_a == null) AddActivity();
            _servAS = container.GetRequiredService<IActivitySigninService>();
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
            return container.GetRequiredService<IReportsRepository>();
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
            _servEM = container.GetRequiredService<IEmailService>();
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
            _servEM.Create(_email, _user); // this is done twice and it's causing an error with EF Core
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

        public IMapper ToWebMapper()
        {
            if (_webMap != null) return _webMap;
            
            var mapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            _webMap = mapperConfig.CreateMapper();
            
            return _webMap;
        }

        public IMapper ToApiMapper()
        {
            if (_apiMap != null) return _apiMap;

            var apiConfig = new MapperConfiguration(config => { config.ConfigureApi(); });
            _apiMap = apiConfig.CreateMapper();

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
