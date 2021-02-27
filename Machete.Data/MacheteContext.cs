#region COPYRIGHT
// File:     MacheteContext.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Machete.Data.Dynamic;
using Machete.Data.Identity;
using Machete.Data.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Activity = Machete.Domain.Activity;

#region SHUTUP RESHARPER
// The purpose of suppressing so many inspections in this case is so that I can
// visually verify the integrity of the file when there are changes.
//
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable InvertIf
// ReSharper disable AssignNullToNotNullAttribute
#endregion

// If you add anything to this file, do it in alphabetical order so that if we have to do a schema compare it is easier
namespace Machete.Data
{
    // http://stackoverflow.com/questions/22105583/why-is-asp-net-identity-identitydbcontext-a-black-box
    public class MacheteContext : IdentityDbContext<MacheteUser, MacheteRole, string,
                                      MacheteUserClaim, MacheteUserRole, IdentityUserLogin<string>,
                                      IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private Tenant _tenant;

        public MacheteContext(DbContextOptions<MacheteContext> options, ITenantService tenantService) : base(options)
        {
            _tenant = tenantService.GetCurrentTenant();
        }

        public MacheteContext(DbContextOptions<MacheteContext> options, Tenant tenant) : base(options)
        {
            _tenant = tenant;
        }
        
