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
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            MacheteDB = new MacheteContext();
        }
        /// <summary>
        /// Tests permissions to drop and re-create database
        /// </summary>
        [TestMethod]
        public void DbSet_Initializer_create_machete()
        {
            
            //Arrange
            MacheteDB.Database.Delete();
            //Act
            try
            {
                MacheteDB.Database.Initialize(true); // should be performed by TestInitializer
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
            
        }
        /// <summary>
        /// Used with SQL Profiler to see what SQL is produced
        /// </summary>
        [TestMethod]
        public void DbSet_Queryable_test()
        {
            // Arrange - load test records
            Records.Initialize(MacheteDB);
            DbSet<WorkerSignin> dbset = MacheteDB.Set<WorkerSignin>();
            IQueryable<WorkerSignin> queryable = dbset.AsQueryable();     
            DateTime datestr = DateTime.Today;
            // Act
            queryable = queryable.Where(r => r.dwccardnum == 30040 && EntityFunctions.DiffDays(r.dateforsignin, datestr) == 0 ? true : false);           
            WorkerSignin result = queryable.FirstOrDefault();
            // Assert
            Assert.IsNotNull(result.ID);
            Assert.AreEqual(result.WorkerID, 1);
            Assert.AreEqual(result.dwccardnum, 30040);
            //exec sp_executesql N'SELECT TOP (1) 
            //[Extent1].[ID] AS [ID], 
            //[Extent1].[dwccardnum] AS [dwccardnum], 
            //[Extent1].[WorkerID] AS [WorkerID], 
            //[Extent1].[WorkAssignmentID] AS [WorkAssignmentID], 
            //[Extent1].[dateforsignin] AS [dateforsignin], 
            //[Extent1].[lottery_timestamp] AS [lottery_timestamp], 
            //[Extent1].[datecreated] AS [datecreated], 
            //[Extent1].[dateupdated] AS [dateupdated], 
            //[Extent1].[Createdby] AS [Createdby], 
            //[Extent1].[Updatedby] AS [Updatedby]
            //FROM [dbo].[WorkerSignins] AS [Extent1]
            //WHERE (CASE WHEN ((30040 = [Extent1].[dwccardnum]) AND (0 = (DATEDIFF (day, [Extent1].[dateforsignin], @p__linq__0)))) 
            //THEN cast(1 as bit) ELSE cast(0 as bit) END) = 1',N'@p__linq__0 datetime2(7)',@p__linq__0='2011-10-31 00:00:00'
        }
        /// <summary>
        /// Used with SQL profiler to see what SQL is produced
        /// </summary>
        [TestMethod]
        public void DbSet_Enumerable_test()
        {
            // Arrange - load test records
            Records.Initialize(MacheteDB);
            DbSet<WorkerSignin> dbset = MacheteDB.Set<WorkerSignin>();
            IEnumerable<WorkerSignin> enumerable = dbset.AsEnumerable();
            DateTime datestr = DateTime.Today;
            // Act
            enumerable = enumerable.Where(r => r.dwccardnum == 30040 && DateTime.Compare(r.dateforsignin.Date, datestr) == 0 ? true : false);
            WorkerSignin result = enumerable.FirstOrDefault();
            // Assert
            Assert.IsNotNull(result.ID);
            Assert.AreEqual(result.WorkerID, 1);
            Assert.AreEqual(result.dwccardnum, 30040);
            //SELECT 
            //[Extent1].[ID] AS [ID], 
            //[Extent1].[dwccardnum] AS [dwccardnum], 
            //[Extent1].[WorkerID] AS [WorkerID], 
            //[Extent1].[WorkAssignmentID] AS [WorkAssignmentID], 
            //[Extent1].[dateforsignin] AS [dateforsignin], 
            //[Extent1].[lottery_timestamp] AS [lottery_timestamp], 
            //[Extent1].[datecreated] AS [datecreated], 
            //[Extent1].[dateupdated] AS [dateupdated], 
            //[Extent1].[Createdby] AS [Createdby], 
            //[Extent1].[Updatedby] AS [Updatedby]
            //FROM [dbo].[WorkerSignins] AS [Extent1]
        }
    }
}
