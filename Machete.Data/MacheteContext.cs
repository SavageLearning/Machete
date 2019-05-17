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
using Machete.Data.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

namespace Machete.Data

{
    // http://stackoverflow.com/questions/22105583/why-is-asp-net-identity-identitydbcontext-a-black-box
    public class MacheteContext : IdentityDbContext<MacheteUser>
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
        public DbSet<Person> Persons { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public virtual DbSet<Lookup> Lookups { get; set; }
        public DbSet<WorkerSignin> WorkerSignins { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkerRequest> WorkerRequests { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivitySignin> ActivitySignins { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ReportDefinition> ReportDefinitions { get; set; }
        public DbSet<TransportProvider> TransportProviders { get; set; }
        public DbSet<TransportProviderAvailability> TransportProvidersAvailability { get; set; }
        public DbSet<TransportRule> TransportRules { get; set; }
        public DbSet<TransportCostRule> TransportCostRules { get; set; }
        public DbSet<ScheduleRule> ScheduleRules { get; set; }

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
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Implement base OnModelCreating functionality before adding our own. Functionally, this does nothing.
            // https://stackoverflow.com/a/39577004/2496266
            base.OnModelCreating(modelBuilder);
            
            // ENTITIES //
            modelBuilder.ApplyConfiguration(new PersonBuilder());
            modelBuilder.ApplyConfiguration(new WorkerBuilder());
            modelBuilder.ApplyConfiguration(new WorkerSigninBuilder());
            modelBuilder.ApplyConfiguration(new EventBuilder());
            modelBuilder.ApplyConfiguration(new JoinEventImageBuilder());
            modelBuilder.ApplyConfiguration(new JoinWorkOrderEmailBuilder());
            modelBuilder.ApplyConfiguration(new ActivitySigninBuilder());
            modelBuilder.ApplyConfiguration(new ActivityBuilder());
            modelBuilder.ApplyConfiguration(new EmailBuilder());
            modelBuilder.ApplyConfiguration(new TransportProviderBuilder());
            modelBuilder.ApplyConfiguration(new TransportProvidersAvailabilityBuilder());
            modelBuilder.ApplyConfiguration(new TransportRuleBuilder());
            modelBuilder.ApplyConfiguration(new TransportCostRuleBuilder());
            modelBuilder.ApplyConfiguration(new ScheduleRuleBuilder());
            modelBuilder.ApplyConfiguration(new EmployerBuilder());
            modelBuilder.ApplyConfiguration(new WorkOrderBuilder());
            modelBuilder.ApplyConfiguration(new WorkAssignmentBuilder());
            modelBuilder.ApplyConfiguration(new ReportDefinitionBuilder());
            
            // VIEWS //
            modelBuilder.Query<QueryMetadata>();
        }
    }

    public class JoinWorkOrderEmailBuilder : IEntityTypeConfiguration<JoinWorkOrderEmail>
    {
        public void Configure(EntityTypeBuilder<JoinWorkOrderEmail> builder)
        {
            builder.HasKey(k => new {k.EmailID, k.WorkOrderID});
        }
    }

    public class PersonBuilder : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(k => k.ID);
            builder.Property(p => p.ID)
                .ValueGeneratedOnAdd();
            
            builder.HasOne(p => p.Worker)
                .WithOne(w => w.Person)
                .HasForeignKey<Worker>(w => w.ID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class WorkerBuilder : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasMany(s => s.workersignins)
                .WithOne(s => s.worker).IsRequired(false)
                .HasForeignKey(s => s.WorkerID);
            builder.HasMany(a => a.workAssignments)
                .WithOne(a => a.workerAssigned).IsRequired(false)
                .HasForeignKey(a => a.workerAssignedID);
        }
    }

    public class ReportDefinitionBuilder : IEntityTypeConfiguration<ReportDefinition>
    {
        public void Configure(EntityTypeBuilder<ReportDefinition> builder)
        {
            builder.HasKey(k => k.ID);
        }
    }

    public class WorkerSigninBuilder : IEntityTypeConfiguration<WorkerSignin>
    {
        public void Configure(EntityTypeBuilder<WorkerSignin> builder)
        {
            builder.HasKey(k => k.ID);
            builder.Property(x => x.ID);
        }
    }

    public class EmployerBuilder : IEntityTypeConfiguration<Employer>
    {
        public void Configure(EntityTypeBuilder<Employer> builder)
        {
            builder.HasKey(e => e.ID);
            builder.HasMany(e => e.WorkOrders)             //define the parent
                .WithOne(w => w.Employer).IsRequired(true) //define the virtual property
                .HasForeignKey(wo => wo.EmployerID)        //define the foreign key relationship
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Employers");
        }
    }

    public class EmailBuilder : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }

