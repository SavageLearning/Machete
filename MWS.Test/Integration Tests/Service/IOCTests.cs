using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Test;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWS.Core;
using MWS.Core.Providers;
using MWS.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Transactions;

namespace MWS.Test.Integration_Tests.Service
{
    [TestClass]
    public class IOCTests
    {
        FluentRecordBase frb;

        [TestInitialize]
        public void TestInitialize()
        {
            //frb = new FluentRecordBase();
            //frb.Initialize(new MacheteInitializer(), "macheteConnection");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //frb.Dispose();
            //frb = null;
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Build_returns_container()
        {
            // Arrange
            frb = new FluentRecordBase();
            var bootstrapper = new InstallBootstrapper();
            IUnityContainer container = bootstrapper.Build();
            // Act
            IEnumerable<Installer> result = container.ResolveAll<Installer>();
            
            // Assert
            Assert.AreEqual(2, result.ToArray().Count(), "InstallBootstrapper for service does not return expected number of objects");
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ProjectInstaller_MWS_Program_Main_gets_to_service()
        {
            var mws = new MacheteWindowsService();
            var result = mws.instances;
            Assert.AreEqual(2,result.Count());
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_getMultipleDatabaseFactories()
        {
            var sbs = new ServiceBootstrapper();
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
             var foo = cfg.Instances;
            var container = sbs.Build(foo);

            Assert.AreNotEqual(
                container.Resolve<IDatabaseFactory>(cfg.Instances[0].Name),
                container.Resolve<IDatabaseFactory>(cfg.Instances[1].Name)
                );

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_proveUnityContainerFunctionality()
        {
            var container = new UnityContainer();
            container.RegisterType<IDatabaseFactory, DatabaseFactory>("foo",
                new InjectionConstructor("macheteConnection")
                );
            container.RegisterType<IDatabaseFactory, DatabaseFactory>("bar",
                new InjectionConstructor("ELMAHlog")
                );

            var foo = container.Resolve<IDatabaseFactory>("foo");
            var bar = container.Resolve<IDatabaseFactory>("bar");

            Assert.AreNotEqual(foo, bar);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_getMultipleUOWs()
        {
            var sbs = new ServiceBootstrapper();
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var container = sbs.Build(cfg.Instances);

            Assert.AreNotEqual(
                container.Resolve<IUnitOfWork>(cfg.Instances[0].Name),
                container.Resolve<IUnitOfWork>(cfg.Instances[1].Name)
                );

            var foo = container.Resolve<IUnitOfWork>(cfg.Instances[0].Name);
            var bar = container.Resolve<IUnitOfWork>(cfg.Instances[0].Name);

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_getMultipleEmailServices()
        {
            var sbs = new ServiceBootstrapper();
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var container = sbs.Build(cfg.Instances);

            Assert.AreNotEqual(
                container.Resolve<IEmailService>(cfg.Instances[0].Name),
                container.Resolve<IEmailService>(cfg.Instances[1].Name)
                );

            var foo = container.Resolve<IEmailService>(cfg.Instances[0].Name);
            var bar = container.Resolve<IEmailService>(cfg.Instances[1].Name);

        }


        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_dbDisposeThenFactoryGet()
        {
            var sbs = new ServiceBootstrapper();
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var container = sbs.Build(cfg.Instances);


            var foo = container.Resolve<IEmailServiceProvider>(cfg.Instances[0].Name);
            var bar = container.Resolve<IEmailServiceProvider>(cfg.Instances[1].Name);

            Assert.AreNotEqual(foo, bar);

            var dbf = container.Resolve<IDatabaseFactory>(cfg.Instances[0].Name);
            var db = dbf.Get();
            db.Dispose();
            // need to assert no exception of something
            var emailQueue1 = foo.GetQueueProvider();
            emailQueue1.ProcessQueue(cfg.Instances[0].EmailQueue.EmailServer);

            var emailQueue2 = bar.GetQueueProvider();
            emailQueue2.ProcessQueue(cfg.Instances[1].EmailQueue.EmailServer);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.MWS), TestCategory(TC.Emails)]
        public void Integration_ServiceBootStrapper_testContextIDChange()
        {
            BindingFlags bindFlags = BindingFlags.Instance |
                         BindingFlags.Public |
                         BindingFlags.NonPublic |
                         BindingFlags.Static;
            FieldInfo field = typeof(SqlConnection).GetField("ObjectID", bindFlags);

            var sbs = new ServiceBootstrapper();
            var cfg = ConfigurationManager.GetSection("MacheteWindowsService") as MacheteWindowsServiceConfiguration;
            var container = sbs.Build(cfg.Instances);

            var dbf0 = container.Resolve<IDatabaseFactory>(cfg.Instances[0].Name);
            var db0 = dbf0.Get();
            var conn0 = (db0 as System.Data.Entity.DbContext).Database.Connection;
            var objid0 = field.GetValue(conn0);

            var dbf1 = container.Resolve<IDatabaseFactory>(cfg.Instances[1].Name);
            var db1 = dbf1.Get();
            var conn1 = (db1 as System.Data.Entity.DbContext).Database.Connection;
            var objid1 = field.GetValue(conn1);

            Assert.AreNotEqual(objid0, objid1);
        }
    
    }
    public static class Constants
    {
        public const string foo = "foo";
    }
    //((System.Data.SqlClient.SqlConnection)(((System.Data.Entity.DbContext)(entities)).Database.Connection)).ObjectID
}
