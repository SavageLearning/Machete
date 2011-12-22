using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using System.Data.Entity;
using Machete.Domain;
using System.Data.Objects;

namespace Machete.Test.Data
{
    [TestClass]
    public class DatabaseTests
    {
        MacheteContext MacheteDB;
        [TestInitialize]
        public void Initialize()
        {
            //var master = new DbContext("master");
            //master.Database.ExecuteSqlCommand("ALTER DATABASE macheteDevTest SET OFFLINE WITH ROLLBACK IMMEDIATE; ALTER DATABASE macheteDevTest SET ONLINE; DROP DATABASE [macheteDevTest]");
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();
            //MacheteDB.Database.ExecuteSqlCommand("USE master; ALTER DATABASE macheteDevTest SET OFFLINE WITH ROLLBACK IMMEDIATE; ALTER DATABASE macheteDevTest SET ONLINE; DROP DATABASE [macheteDevTest]");

        }

        [TestMethod]
        public void DbSet_Initializer_create_machete()
        {
            
            //Arrange
            MacheteDB.Database.Delete();
            //Act
            try
            {
                MacheteDB.Database.Initialize(true);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
            
        }

        [TestMethod]
        public void DbSet_Queryable_test()
        {
            Records.Initialize(MacheteDB);

            var dbset = MacheteDB.Set<WorkerSignin>();
            //var foo1 = dbset.Where(r => r.dwccardnum == 30040).AsQueryable();
            DateTime datestr = DateTime.Parse("8/10/2011");
            Func<WorkerSignin, bool> where = (r => r.dwccardnum == 30040 && EntityFunctions.DiffDays(r.dateforsignin, datestr) == 0 ? true : false);
            var foo1 = dbset.AsQueryable().FirstOrDefault(where);
            Assert.IsNotNull(foo1.ID);
        }

    }
}