    public class WorkOrderBuilder : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(p => p.Employer)
                .WithMany(e => e.WorkOrders)
                .HasForeignKey(e => e.EmployerID)
                .IsRequired(true);
            builder.ToTable("WorkOrders");
        }
    }

    public class WorkAssignmentBuilder : IEntityTypeConfiguration<WorkAssignment>
    {
        public void Configure(EntityTypeBuilder<WorkAssignment> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(k => k.workOrder)
                .WithMany(a => a.workAssignments)
                .HasForeignKey(a => a.workOrderID)
                .IsRequired(true);
            builder.ToTable("WorkAssignments");
        }
    }

    public class EventBuilder : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(k => k.Person)
                .WithMany(e => e.Events)
                .HasForeignKey(k => k.PersonID)
                .IsRequired(true);
        }
    }

    public class JoinEventImageBuilder : IEntityTypeConfiguration<JoinEventImage>
    {
        public void Configure(EntityTypeBuilder<JoinEventImage> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(k => k.Event)
                .WithMany(d => d.JoinEventImages)
                .HasForeignKey(k => k.EventID)
                .IsRequired(true);
            builder.HasOne(k => k.Image).WithOne().IsRequired(true);
        }
    }

    public class ActivityBuilder : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasMany(e => e.Signins)
                .WithOne(w => w.Activity)
                .IsRequired(true)
                .HasForeignKey(k => k.activityID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ActivitySigninBuilder : IEntityTypeConfiguration<ActivitySignin>
    {
        public void Configure(EntityTypeBuilder<ActivitySignin> builder)
        {
            builder.HasKey(k => k.ID);
            builder.Property(x => x.ID);
        }
    }

    public class TransportProviderBuilder : IEntityTypeConfiguration<TransportProvider>
    {
        public void Configure(EntityTypeBuilder<TransportProvider> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasMany(e => e.AvailabilityRules)      // define the parent.
                .WithOne(w => w.Provider).IsRequired(true) // define the EF Core virtual property.
                .HasForeignKey(w => w.transportProviderID) // define the foreign key constraint.
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class TransportProvidersAvailabilityBuilder : IEntityTypeConfiguration<TransportProviderAvailability>
    {
        public void Configure(EntityTypeBuilder<TransportProviderAvailability> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(p => p.Provider)
                .WithMany(e => e.AvailabilityRules)
                .HasForeignKey(e => e.transportProviderID);
        }
    }

    public class TransportRuleBuilder : IEntityTypeConfiguration<TransportRule>
    {
        public void Configure(EntityTypeBuilder<TransportRule> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasMany(c => c.costRules)
                .WithOne(r => r.transportRule).IsRequired(true)
                .HasForeignKey(k => k.transportRuleID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class TransportCostRuleBuilder : IEntityTypeConfiguration<TransportCostRule>
    {
        public void Configure(EntityTypeBuilder<TransportCostRule> builder)
        {
            builder.HasKey(k => k.ID);
            builder.HasOne(k => k.transportRule)
                .WithMany(c => c.costRules)
                .HasForeignKey(k => k.transportRuleID)
                .IsRequired(true);
        }
    }

    public class ScheduleRuleBuilder : IEntityTypeConfiguration<ScheduleRule>
    {
        public void Configure(EntityTypeBuilder<ScheduleRule> builder)
        {
            builder.HasKey(k => k.ID);
        }
    }

    public static class MacheteEntityTypeBuilder
    {
        public static EntityTypeBuilder<T> Configurations<T>(this ModelBuilder model) where T : Record
        {
            return model.Entity<T>();
        }

        public static void Add(this EntityTypeBuilder entity, Type type)
        {
            Activator.CreateInstance(type, entity);
        }
    }
}
