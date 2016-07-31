#region COPYRIGHT
// File:     EmployerWoCombined.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
// License:  GPL v3
// Project:  Machete.Web
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
using Machete.Domain;
using System;
using System.ComponentModel.DataAnnotations;


namespace Machete.Web.ViewModel
{
    public class EmployerWoCombined 

    {
        [LocalizedDisplayName("isbusiness", NameResourceType = typeof(Domain.Resources.Employer))]
        public bool business { get; set; }

        [LocalizedDisplayName("businessname", NameResourceType = typeof(Domain.Resources.Employer))]
        public string businessname { get; set; }
        
        [LocalizedDisplayName("name", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "namerequired", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string name { get; set; }
        //
        [LocalizedDisplayName("address1", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "address1required", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string address1 { get; set; }
        //
        [LocalizedDisplayName("address2", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string address2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string state { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [RegularExpression(@"^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string phone { get; set; }
        //
        [LocalizedDisplayName("cellphone", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        //[RequiredIfEmpty("phone", ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string cellphone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string zipcode { get; set; }
        //
        [LocalizedDisplayName("email", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string email { get; set; }
        //
        [LocalizedDisplayName("licenseplate", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string licenseplate { get; set; }
        //
        [LocalizedDisplayName("driverslicense", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string driverslicense { get; set; }
        //
        [LocalizedDisplayName("referredby", NameResourceType = typeof(Domain.Resources.Employer))]
        public int? referredby { get; set; }
        //
        [LocalizedDisplayName("referredbyOther", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string referredbyOther { get; set; }
        //
        [LocalizedDisplayName("blogparticipate", NameResourceType = typeof(Domain.Resources.Employer))]
        public bool blogparticipate { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Domain.Resources.Employer))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.Employer))]
        public string notes { get; set; }
        //
        [LocalizedDisplayName("onlineSource", NameResourceType = typeof(Domain.Resources.Employer))]
        public bool onlineSource { get; set; }
        //
        [LocalizedDisplayName("returnCustomer", NameResourceType = typeof(Domain.Resources.Employer))]
        public bool returnCustomer { get; set; }
        //
        [LocalizedDisplayName("receiveUpdates", NameResourceType = typeof(Domain.Resources.Employer))]
        public bool receiveUpdates { get; set; }
        //
        // WorkOrder field
        //
        //
        [LocalizedDisplayName("contactName", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "contactNamerequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string contactName { get; set; }
        //
        [LocalizedDisplayName("workSiteAddress1", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "workSiteAddress1required", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string workSiteAddress1 { get; set; }
        //
        [LocalizedDisplayName("workSiteAddress2", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string workSiteAddress2 { get; set; }
        //
        [LocalizedDisplayName("city", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "cityrequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string wo_city { get; set; }
        //
        [LocalizedDisplayName("state", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(2, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "staterequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string wo_state { get; set; }
        //
        [LocalizedDisplayName("phone", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(12, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "phonerequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [RegularExpression(@"^$|^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$", ErrorMessageResourceName = "phoneformat", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string wo_phone { get; set; }
        //
        [LocalizedDisplayName("zipcode", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(10, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "zipcode", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string wo_zipcode { get; set; }
        //
        [LocalizedDisplayName("typeOfWorkID", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "typeOfWorkIDrequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public int typeOfWorkID { get; set; }
        //
        [LocalizedDisplayName("englishRequired", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        public bool englishRequired { get; set; }
        //
        [LocalizedDisplayName("englishRequiredNote", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string englishRequiredNote { get; set; }
        //
        [LocalizedDisplayName("lunchSupplied", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        public bool lunchSupplied { get; set; }
        //
        //
        [LocalizedDisplayName("description", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        //[Required(ErrorMessageResourceName = "descriptionRequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public string description { get; set; }
        //
        [LocalizedDisplayName("dateTimeofWork", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "dateTimeofWorkrequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public DateTime dateTimeofWork { get; set; }
        //
        [LocalizedDisplayName("timeFlexible", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        public bool timeFlexible { get; set; }

        [LocalizedDisplayName("transportMethodID", NameResourceType = typeof(Domain.Resources.WorkOrder))]
        [Required(ErrorMessageResourceName = "transportMethodIDrequired", ErrorMessageResourceType = typeof(Domain.Resources.WorkOrder))]
        public int transportMethodID { get; set; }
    }
}
