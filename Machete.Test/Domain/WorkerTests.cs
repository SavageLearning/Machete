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
namespace Machete.Test
{
    [TestClass]
    public class WorkerTests
    {
        [TestMethod]
        public void Worker_add_simple_record()
        {
            //
            //Arrange
            DbDatabase.SetInitializer<MacheteContext>(new MacheteInitializer());
            MacheteContext MacheteDB = new MacheteContext();
            //
            //Act
            try
            {
                Records._person2.Worker = Records._worker2;
                MacheteDB.Persons.Add(Records._person1);
                MacheteDB.Persons.Add(Records._person3);
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(Records._person2);
                MacheteDB.SaveChanges();
                Records._person1.Worker = Records._worker1;
                MacheteDB.Workers.Add(Records._worker1);
                //TODO: DBUpdat Exception here. Add a record after already added for error.
                MacheteDB.SaveChanges();
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                e.GetType(), e.Message));
            }
            //
            //Assert
            Assert.IsTrue(Records._person1.ID == 1);
            Assert.IsTrue(Records._person2.ID == 3);
            Assert.IsNotNull(Records._person1.Worker);
            Assert.IsNotNull(Records._person2.Worker);
            Assert.IsNull(Records._person3.Worker);
            Assert.IsInstanceOfType(Records._worker1.Person, typeof(Person));
        }
    }
}
