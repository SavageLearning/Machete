#region COPYRIGHT
// File:     Record.cs
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    public class Record : ICloneable
    {
        [NotMapped]
        public string idString { get; set; }
        public int ID { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        [StringLength(30)]
        public string Createdby { get; set; }
        [StringLength(30)]
        public string Updatedby { get; set; }

        public Record() {}

        public void updatedby(string user)
        {            
            dateupdated = DateTime.Now;  
            Updatedby = user;
        }
        public void createdby(string user)
        {
            datecreated = DateTime.Now;
            Createdby = user;
            updatedby(user);
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string idPrefix
        {
            get
            {
                return idString + this.ID.ToString() + "-";
            }
        }
    }
}
