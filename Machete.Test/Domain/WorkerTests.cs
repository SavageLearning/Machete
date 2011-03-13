using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;
using System.Data.Entity.Database;
using System.Data.Entity.Infrastructure;
using Machete.Service;
using System.Data.Entity.Validation;
namespace Machete.Test
{
    [TestClass]
    public class WorkerTests
    {
        MacheteContext MacheteDB;

        [TestInitialize]
        public void Initialize()
        {
            DbDatabase.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();
        }
        
        [TestMethod]
        public void Worker_add_one_person_one_worker() 
        {
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person1 = Records._person1;
            Worker _worker1 = Records._worker1;
            _worker1.Person = _person1;
            _person1.firstname2 = "Worker_add_simple_record";
            _worker1.height = "Worker_add_simple_record";
            MacheteDB.Workers.Add(_worker1);
            //Act
            try
            {
                MacheteDB.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Assert.Fail(string.Format("Validation exception for field {0} caught: {1}",
                    ex.EntityValidationErrors.First().ValidationErrors.First().PropertyName,
                    ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage));
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                ex.GetType(), ex.Message));
            }
            //Assert
            Assert.IsNotNull(_person1.Worker);
            Assert.IsNotNull(_worker1.Person);
            Assert.IsInstanceOfType(_worker1.Person, typeof(Person));
            Assert.IsInstanceOfType(_person1.Worker, typeof(Worker));
            Assert.IsTrue(_person1.ID == 1);
            Assert.IsTrue(_worker1.ID == 1);
        }

        [TestMethod]
        public void Worker_add_multiple_persons_workers()
        {
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            Person _person1 = Records._person1;
            Person _person2 = Records._person2;
            Person _person3 = Records._person3;
            Worker _worker1 = Records._worker1;
            Worker _worker2 = Records._worker2;
            _person1.firstname2 = "Worker_add_multiple_persons_workers";
            _person2.firstname2 = "Worker_add_multiple_persons_workers";
            //_person2.Worker = _worker2;
            _person3.firstname2 = "Worker_add_multiple_persons_workers";
            _worker1.height = "Worker_add_multiple_persons_workers";
            _worker2.height = "Worker_add_multiple_persons_workers";
            MacheteDB.Persons.Add(_person1);
            MacheteDB.Persons.Add(_person3);
            //
            //Act
            //try
            //{
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person2);
                MacheteDB.SaveChanges();
                _worker1.Person = _person1;
                MacheteDB.Workers.Add(_worker1);
                //TODO: DBUpdat Exception here. Add a record after already added for error.
                MacheteDB.SaveChanges();

            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
            //    e.GetType(), e.Message));
            //}
            //
            //Assert
            Assert.IsTrue(_person1.ID == 1);
            Assert.IsTrue(_person2.ID == 3);
            Assert.IsNotNull(_person1.Worker);
            Assert.IsNotNull(_person2.Worker);
            Assert.IsNull(_person3.Worker);
            Assert.IsInstanceOfType(_worker1.Person, typeof(Person));
        }
        [TestMethod]
        public void Worker_test_deduplication()
        {
            int reccount = 0;
            //
            //Arrange
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
            //MacheteDB.Configuration.AutoDetectChangesEnabled = true;
            Person _person2 = Records._person2;
            Worker _worker2 = Records._worker2;
            _person2.firstname2 = "Worker_test_deduplication";
            _worker2.height = "Worker_test_deduplication";
            _worker2.Person = _person2;
            //
            //Act
            //try
            //{
                MacheteDB.Workers.Add(_worker2);
                MacheteDB.SaveChanges();
                MacheteDB.Commit();
                MacheteDB.Persons.Add(_person2);
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_person2);
                MacheteDB.SaveChanges();
                reccount = MacheteDB.Persons.Count(n => n.firstname1 == _person2.firstname1);
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
            //    e.GetType(), e.Message));
            //}
            //
            //Assert
            Assert.IsTrue(reccount == 1, "Deduplication of records failed.");

        }
    }
}
