using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machete.Data;
using System.Data.Entity.Database;

namespace Machete.Test.Data
{
    [TestClass]
    public class DatabaseTests
    {
        MacheteContext MacheteDB;

        [TestInitialize]
        public void Initialize()
        {
            DbDatabase.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();

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
    }
}
