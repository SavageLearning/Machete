#region COPYRIGHT
// File:     WorkerTests.cs
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
            Database.SetInitializer<MacheteContext>(new TestInitializer());
            this.MacheteDB = new MacheteContext();
            MacheteDB.Database.Delete();
            MacheteDB.Database.Initialize(true);
        }
        /// <summary>
        /// Inspecting how/when EntityFramework makes the link between parent/child records
        /// </summary>
        [TestMethod]
        public void Integration_Worker_add_worker_check_person_link() 
        {
            //Arrange

            Person _p = (Person)Records.person.Clone();
            Worker _w = (Worker)Records.worker.Clone();
            _w.Person = _p;
            _p.firstname2 = "Worker_add_simple_record";
            _w.height = "Worker_add_simple_record";
            MacheteDB.Workers.Add(_w);
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
                ex.GetType(), Domain.Entities.RootException.Get(ex, "WorkerTests")));
            }
            //Assert
            Assert.IsNotNull(_p.Worker);
            Assert.IsNotNull(_w.Person);
            Assert.IsInstanceOfType(_w.Person, typeof(Person));
            Assert.IsInstanceOfType(_p.Worker, typeof(Worker));
            Assert.IsTrue(_p.ID == 1);
            Assert.IsTrue(_w.ID == 1);
        }
        /// <summary>
        /// Testing when Ef commits and the resulting order
        /// </summary>
        [TestMethod]
        public void Integration_w_verify_identity_assignment_order()
        {
            //
            //Arrange
            // Person1
            Person _p1 = (Person)Records.person.Clone();
            _p1.ID = 1;
            _p1.firstname2 = "Worker_add_multiple_ps_ws";
            // Person 2
            Person _p2 = (Person)Records.person.Clone();
            _p2.ID = 3;
            _p2.firstname2 = "Worker_add_multiple_ps_ws";
            // Person 3
            Person _p3 = (Person)Records.person.Clone();
            _p3.ID = 2;
            _p3.firstname2 = "Worker_add_multiple_ps_ws";
            // Worker 1
            Worker _w1 = (Worker)Records.worker.Clone();
            _w1.ID = 1;
            _w1.height = "Worker_add_multiple_ps_ws";
            // Worker 2
            Worker _w2 = (Worker)Records.worker.Clone();
            _w2.ID = 3;
            _w2.height = "Worker_add_multiple_ps_ws";

            _p2.Worker = _w2;
            _p1.Worker = _w1;
            MacheteDB.Persons.Add(_p1);
            MacheteDB.Persons.Add(_p3);
            //
            //Act
            try
            {
                MacheteDB.SaveChanges();
                MacheteDB.Persons.Add(_p2);
                MacheteDB.SaveChanges();
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                e.GetType(), e.Message));
            }
            //
            //Assert
            Assert.IsTrue(_p1.ID == 1);
            Assert.IsTrue(_p2.ID == 3);
            Assert.IsNotNull(_p1.Worker);
            Assert.IsNotNull(_p2.Worker);
            Assert.IsNull(_p3.Worker);
            //Assert.IsInstanceOfType(_w1.Person, typeof(Person));
        }
        /// <summary>
        /// Testing EF deduplication behavior
        /// </summary>
        [TestMethod]
        public void Integration_Worker_test_deduplication()
        {
            int reccount = 0;
            //
            //Arrange
            Person _p = (Person)Records.person.Clone();
            _p.ID = 0;
            Worker _w = (Worker)Records.worker.Clone();
            _w.ID = 0;
            _p.firstname2 = "Worker_test_deduplication";
            _w.height = "Worker_test_deduplication";
            _w.Person = _p;
            //
            //Act
            try
            {
                MacheteDB.Workers.Add(_w);
                MacheteDB.Persons.Add(_p);
                MacheteDB.Persons.Add(_p);
                MacheteDB.SaveChanges();
                reccount = MacheteDB.Persons.Count(n => n.firstname1 == _p.firstname1);
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
