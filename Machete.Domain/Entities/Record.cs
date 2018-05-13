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
        [Column("Createdby")] // the legacy of 2011 inconsistency
        public string createdby { get; set; }
        [StringLength(30)]
        [Column("Updatedby")] // the legacy of 2011 inconsistency
        public string updatedby { get; set; }
        private static int byLength = 30;
        public Record() {}

        public void updatedByUser(string user)
        {
            var short_user = user.Length <= byLength ? user : user.Substring(0, byLength);

            dateupdated = DateTime.Now;  
            updatedby = short_user;
        }
        public void createdByUser(string user)
        {
            var short_user = user.Length <= byLength ? user : user.Substring(0, byLength);
            datecreated = DateTime.Now;
            createdby = short_user;
            updatedByUser(short_user);
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
