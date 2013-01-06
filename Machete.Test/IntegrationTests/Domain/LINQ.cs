#region COPYRIGHT
// File:     LINQ.cs
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
using System.Diagnostics;
using Machete.Data;
using System.Data.Entity;

namespace Machete.Test
{
    [TestClass]
    public class LINQ
    {
        /// <summary>
        /// Testing outer join of linq to entities
        /// </summary>
        [TestMethod]
        public void LINQ_outer_join_test()
        {
            //Arrange
            var persons = new List<Person>();
            persons.Add(new Person { ID = 1, firstname1 = "foo1" });
            persons.Add(new Person { ID = 2, firstname1 = "foo2" });
            persons.Add(new Person { ID = 3, firstname1 = "foo3" });
            persons.Add(new Person { ID = 4, firstname1 = "foo4" });

            var workers = new List<Worker>();
            workers.Add(new Worker { ID = 1, dwccardnum = 12345, height = "h1" });
            workers.Add(new Worker { ID = 2, dwccardnum = 12346, height = "h2" });
            workers.Add(new Worker { ID = 3, dwccardnum = 12347, height = "h3" });

            var signins = new List<WorkerSignin>();
            signins.Add(new WorkerSignin { dwccardnum = 12345, dateforsignin = DateTime.Now });
            signins.Add(new WorkerSignin { dwccardnum = 12346, dateforsignin = DateTime.Now });
            signins.Add(new WorkerSignin { dwccardnum = 12347, dateforsignin = DateTime.Now }); 

            var q = from p in persons
                    join w in workers on p.ID equals w.ID into outer
                    from w in outer.DefaultIfEmpty()
                    select new
                    {
                        Name = p.firstname1,
                        height = ((w == null) ? "(no worker)" : w.height)
                    };

            foreach (var i in q)
            {
                Debug.WriteLine("Customer: {0}  Order Number: {1}",
                    i.Name.PadRight(11, ' '), i.height);
            }

            var q2 = from s in signins
                     join w in workers on s.dwccardnum equals w.dwccardnum into outer
                     from ps in outer.DefaultIfEmpty()
                     join p in persons on ps.ID equals p.ID
                     select new { s.dwccardnum, s.dateforsignin, ps.raceother, p.firstname1 };

            foreach (var ii in q2)
            {
                Debug.WriteLine("Customer: {0}  dateforsignin: {1}",
                        ii.dwccardnum.ToString().PadRight(11, ' '), ii.dateforsignin);
            }
        }
    }
}
