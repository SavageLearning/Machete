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
namespace Machete.Domain
{
    public class WorkerRequest : Record
    {
        public WorkerRequest()
        {
            idString = "wkrRequest";
        }

        public int WorkOrderID { get; set; }
        public virtual WorkOrder workOrder { get; set; }
        public int WorkerID { get; set; }
        public virtual Worker workerRequested { get; set; }
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
