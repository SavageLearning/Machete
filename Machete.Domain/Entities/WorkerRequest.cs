#region COPYRIGHT
// File:     WorkerRequest.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Domain
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;
namespace Machete.Domain
{
    public class WorkerRequest : Record
    {
        public WorkerRequest()
        {
            idString = "wkrFOOBARRequest";
        }
        //public int ID { get; set; }
        public int WorkOrderID { get; set; }

        public virtual WorkOrder workOrder { get; set; }
        //[System.ComponentModel.DataAnnotations.NotMapped]
        //public int dwccardnum { get; set; }
        public int WorkerID { get; set; }
        public virtual Worker workerRequested { get; set; }
        public string fullName
        {
            get {
                Person p = this.workerRequested.Person;
                    return p.firstname1 + " " +
                         p.firstname2 + " " +
                         p.lastname1 + " " +
                         p.lastname2;
                }
        }
        public string fullNameAndID
        {
            get
            {
                Person p = this.workerRequested.Person;
                return this.workerRequested.dwccardnum + " " + 
                     p.firstname1 + " " +
                     p.firstname2 + " " +
                     p.lastname1 + " " +
                     p.lastname2;
            }
        }
    }
    public class WorkerRequestComparer : IEqualityComparer<WorkerRequest>
    {
        bool IEqualityComparer<WorkerRequest>.Equals(WorkerRequest x, WorkerRequest y)
        {
            return x.WorkerID == y.WorkerID ? true : false;
        }
        int IEqualityComparer<WorkerRequest>.GetHashCode(WorkerRequest obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;
            return 1;
        } 
    }
}
