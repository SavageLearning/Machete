using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Machete.Test.IntegrationTests.System
{
    [TestClass]
    public class WorkerCacheTests
    {
        MacheteContext DB;

        [TestInitialize]
        public void TestInitialize()
        {

            Database.SetInitializer<MacheteContext>(new TestInitializer());
            this.DB = new MacheteContext();
            DB.Database.Delete(); // double delete because of DB isnt clearing for all tests
            DB.Database.Initialize(true); 
            LookupCache.Initialize(DB); //Needed before WorkerCache
            recInitialize(DB);
        }

        [TestMethod]
        public void DbSet_WorkerCache_ExpireMembers_expires_1_active()
        {

            //Arrange
            //Act
            WorkerCache.ExpireMembers(DB);
            IEnumerable<Worker> result = DB.Workers.AsEnumerable()
                .Where(p => p.memberStatus == Worker.iExpired);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to expire members");
        }

        [TestMethod]
        public void DbSet_WorkerCache_ExpireMembers_doesnt_expire_1_inactive()
        {

            //Arrange
            var wkr = DB.Workers.Single(w => w.dwccardnum == 30040);
            wkr.memberStatus = Worker.iInactive;
            DB.SaveChanges();
            //Act
            WorkerCache.ExpireMembers(DB);
            IEnumerable<Worker> result = DB.Workers.AsEnumerable()
                .Where(p => p.memberStatus == Worker.iExpired);
            //Assert
            Assert.AreEqual(0, result.Count(), "Failed to expire members");
        }

        [TestMethod]
        public void DbSet_WorkerCache_ReactivateMembers_activates_1_sanctioned()
        {

            //Arrange
            //Act
            WorkerCache.ReactivateMembers(DB);
            IEnumerable<Worker> result = DB.Workers.AsEnumerable()
                .Where(p => p.memberStatus == Worker.iActive);
            //Assert
            Assert.AreEqual(3, result.Count(), "Failed to reactivate members");
        }

        [TestMethod]
        public void DbSet_WorkerCache_ExpireMembers_doesnt_activate_1_current_sanction()
        {

            //Arrange
            var wkr = DB.Workers.Single(w => w.dwccardnum == 30042);
            wkr.memberReactivateDate = DateTime.Now.AddMonths(1);
            DB.SaveChanges();
            //Act
            WorkerCache.ReactivateMembers(DB);
            IEnumerable<Worker> result = DB.Workers.AsEnumerable()
                .Where(p => p.memberStatus == Worker.iSanctioned);
            //Assert
            Assert.AreEqual(1, result.Count(), "Failed to reactivate members");
        }

        private static void recInitialize(MacheteContext DB)
        {
            Person p1 = (Person)Records.person.Clone(); 
            p1.Worker = (Worker)Records.worker.Clone(); 
            p1.Worker.dwccardnum = 30040; p1.Worker.skill1 = 62;
            p1.Worker.memberStatus = Worker.iActive;
            p1.Worker.memberexpirationdate = DateTime.Now.AddDays(-1);
            p1.Worker.Person = p1;
            DB.Persons.Add(p1);
            DB.Workers.Add(p1.Worker);
            Person p2 = (Person)Records.person.Clone(); 
            DB.Persons.Add(p2); p2.Worker = (Worker)Records.worker.Clone(); 
            p2.Worker.dwccardnum = 30041;
            p2.Worker.memberStatus = Worker.iActive;
            p2.Worker.memberexpirationdate = DateTime.Now.AddDays(1);
            Person p3 = (Person)Records.person.Clone(); 
            DB.Persons.Add(p3); p3.Worker = (Worker)Records.worker.Clone(); 
            p3.Worker.dwccardnum = 30042;
            p3.Worker.memberReactivateDate = DateTime.Now.AddDays(-1);
            p3.Worker.memberStatus = Worker.iSanctioned;
            p3.Worker.memberexpirationdate = DateTime.Now.AddDays(1);
            DB.SaveChanges();
        }
    }
}
