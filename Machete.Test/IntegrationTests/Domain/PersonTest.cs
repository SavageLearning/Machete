using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Machete.Service;
namespace Machete.Test
{
    [TestClass]
    public class PersonTest
    {
        MacheteContext MacheteDB;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();

        }

        [TestMethod]
        public void DbSet_Person_add_simple_record()
        {
            //
            //Arrange
            int reccount = 0;
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person1 = Records._person1; _person1.ID = 0;
            Person _person2 = Records._person2; _person2.ID = 0;
            Person _person3 = Records._person3; _person3.ID = 0;
            _person1.firstname2 = "Person_add_simple_record";
            _person2.firstname2 = "Person_add_simple_record";
            _person3.firstname2 = "Person_add_simple_record";
            //
            //Act
            try
            {
                MacheteDB.Persons.Add(_person1);
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person3);
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person2);
                //TODO: DBUpdat Exception here. Add a record after already added for error.
                MacheteDB.SaveChanges();
                reccount = MacheteDB.Persons.Count();
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                e.GetType(), e.Message));
            }
            //
            //Assert
            Assert.IsTrue(_person1.ID == 1);
            Assert.IsTrue(_person2.ID == 3, "Expected ID of 3, found ID of \"{0}\"", _person2.ID);
            Assert.IsTrue(reccount == 3, "Expected 3 records, found {0}", reccount);
        }
        [TestMethod]
        public void DbSet_Person_test_deduplication()
        {
            int reccount = 0;
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            MacheteDB.Configuration.AutoDetectChangesEnabled = true;
            Person _person2 = Records._person2;
            _person2.firstname2 = "Person_test_deduplication";
            //
            //Act
            try
            {
                MacheteDB.Persons.Add(_person2);
                //MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person2);
                //MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person2);
                MacheteDB.SaveChanges();
                reccount = MacheteDB.Persons.Count(n => n.firstname1 == _person2.firstname1);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                e.GetType(), e.Message));
            }
            //
            //Assert
            Assert.IsTrue(reccount == 1, "Deduplication of records failed.");
        }
    }
}