#region COPYRIGHT
// File:     WorkerSignin.cs
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

namespace Machete.Domain
{
    public class WorkerSignin : Signin 
    {
        public WorkerSignin()
        {
            idString = "wsi";
        }
        public int? WorkAssignmentID { get; set; }
        public DateTime? lottery_timestamp { get; set; }
        public int? lottery_sequence { get; set; }
        public virtual Worker worker { get; set; }
        public int? WorkerID { get; set; }

    }
    public abstract class Signin : Record
    {
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Resources.Worker))]
        [LocalizedDisplayName("dwccardnum", NameResourceType = typeof(Resources.Worker))]
        public virtual int dwccardnum { get; set; } 
        public int? memberStatus { get; set; }
        public DateTime dateforsignin { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    //public class signinView : Record
    //{
    //    public int dwccardnum { get; set; }
    //    public string firstname1 { get; set; }
    //    public string firstname2 { get; set; }
    //    public string lastname1 { get; set; }
    //    public string lastname2 { get; set; }
    //    public string fullname
    //    {
    //        get
    //        {
    //            return firstname1 + " " +
    //                    firstname2 + " " +
    //                    lastname1 + " " +
    //                    lastname2;
    //        }
    //        set{}
    //    }
    //    public int signinID { get; set; }
    //    public DateTime dateforsignin { get; set; }
    //    public int? imageID { get; set; }
    //    public DateTime expirationDate { get; set; }
    //    public int memberStatus { get; set; }
    //    public Person p { get; set; }
    //    public Worker w { get; set; }
    //    public Signin s { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="per"></param>
    //    /// <param name="sign"></param>
    //    public signinView(Person per, Signin sign)
    //    {
    //        p = per;
    //        w = p.Worker;
    //        s = sign;
    //        ID = s.ID;
    //        firstname1 = p == null ? null : p.firstname1;
    //        firstname2 = p == null ? null : p.firstname2;
    //        lastname1 = p == null ? null : p.lastname1;
    //        lastname2 = p == null ? null : p.lastname2;
    //        dateforsignin = s.dateforsignin;
    //        dwccardnum = s.dwccardnum;
    //        signinID = s.ID;
    //        dateupdated = s.dateupdated;
    //        datecreated = s.datecreated;
    //        createdby = s.createdby;
    //        updatedby = s.updatedby;
    //        imageID = p == null ? null : p.Worker.ImageID;
    //        expirationDate = p == null ? DateTime.MinValue : p.Worker.memberexpirationdate;
    //        memberStatus = p == null ? 0 : p.Worker.memberStatusID;

    //    }
    //    public signinView() { }
    //}
}
