#region COPYRIGHT
// File:     Worker.cs
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Domain
{
    public class Worker : Record
    {
        public static int iActive { get; set; }
        public static int iInactive { get; set; }
        public static int iSanctioned { get; set; }
        public static int iExpired { get; set; }
        public static int iExpelled { get; set; }

        //public int ID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<WorkerSignin> workersignins { get; set; }
        public virtual ICollection<WorkAssignment> workAssignments { get; set; }
        //
        [StringLength(100)]
        public string fullNameAndID { get; set; }

        [Required]
        public int typeOfWorkID { get; set; }
        // typeOfWork is really 'program'; as in, what program at the Center does the worker belong to
        // Program is the letter code displayed instead of the full program name
        public string typeOfWork { get; set; }
        [Required]
        public DateTime dateOfMembership { get; set; }
        public DateTime? dateOfBirth {get; set;}
        [Required, Column("memberStatus")]
        public int memberStatusID { get; set; }
        [StringLength(50)]
        public string memberStatusEN { get; set; }
        [StringLength(50)]
        public string memberStatusES { get; set; }
        public DateTime? memberReactivateDate { get; set; }
        public bool? active { get; set; }
        public bool? homeless { get; set; }
        public int? housingType { get; set; }
        public int? RaceID { get; set; }
        [StringLength(20)]
        public string raceother { get; set; }
        [StringLength(50)]
        public string height { get; set; }
        [StringLength(10)]
        public string weight { get; set; }
        public int englishlevelID { get; set; }
        public bool? recentarrival { get; set; }
        public DateTime? dateinUSA { get; set; }
        public DateTime? dateinseattle { get; set; }
        public bool? disabled { get; set; }
        [StringLength(50)]
        public string disabilitydesc { get; set; }
        public int? maritalstatus { get; set; }
        public bool? livewithchildren { get; set; }
        public bool? liveWithSpouse { get; set; }
        public bool? livealone { get; set; }
        [StringLength(1000)]
        public string liveWithDescription { get; set; }
        [RegularExpression("^([0-9]|[0-1]\\d|20)$")]
        public int? numofchildren { get; set; }
        public int? americanBornChildren { get; set; }
        public int? numChildrenUnder18 { get; set; }
        public int? educationLevel { get; set; }
        public int? farmLaborCharacteristics { get; set; }
        public bool? wageTheftVictim { get; set; }
        public double? wageTheftRecoveryAmount { get; set; }
        public int? incomeID { get; set; }
        [StringLength(50)]
        public string emcontUSAname { get; set; }
        [StringLength(30)]
        public string emcontUSArelation { get; set; }
        [StringLength(14)]
        public string emcontUSAphone { get; set; }
        // TODO: require unique number when EF supports it
        [Required]
        [RegularExpression("^[0-9]{5,5}$")]
        public int dwccardnum { get; set; }
        public int? neighborhoodID { get; set; }
        public bool? immigrantrefugee { get; set; }
        public int? countryoforiginID { get; set; }
        [StringLength(50)]
        public string emcontoriginname { get; set; }
        [StringLength(30)]
        public string emcontoriginrelation { get; set; }
        [StringLength(14)]
        public string emcontoriginphone { get; set; }
        [Required]
        public DateTime memberexpirationdate { get; set; }
        public bool? driverslicense { get; set; }
        public DateTime? licenseexpirationdate { get; set; }
        public bool? carinsurance { get; set; }
        public DateTime? insuranceexpiration { get; set; }
        public DateTime? lastPaymentDate { get; set; }
        public double? lastPaymentAmount { get; set; }
        public bool? ownTools { get; set; }
        public bool? healthInsurance { get; set; }
        public bool? usVeteran { get; set; }
        public DateTime? healthInsuranceDate { get; set; }
        public int? vehicleTypeID { get; set; }
        public int? incomeSourceID { get; set; }
        [StringLength(1000)]
        public string introToCenter { get; set; }
        public int? ImageID { get; set; }
        public int? skill1 { get; set; }
        public int? skill2 { get; set; }
        public int? skill3 { get; set; }
        public string skillCodes { get; set; }
        public float? workerRating { get; set; }
        public bool? lgbtq { get; set; }
        // TODO2017: these should be in automapper profiles
        public bool isActive 
        {
            get { return this.memberStatusID == iActive ? true : false; }
        }
        public bool isInactive
        {
            get { return this.memberStatusID == iInactive ? true : false; }
        }
        public bool isSanctioned
        {
            get { return this.memberStatusID == iSanctioned ? true : false; }
        }
        public bool isExpired
        {
            get { return this.memberStatusID == iExpired ? true : false; }
        }
        public bool isExpelled
        {
            get { return this.memberStatusID == iExpelled ? true : false; }
        }
    }
}