        // Machete here defines the data context to use by EF Core convention.
        // Entity Framework will not retrieve or modify types not expressed here.
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivitySignin> ActivitySignins { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Image> Images { get; set; }
        public virtual DbSet<Lookup> Lookups { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ReportDefinition> ReportDefinitions { get; set; }
        public DbSet<ScheduleRule> ScheduleRules { get; set; }
        public DbSet<TransportCostRule> TransportCostRules { get; set; }
        public DbSet<TransportProviderAvailability> TransportProviderAvailabilities { get; set; }
        public DbSet<TransportProvider> TransportProviders { get; set; }
        public DbSet<TransportRule> TransportRules { get; set; }
        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkerRequest> WorkerRequests { get; set; }
        public DbSet<WorkerSignin> WorkerSignins { get; set; }
        //public DbQuery<WOWASummary WOWASummaries { get; set; }

        /// <summary>
        /// Writes the changes for all entities modified since the last save. Modifications are automatically detected.
        /// </summary>
        /// <returns>The number of changes written.</returns>
        /// <exception cref="Exception"></exception>
        public override int SaveChanges()
        {
            // https://github.com/aspnet/EntityFrameworkCore/issues/3680#issuecomment-155502539
            var validationErrors = ChangeTracker
                .Entries<IValidatableObject>()
                .SelectMany(entities => entities.Entity.Validate(null))
                .Where(result => result != ValidationResult.Success);
            
            if (validationErrors.Any()) {
                var details = new StringBuilder();
                var preface = "DbEntityValidation Error: ";
                Trace.TraceInformation(preface);
                details.AppendLine(preface);

                foreach (var validationError in validationErrors) {
                    var line = $"Property: {validationError.MemberNames} Error: {validationError.ErrorMessage}";
                    details.AppendLine(line);
                    Trace.TraceInformation(line);
                }

                throw new Exception(details.ToString());
            }

            return base.SaveChanges();
        }

        public bool IsDead { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_tenant.ConnectionString);
            //optionsBuilder.UseLazyLoadingProxies();
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://stackoverflow.com/a/39577004/2496266
            base.OnModelCreating(modelBuilder);
            
            // IDENTITY //
            //
            // delete *nothing* in this section, comments included; do not move this section or alphabetize it
            // creates the original Identity Core relationships used by Entity Framework, so that we can use
            // Identity without doing a migration for our custom implementations.
            //
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers
            modelBuilder.Entity<MacheteUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
//                b.HasMany(e => e.Tokens)
//                    .WithOne()
//                    .HasForeignKey(ut => ut.UserId)
//                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<MacheteRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
            // //
                        
            // ENTITIES //
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.dateEnd)
                    .HasColumnName("dateEnd")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateStart)
                    .HasColumnName("dateStart")
                    .HasColumnType("datetime");

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.firstID).HasColumnName("firstID");

                entity.Property(e => e.nameID).HasColumnName("name");

                entity.Property(e => e.nameEN)
                    .HasColumnName("nameEN")
                    .HasMaxLength(50);

                entity.Property(e => e.nameES)
                    .HasColumnName("nameES")
                    .HasMaxLength(50);

                entity.Property(e => e.notes)
                    .HasColumnName("notes")
                    .HasMaxLength(4000);

                entity.Property(e => e.recurring).HasColumnName("recurring");

                entity.Property(e => e.teacher)
                    .IsRequired()
                    .HasColumnName("teacher");

                entity.Property(e => e.typeID).HasColumnName("type");

                entity.Property(e => e.typeEN)
                    .HasColumnName("typeEN")
                    .HasMaxLength(50);

                entity.Property(e => e.typeES)
                    .HasColumnName("typeES")
                    .HasMaxLength(50);

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });

            modelBuilder.Entity<ActivitySignin>(entity =>
            {
                entity.HasIndex(e => e.activityID)
                    .HasName("IX_activityID");

                entity.HasIndex(e => e.personID)
                    .HasName("IX_personID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.activityID).HasColumnName("activityID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateforsignin)
                    .HasColumnName("dateforsignin")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dwccardnum).HasColumnName("dwccardnum");

                entity.Property(e => e.memberStatusID).HasColumnName("memberStatus");

                entity.Property(e => e.personID).HasColumnName("personID");

                entity.Property(e => e.timeZoneOffset).HasColumnName("timeZoneOffset");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.Signins)
                    .HasForeignKey(d => d.activityID)
                    .HasConstraintName("FK_dbo.ActivitySignins_dbo.Activities_activityID");

                entity.HasOne(d => d.person)
                    .WithMany(p => p.activitySignins)
                    .HasForeignKey(d => d.personID)
                    .HasConstraintName("FK_dbo.ActivitySignins_dbo.Persons_personID");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.category).HasColumnName("category");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.description).HasColumnName("description");

                entity.Property(e => e.key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(50);

                entity.Property(e => e.publicConfig)
                    .IsRequired()
                    .HasColumnName("publicConfig")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.value)
                    .IsRequired()
                    .HasColumnName("value");
            });

            modelBuilder.Entity<EmailWorkOrder>(entity =>
            {
                entity.HasKey(e => new { e.EmailID, e.WorkOrderID })
                    .HasName("PK_dbo.EmailWorkOrders");

                entity.HasIndex(e => e.EmailID)
                    .HasName("IX_Email_ID");

                entity.HasIndex(e => e.WorkOrderID)
                    .HasName("IX_WorkOrder_ID");

                entity.Property(e => e.EmailID).HasColumnName("Email_ID");

                entity.Property(e => e.WorkOrderID).HasColumnName("WorkOrder_ID");

                entity.HasOne(d => d.Email)
                    .WithMany(p => p.EmailWorkOrders)
                    .HasForeignKey(d => d.EmailID)
                    .HasConstraintName("FK_dbo.EmailWorkOrders_dbo.Emails_Email_ID");
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.attachment).HasColumnName("attachment");

                entity.Property(e => e.attachmentContentType).HasColumnName("attachmentContentType");

                entity.Property(e => e.body)
                    .IsRequired()
                    .HasColumnName("body");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.emailFrom)
                    .HasColumnName("emailFrom")
                    .HasMaxLength(50);

                entity.Property(e => e.emailTo)
                    .IsRequired()
                    .HasColumnName("emailTo")
                    .HasMaxLength(50);

                entity.Property(e => e.lastAttempt)
                    .HasColumnName("lastAttempt")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.statusID).HasColumnName("statusID");

                entity.Property(e => e.subject)
                    .IsRequired()
                    .HasColumnName("subject")
                    .HasMaxLength(100);

                entity.Property(e => e.transmitAttempts).HasColumnName("transmitAttempts");

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.address1)
                    .IsRequired()
                    .HasColumnName("address1")
                    .HasMaxLength(50);

                entity.Property(e => e.address2)
                    .HasColumnName("address2")
                    .HasMaxLength(50);

                entity.Property(e => e.blogparticipate).HasColumnName("blogparticipate");

                entity.Property(e => e.business).HasColumnName("business");

                entity.Property(e => e.businessname).HasColumnName("businessname");

                entity.Property(e => e.cellphone)
                    .HasColumnName("cellphone")
                    .HasMaxLength(12);

                entity.Property(e => e.city)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.driverslicense)
                    .HasColumnName("driverslicense")
                    .HasMaxLength(30);

                entity.Property(e => e.email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.fax)
                    .HasColumnName("fax")
                    .HasMaxLength(12);

                entity.Property(e => e.isOnlineProfileComplete).HasColumnName("isOnlineProfileComplete");

                entity.Property(e => e.licenseplate)
                    .HasColumnName("licenseplate")
                    .HasMaxLength(10);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.notes)
                    .HasColumnName("notes")
                    .HasMaxLength(4000);

                entity.Property(e => e.onlineSigninID)
                    .HasColumnName("onlineSigninID")
                    .HasMaxLength(128);

                entity.Property(e => e.onlineSource).HasColumnName("onlineSource");

                entity.Property(e => e.phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(12);

                entity.Property(e => e.receiveUpdates).HasColumnName("receiveUpdates");

                entity.Property(e => e.referredby).HasColumnName("referredby");

                entity.Property(e => e.referredbyOther)
                    .HasColumnName("referredbyOther")
                    .HasMaxLength(50);

                entity.Property(e => e.returnCustomer).HasColumnName("returnCustomer");

                entity.Property(e => e.state)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(2);

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.zipcode)
                    .IsRequired()
                    .HasColumnName("zipcode")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasIndex(e => e.PersonID)
                    .HasName("IX_PersonID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.dateFrom)
                    .HasColumnName("dateFrom")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateTo)
                    .HasColumnName("dateTo")
                    .HasColumnType("datetime");

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.eventTypeID).HasColumnName("eventType");

                entity.Property(e => e.eventTypeEN)
                    .HasColumnName("eventTypeEN")
                    .HasMaxLength(50);

                entity.Property(e => e.eventTypeES)
                    .HasColumnName("eventTypeES")
                    .HasMaxLength(50);

                entity.Property(e => e.notes)
                    .HasColumnName("notes")
                    .HasMaxLength(4000);

                entity.Property(e => e.PersonID).HasColumnName("PersonID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.PersonID)
                    .HasConstraintName("FK_dbo.Events_dbo.Persons_PersonID");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.filename)
                    .HasColumnName("filename")
                    .HasMaxLength(255);

                entity.Property(e => e.ImageMimeType).HasMaxLength(30);

                entity.Property(e => e.parenttable)
                    .HasColumnName("parenttable")
                    .HasMaxLength(30);

                entity.Property(e => e.recordkey)
                    .HasColumnName("recordkey")
                    .HasMaxLength(20);

                entity.Property(e => e.ThumbnailMimeType).HasMaxLength(30);

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });

            modelBuilder.Entity<JoinEventImage>(entity =>
            {
                entity.HasIndex(e => e.EventID)
                    .HasName("IX_EventID");

                entity.HasIndex(e => e.ImageID)
                    .HasName("IX_ImageID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.ImageID).HasColumnName("ImageID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.JoinEventImages)
                    .HasForeignKey(d => d.EventID)
                    .HasConstraintName("FK_dbo.JoinEventImages_dbo.Events_EventID");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.JoinEventImages)
                    .HasForeignKey(d => d.ImageID)
                    .HasConstraintName("FK_dbo.JoinEventImages_dbo.Images_ImageID");
            });

            modelBuilder.Entity<Lookup>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.category)
                    .IsRequired()
                    .HasColumnName("category")
                    .HasMaxLength(20);

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.emailTemplate).HasColumnName("emailTemplate");

                entity.Property(e => e.fixedJob).HasColumnName("fixedJob");

                entity.Property(e => e.key)
                    .HasColumnName("key")
                    .HasMaxLength(30);

                entity.Property(e => e.level).HasColumnName("level");

                entity.Property(e => e.ltrCode)
                    .HasColumnName("ltrCode")
                    .HasMaxLength(3);

                entity.Property(e => e.minHour).HasColumnName("minHour");

                entity.Property(e => e.minimumCost).HasColumnName("minimumCost");

                entity.Property(e => e.selected).HasColumnName("selected");

                entity.Property(e => e.skillDescriptionEn)
                    .HasColumnName("skillDescriptionEn")
                    .HasMaxLength(300);

                entity.Property(e => e.skillDescriptionEs)
                    .HasColumnName("skillDescriptionEs")
                    .HasMaxLength(300);

                entity.Property(e => e.sortorder).HasColumnName("sortorder");

                entity.Property(e => e.speciality).HasColumnName("speciality");

                entity.Property(e => e.subcategory)
                    .HasColumnName("subcategory")
                    .HasMaxLength(20);

                entity.Property(e => e.text_EN)
                    .IsRequired()
                    .HasColumnName("text_EN")
                    .HasMaxLength(50);

                entity.Property(e => e.text_ES)
                    .IsRequired()
                    .HasColumnName("text_ES")
                    .HasMaxLength(50);

                entity.Property(e => e.typeOfWorkID).HasColumnName("typeOfWorkID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.wage).HasColumnName("wage");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.address1)
                    .HasColumnName("address1")
                    .HasMaxLength(50);

                entity.Property(e => e.address2)
                    .HasColumnName("address2")
                    .HasMaxLength(50);

                entity.Property(e => e.cellphone)
                    .HasColumnName("cellphone")
                    .HasMaxLength(12);

                entity.Property(e => e.city)
                    .HasColumnName("city")
                    .HasMaxLength(25);

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.facebook)
                    .HasColumnName("facebook")
                    .HasMaxLength(50);

                entity.Property(e => e.firstname1)
                    .IsRequired()
                    .HasColumnName("firstname1")
                    .HasMaxLength(50);

                entity.Property(e => e.firstname2)
                    .HasColumnName("firstname2")
                    .HasMaxLength(50);

                entity.Property(e => e.fullName).HasColumnName("fullName");

                entity.Property(e => e.gender).HasColumnName("gender");

                entity.Property(e => e.genderother)
                    .HasColumnName("genderother")
                    .HasMaxLength(20);

                entity.Property(e => e.lastname1)
                    .IsRequired()
                    .HasColumnName("lastname1")
                    .HasMaxLength(50);

                entity.Property(e => e.lastname2)
                    .HasColumnName("lastname2")
                    .HasMaxLength(50);

                entity.Property(e => e.nickname)
                    .HasColumnName("nickname")
                    .HasMaxLength(50);

                entity.Property(e => e.phone)
                    .HasColumnName("phone")
                    .HasMaxLength(12);

                entity.Property(e => e.state)
                    .HasColumnName("state")
                    .HasMaxLength(2);

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ReportDefinition>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.category).HasColumnName("category");

                entity.Property(e => e.columnsJson).HasColumnName("columnsJson");

                entity.Property(e => e.commonName).HasColumnName("commonName");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.description).HasColumnName("description");

                entity.Property(e => e.inputsJson).HasColumnName("inputsJson");

                entity.Property(e => e.name).HasColumnName("name");

                entity.Property(e => e.sqlquery).HasColumnName("sqlquery");

                entity.Property(e => e.subcategory).HasColumnName("subcategory");

                entity.Property(e => e.title).HasColumnName("title");

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });

            modelBuilder.Entity<ScheduleRule>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.day).HasColumnName("day");

                entity.Property(e => e.leadHours).HasColumnName("leadHours");

                entity.Property(e => e.maxEndMin).HasColumnName("maxEndMin");

                entity.Property(e => e.minStartMin).HasColumnName("minStartMin");

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });
            
            modelBuilder.Entity<TransportCostRule>(entity =>
            {
                entity.HasIndex(e => e.transportRuleID)
                    .HasName("IX_transportRuleID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.cost).HasColumnName("cost");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.maxWorker).HasColumnName("maxWorker");

                entity.Property(e => e.minWorker).HasColumnName("minWorker");

                entity.Property(e => e.transportRuleID).HasColumnName("transportRuleID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.HasOne(d => d.transportRule)
                    .WithMany(p => p.costRules)
                    .HasForeignKey(d => d.transportRuleID)
                    .HasConstraintName("FK_dbo.TransportCostRules_dbo.TransportRules_transportRuleID");
            });

            modelBuilder.Entity<TransportProviderAvailability>(entity =>
            {
                entity.HasIndex(e => e.transportProviderID)
                    .HasName("IX_transportProviderID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.available).HasColumnName("available");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.day).HasColumnName("day");

                entity.Property(e => e.key)
                    .HasColumnName("key")
                    .HasMaxLength(50);

                entity.Property(e => e.lookupKey)
                    .HasColumnName("lookupKey")
                    .HasMaxLength(50);

                entity.Property(e => e.transportProviderID).HasColumnName("transportProviderID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.HasOne(d => d.TransportProvider)
                    .WithMany(p => p.AvailabilityRules)
                    .HasForeignKey(d => d.transportProviderID)
                    .HasConstraintName("FK_dbo.TransportProviderAvailabilities_dbo.TransportProviders_transportProviderID");
            });

            modelBuilder.Entity<TransportProvider>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.defaultAttribute).HasColumnName("defaultAttribute");

                entity.Property(e => e.key)
                    .HasColumnName("key")
                    .HasMaxLength(50);

                entity.Property(e => e.sortorder).HasColumnName("sortorder");

                entity.Property(e => e.text_EN)
                    .HasColumnName("text_EN")
                    .HasMaxLength(50);

                entity.Property(e => e.text_ES)
                    .HasColumnName("text_ES")
                    .HasMaxLength(50);

                entity.Property(e => e.updatedby).HasMaxLength(30);
            });

            modelBuilder.Entity<TransportRule>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.key)
                    .HasColumnName("key")
                    .HasMaxLength(50);

                entity.Property(e => e.lookupKey)
                    .HasColumnName("lookupKey")
                    .HasMaxLength(50);

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.zipcodes)
                    .HasColumnName("zipcodes")
                    .HasMaxLength(1000);

                entity.Property(e => e.zoneLabel)
                    .HasColumnName("zoneLabel")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<WorkAssignment>(entity =>
            {
                entity.HasIndex(e => e.workOrderID)
                    .HasName("IX_workOrderID");

                entity.HasIndex(e => e.workerAssignedID)
                    .HasName("IX_workerAssignedID");

                entity.HasIndex(e => e.workerSigninID)
                    .HasName("IX_workerSigninID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.attitude).HasColumnName("attitude");

                entity.Property(e => e.comments).HasColumnName("comments");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.days).HasColumnName("days");

                entity.Property(e => e.description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.englishLevelID).HasColumnName("englishLevelID");

                entity.Property(e => e.followDirections).HasColumnName("followDirections");

                entity.Property(e => e.fullWAID).HasColumnName("fullWAID");

                entity.Property(e => e.hourRange).HasColumnName("hourRange");

                entity.Property(e => e.hourlyWage).HasColumnName("hourlyWage");

                entity.Property(e => e.hours).HasColumnName("hours");

                entity.Property(e => e.maxEarnings).HasColumnName("maxEarnings");

                entity.Property(e => e.minEarnings).HasColumnName("minEarnings");

                entity.Property(e => e.pseudoID).HasColumnName("pseudoID");

                entity.Property(e => e.qualityOfWork).HasColumnName("qualityOfWork");

                entity.Property(e => e.reliability).HasColumnName("reliability");

                entity.Property(e => e.skillEN).HasColumnName("skillEN");

                entity.Property(e => e.skillES).HasColumnName("skillES");

                entity.Property(e => e.skillID).HasColumnName("skillID");

                entity.Property(e => e.surcharge).HasColumnName("surcharge");

                entity.Property(e => e.transportCost).HasColumnName("transportCost");

                entity.Property(e => e.transportProgram).HasColumnName("transportProgram");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.weightLifted).HasColumnName("weightLifted");

                entity.Property(e => e.workOrderID).HasColumnName("workOrderID");

                entity.Property(e => e.workerAssignedID).HasColumnName("workerAssignedID");

                entity.Property(e => e.workerRating).HasColumnName("workerRating");

                entity.Property(e => e.workerRatingComments)
                    .HasColumnName("workerRatingComments")
                    .HasMaxLength(500);

                entity.Property(e => e.workerSigninID).HasColumnName("workerSigninID");

                entity.HasOne(d => d.workOrder)
                    .WithMany(p => p.workAssignments)
                    .HasForeignKey(d => d.workOrderID)
                    .HasConstraintName("FK_dbo.WorkAssignments_dbo.WorkOrders_workOrderID");

                entity.HasOne(d => d.workerAssignedDDD)
                    .WithMany(p => p.workAssignments)
                    .HasForeignKey(d => d.workerAssignedID)
                    .HasConstraintName("FK_dbo.WorkAssignments_dbo.Workers_workerAssignedID");

                entity.HasOne(d => d.workerSiginin)
                    .WithMany(p => p.workAssignments)
                    .HasForeignKey(d => d.workerSigninID)
                    .HasConstraintName("FK_dbo.WorkAssignments_dbo.WorkerSignins_workerSigninID");
            });

            modelBuilder.Query<WOWASummary>().ToView("View_WOWASummary");

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasMany(e => e.workerRequestsDDD)
                    .WithOne(e => e.workOrder);

                entity.HasMany(e => e.workAssignments)
                    .WithOne(e => e.workOrder);
                
                entity.HasIndex(e => e.dateTimeofWork)
                    .HasName("dateTimeofWork");

                entity.HasIndex(e => e.EmployerID)
                    .HasName("IX_EmployerID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.additionalNotes)
                    .HasColumnName("additionalNotes")
                    .HasMaxLength(1000);

                entity.Property(e => e.city)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.contactName)
                    .IsRequired()
                    .HasColumnName("contactName")
                    .HasMaxLength(50);

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.dateTimeofWork)
                    .HasColumnName("dateTimeofWork")
                    .HasColumnType("datetime");

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.description)
                    .HasColumnName("description")
                    .HasMaxLength(4000);

                entity.Property(e => e.disclosureAgreement).HasColumnName("disclosureAgreement");

                entity.Property(e => e.EmployerID).HasColumnName("EmployerID");

                entity.Property(e => e.englishRequired).HasColumnName("englishRequired");

                entity.Property(e => e.englishRequiredNote)
                    .HasColumnName("englishRequiredNote")
                    .HasMaxLength(100);

                entity.Property(e => e.lunchSupplied).HasColumnName("lunchSupplied");

                entity.Property(e => e.onlineSource).HasColumnName("onlineSource");

                entity.Property(e => e.paperOrderNum).HasColumnName("paperOrderNum");

                entity.Property(e => e.permanentPlacement).HasColumnName("permanentPlacement");

                entity.Property(e => e.phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(12);

                entity.Property(e => e.ppFee).HasColumnName("ppFee");

                entity.Property(e => e.ppPayerID)
                    .HasColumnName("ppPayerID")
                    .HasMaxLength(25);

                entity.Property(e => e.ppPaymentID)
                    .HasColumnName("ppPaymentID")
                    .HasMaxLength(50);

                entity.Property(e => e.ppPaymentToken)
                    .HasColumnName("ppPaymentToken")
                    .HasMaxLength(25);

                entity.Property(e => e.ppResponse).HasColumnName("ppResponse");

                entity.Property(e => e.ppState)
                    .HasColumnName("ppState")
                    .HasMaxLength(20);

                entity.Property(e => e.state)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(2);

                entity.Property(e => e.statusID).HasColumnName("status");

                entity.Property(e => e.statusEN)
                    .HasColumnName("statusEN")
                    .HasMaxLength(50);

                entity.Property(e => e.statusES)
                    .HasColumnName("statusES")
                    .HasMaxLength(50);

                entity.Property(e => e.timeFlexible).HasColumnName("timeFlexible");

                entity.Property(e => e.timeZoneOffset).HasColumnName("timeZoneOffset");

                entity.Property(e => e.transportFee).HasColumnName("transportFee");

                entity.Property(e => e.transportFeeExtra).HasColumnName("transportFeeExtra");

                entity.Property(e => e.transportMethodEN).HasColumnName("transportMethodEN");

                entity.Property(e => e.transportMethodES).HasColumnName("transportMethodES");

                entity.Property(e => e.transportMethodID).HasColumnName("transportMethodID");

                entity.Property(e => e.transportProviderID).HasColumnName("transportProviderID");

                entity.Property(e => e.transportTransactID)
                    .HasColumnName("transportTransactID")
                    .HasMaxLength(50);

                entity.Property(e => e.transportTransactType).HasColumnName("transportTransactType");

                entity.Property(e => e.typeOfWorkID).HasColumnName("typeOfWorkID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.waPseudoIDCounter).HasColumnName("waPseudoIDCounter");

                entity.Property(e => e.workSiteAddress1)
                    .IsRequired()
                    .HasColumnName("workSiteAddress1")
                    .HasMaxLength(50);

                entity.Property(e => e.workSiteAddress2)
                    .HasColumnName("workSiteAddress2")
                    .HasMaxLength(50);

                entity.Property(e => e.zipcode)
                    .IsRequired()
                    .HasColumnName("zipcode")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.WorkOrders)
                    .HasForeignKey(d => d.EmployerID)
                    .HasConstraintName("FK_dbo.WorkOrders_dbo.Employers_EmployerID");
            });

            modelBuilder.Entity<WorkerRequest>(entity =>
            {
                entity.HasIndex(e => e.WorkOrderID)
                    .HasName("IX_WorkOrderID");

                entity.HasIndex(e => e.WorkerID)
                    .HasName("IX_WorkerID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.WorkOrderID).HasColumnName("WorkOrderID");

                entity.Property(e => e.WorkerID).HasColumnName("WorkerID");

                entity.HasOne(d => d.workOrder)
                    .WithMany(p => p.workerRequestsDDD)
                    .HasForeignKey(d => d.WorkOrderID)
                    .HasConstraintName("FK_dbo.WorkerRequests_dbo.WorkOrders_WorkOrderID");

                entity.HasOne(d => d.workerRequested)
                    .WithMany(p => p.workerRequests)
                    .HasForeignKey(d => d.WorkerID)
                    .HasConstraintName("FK_dbo.WorkerRequests_dbo.Workers_WorkerID");
            });

            modelBuilder.Entity<WorkerSignin>(entity =>
            {
                entity.HasIndex(e => e.WorkerID)
                    .HasName("IX_WorkerID");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateforsignin)
                    .HasColumnName("dateforsignin")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dwccardnum).HasColumnName("dwccardnum");

                entity.Property(e => e.lottery_sequence).HasColumnName("lottery_sequence");

                entity.Property(e => e.lottery_timestamp)
                    .HasColumnName("lottery_timestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.memberStatusID).HasColumnName("memberStatus");

                entity.Property(e => e.timeZoneOffset).HasColumnName("timeZoneOffset");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.WorkAssignmentID).HasColumnName("WorkAssignmentID");

                entity.Property(e => e.WorkerID).HasColumnName("WorkerID");

                entity.HasOne(d => d.worker)
                    .WithMany(p => p.workersignins)
                    .HasForeignKey(d => d.WorkerID)
                    .HasConstraintName("FK_dbo.WorkerSignins_dbo.Workers_WorkerID");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasIndex(e => e.ID)
                    .HasName("IX_ID");

                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.active).HasColumnName("active");

                entity.Property(e => e.americanBornChildren).HasColumnName("americanBornChildren");

                entity.Property(e => e.carinsurance).HasColumnName("carinsurance");

                entity.Property(e => e.countryoforiginID).HasColumnName("countryoforiginID");

                entity.Property(e => e.createdby).HasMaxLength(30);

                entity.Property(e => e.dateOfBirth)
                    .HasColumnName("dateOfBirth")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateOfMembership)
                    .HasColumnName("dateOfMembership")
                    .HasColumnType("datetime");

                entity.Property(e => e.datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateinUSA)
                    .HasColumnName("dateinUSA")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateinseattle)
                    .HasColumnName("dateinseattle")
                    .HasColumnType("datetime");

                entity.Property(e => e.dateupdated)
                    .HasColumnName("dateupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.disabilitydesc)
                    .HasColumnName("disabilitydesc")
                    .HasMaxLength(50);

                entity.Property(e => e.disabled).HasColumnName("disabled");

                entity.Property(e => e.driverslicense).HasColumnName("driverslicense");

                entity.Property(e => e.dwccardnum).HasColumnName("dwccardnum");

                entity.Property(e => e.educationLevel).HasColumnName("educationLevel");

                entity.Property(e => e.emcontUSAname)
                    .HasColumnName("emcontUSAname")
                    .HasMaxLength(50);

                entity.Property(e => e.emcontUSAphone)
                    .HasColumnName("emcontUSAphone")
                    .HasMaxLength(14);

                entity.Property(e => e.emcontUSArelation)
                    .HasColumnName("emcontUSArelation")
                    .HasMaxLength(30);

                entity.Property(e => e.emcontoriginname)
                    .HasColumnName("emcontoriginname")
                    .HasMaxLength(50);

                entity.Property(e => e.emcontoriginphone)
                    .HasColumnName("emcontoriginphone")
                    .HasMaxLength(14);

                entity.Property(e => e.emcontoriginrelation)
                    .HasColumnName("emcontoriginrelation")
                    .HasMaxLength(30);

                entity.Property(e => e.englishlevelID).HasColumnName("englishlevelID");

                entity.Property(e => e.farmLaborCharacteristics).HasColumnName("farmLaborCharacteristics");

                entity.Property(e => e.fullNameAndID)
                    .HasColumnName("fullNameAndID")
                    .HasMaxLength(100);

                entity.Property(e => e.healthInsurance).HasColumnName("healthInsurance");

                entity.Property(e => e.healthInsuranceDate)
                    .HasColumnName("healthInsuranceDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.height)
                    .HasColumnName("height")
                    .HasMaxLength(50);

                entity.Property(e => e.homeless).HasColumnName("homeless");

                entity.Property(e => e.housingType).HasColumnName("housingType");

                entity.Property(e => e.ImageID).HasColumnName("ImageID");

                entity.Property(e => e.immigrantrefugee).HasColumnName("immigrantrefugee");

                entity.Property(e => e.incomeID).HasColumnName("incomeID");

                entity.Property(e => e.incomeSourceID).HasColumnName("incomeSourceID");

                entity.Property(e => e.insuranceexpiration)
                    .HasColumnName("insuranceexpiration")
                    .HasColumnType("datetime");

                entity.Property(e => e.introToCenter)
                    .HasColumnName("introToCenter")
                    .HasMaxLength(1000);

                entity.Property(e => e.lastPaymentAmount).HasColumnName("lastPaymentAmount");

                entity.Property(e => e.lastPaymentDate)
                    .HasColumnName("lastPaymentDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.lgbtq).HasColumnName("lgbtq");

                entity.Property(e => e.licenseexpirationdate)
                    .HasColumnName("licenseexpirationdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.liveWithDescription)
                    .HasColumnName("liveWithDescription")
                    .HasMaxLength(1000);

                entity.Property(e => e.liveWithSpouse).HasColumnName("liveWithSpouse");

                entity.Property(e => e.livealone).HasColumnName("livealone");

                entity.Property(e => e.livewithchildren).HasColumnName("livewithchildren");

                entity.Property(e => e.maritalstatus).HasColumnName("maritalstatus");

                entity.Property(e => e.memberReactivateDate)
                    .HasColumnName("memberReactivateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.memberStatusID).HasColumnName("memberStatus");

                entity.Property(e => e.memberStatusEN)
                    .HasColumnName("memberStatusEN")
                    .HasMaxLength(50);

                entity.Property(e => e.memberStatusES)
                    .HasColumnName("memberStatusES")
                    .HasMaxLength(50);

                entity.Property(e => e.memberexpirationdate)
                    .HasColumnName("memberexpirationdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.neighborhoodID).HasColumnName("neighborhoodID");

                entity.Property(e => e.numChildrenUnder18).HasColumnName("numChildrenUnder18");

                entity.Property(e => e.numofchildren).HasColumnName("numofchildren");

                entity.Property(e => e.ownTools).HasColumnName("ownTools");

                entity.Property(e => e.RaceID).HasColumnName("RaceID");

                entity.Property(e => e.raceother)
                    .HasColumnName("raceother")
                    .HasMaxLength(20);

                entity.Property(e => e.recentarrival).HasColumnName("recentarrival");

                entity.Property(e => e.skill1).HasColumnName("skill1");

                entity.Property(e => e.skill2).HasColumnName("skill2");

                entity.Property(e => e.skill3).HasColumnName("skill3");

                entity.Property(e => e.skillCodes).HasColumnName("skillCodes");

                entity.Property(e => e.typeOfWork).HasColumnName("typeOfWork");

                entity.Property(e => e.typeOfWorkID).HasColumnName("typeOfWorkID");

                entity.Property(e => e.updatedby).HasMaxLength(30);

                entity.Property(e => e.usVeteran).HasColumnName("usVeteran");

                entity.Property(e => e.vehicleTypeID).HasColumnName("vehicleTypeID");

                entity.Property(e => e.wageTheftRecoveryAmount).HasColumnName("wageTheftRecoveryAmount");

                entity.Property(e => e.wageTheftVictim).HasColumnName("wageTheftVictim");

                entity.Property(e => e.weight)
                    .HasColumnName("weight")
                    .HasMaxLength(10);

                entity.Property(e => e.workerRating).HasColumnName("workerRating");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Worker)
                    .HasForeignKey<Worker>(d => d.ID)
                    .HasConstraintName("FK_dbo.Workers_dbo.Persons_ID");
            });
            
            // VIEWS //
            modelBuilder.Query<QueryMetadata>();
        }
    }
}
