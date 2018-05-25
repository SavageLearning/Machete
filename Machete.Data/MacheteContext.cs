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
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;

namespace Machete.Data

{
    [DbConfigurationType(typeof(AzureConfiguration))]
    // http://stackoverflow.com/questions/22105583/why-is-asp-net-identity-identitydbcontext-a-black-box
    public class MacheteContext : IdentityDbContext<MacheteUser>, IDisposable
    {
        public MacheteContext() : base("macheteConnection", throwIfV1Schema: false) { }
        public MacheteContext(string connectionString) : base(connectionString, throwIfV1Schema: false) { }

        //Machete here defines the database to use, by convention.
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
        public DbSet<Event> Events {get; set;}
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivitySignin> ActivitySignins { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ReportDefinition> ReportDefinitions { get; set; }
        public DbSet<TransportProvider> TransportProviders { get; set; }
        public DbSet<TransportProviderAvailability> TransportProvidersAvailability { get; set; }
        public DbSet<TransportRule> TransportRules { get; set; }
        public DbSet<TransportCostRule> TransportCostRules { get; set; }
        public DbSet<ScheduleRule> ScheduleRules { get; set; }
        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }

            catch (DbEntityValidationException dbEx)
            {
                var details = new StringBuilder();
                var preface = String.Format("DbEntityValidation Error: ");
                Trace.TraceInformation(preface);
                details.AppendLine(preface);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var tempstr = String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        details.AppendLine(tempstr);
                        Trace.TraceInformation(tempstr);
                    }
                }
                
                throw new Exception(details.ToString());
            }
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //This calls the other builders (below)
            modelBuilder.Configurations.Add(new PersonBuilder());
            modelBuilder.Configurations.Add(new WorkerBuilder());
            modelBuilder.Configurations.Add(new WorkerSigninBuilder());
            modelBuilder.Configurations.Add(new EventBuilder());
            modelBuilder.Configurations.Add(new JoinEventImageBuilder());
            //modelBuilder.Configurations.Add(new JoinWorkorderEmailBuilder());
            modelBuilder.Configurations.Add(new ActivitySigninBuilder());
            modelBuilder.Configurations.Add(new ActivityBuilder());
            modelBuilder.Configurations.Add(new EmailBuilder());
            modelBuilder.Configurations.Add(new TransportProviderBuilder());
            modelBuilder.Configurations.Add(new TransportProvidersAvailabilityBuilder());
            modelBuilder.Configurations.Add(new TransportRuleBuilder());
            modelBuilder.Configurations.Add(new TransportCostRuleBuilder());
            modelBuilder.Configurations.Add(new ScheduleRuleBuilder());
            modelBuilder.Configurations.Add(new EmployerBuilder());
            modelBuilder.Configurations.Add(new WorkOrderBuilder());
            modelBuilder.Configurations.Add(new WorkAssignmentBuilder());

            modelBuilder.Entity<Employer>().ToTable("Employers");
            modelBuilder.Entity<WorkOrder>().ToTable("WorkOrders");
            modelBuilder.Entity<WorkAssignment>().ToTable("WorkAssignments");
            modelBuilder.Configurations.Add(new ReportDefinitionBuilder());
        }
    }

    public class ConfigBuilder : EntityTypeConfiguration<Config>
    {
        public ConfigBuilder()
        {
            ToTable("Configs");
            HasKey(k => k.ID);
        }
    }

    public class PersonBuilder : EntityTypeConfiguration<Person>
    {
        public PersonBuilder()
        {
            ToTable("Persons");
            HasKey(k => k.ID);
            HasOptional(p => p.Worker).WithRequired(p => p.Person).WillCascadeOnDelete();
        }
    }

    public class WorkerBuilder : EntityTypeConfiguration<Worker>
    {
        public WorkerBuilder() 
        {
            HasKey(k => k.ID);
            HasMany(s => s.workersignins)
                .WithOptional(s => s.worker)
                .HasForeignKey(s => s.WorkerID);
            HasMany(a => a.workAssignments)
                .WithOptional(a => a.workerAssigned)
                .HasForeignKey(a => a.workerAssignedID);
        }
    }

    public class ReportDefinitionBuilder : EntityTypeConfiguration<ReportDefinition>
    {
        public ReportDefinitionBuilder()
        {
            HasKey(k => k.ID);
        }
    }

    public class WorkerSigninBuilder : EntityTypeConfiguration<WorkerSignin>
    {
        public WorkerSigninBuilder()
        {
            HasKey(k => k.ID)
            .Property(x => x.ID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("WorkerSignins");
            });
        }
    }

    public class EmployerBuilder : EntityTypeConfiguration<Employer>
    {
        public EmployerBuilder()
        {
            HasKey(e => e.ID);                  //being explicit for EF
            HasMany(e => e.WorkOrders)          //define the parent
            .WithRequired(w => w.Employer)      //Virtual property definition
            .HasForeignKey(w => w.EmployerID)   //DB foreign key definition
            .WillCascadeOnDelete();
        }
    }

    public class EmailBuilder : EntityTypeConfiguration<Email>
    {
        public EmailBuilder()
        {
            HasKey(e => e.ID);
        }
    }

    public class WorkOrderBuilder : EntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(p => p.Employer)
            .WithMany(e => e.WorkOrders)
            .HasForeignKey(e => e.EmployerID);
        }
    }

    public class WorkAssignmentBuilder : EntityTypeConfiguration<WorkAssignment>
    {
        public WorkAssignmentBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(k => k.workOrder)
                .WithMany(a => a.workAssignments)
                .HasForeignKey(a => a.workOrderID);

        }
    }

    public class EventBuilder : EntityTypeConfiguration<Event>
    {
        public EventBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(k => k.Person)
                .WithMany(e => e.Events)
                .HasForeignKey(k => k.PersonID);
        }
    }

    public class JoinEventImageBuilder : EntityTypeConfiguration<JoinEventImage>
    {
        public JoinEventImageBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(k => k.Event)
                .WithMany(d => d.JoinEventImages)
                .HasForeignKey(k => k.EventID);
            HasRequired(k => k.Image);
        }
    }

    public class ActivityBuilder : EntityTypeConfiguration<Activity>
    {
        public ActivityBuilder()
        {
            HasKey(k => k.ID);
            HasMany(e => e.Signins)
                .WithRequired(w => w.Activity)
                .HasForeignKey(k => k.activityID)
                .WillCascadeOnDelete();
        }
    }

    public class ActivitySigninBuilder : EntityTypeConfiguration<ActivitySignin>
    {
        public ActivitySigninBuilder()
        {
            HasKey(k => k.ID)
            .Property(x => x.ID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Map(m => {
                m.MapInheritedProperties();
                m.ToTable("ActivitySignins");
            });
        }
    }

    public class TransportProviderBuilder : EntityTypeConfiguration<TransportProvider>
    {
        public TransportProviderBuilder()
        {
            HasKey(k => k.ID);
            HasMany(e => e.AvailabilityRules)          //define the parent
            .WithRequired(w => w.Provider)      //Virtual property definition
            .HasForeignKey(w => w.transportProviderID)   //DB foreign key definition
            .WillCascadeOnDelete();
        }
    }

    public class TransportProvidersAvailabilityBuilder : EntityTypeConfiguration<TransportProviderAvailability>
    {
        public TransportProvidersAvailabilityBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(p => p.Provider)
            .WithMany(e => e.AvailabilityRules)
            .HasForeignKey(e => e.transportProviderID);
        }
    }

    public class TransportRuleBuilder : EntityTypeConfiguration<TransportRule>
    {
        public TransportRuleBuilder()
        {
            HasKey(k => k.ID);
            HasMany(c => c.costRules)
                .WithRequired(r => r.transportRule)
                .HasForeignKey(k => k.transportRuleID)
                .WillCascadeOnDelete();
        }
    }
    public class TransportCostRuleBuilder : EntityTypeConfiguration<TransportCostRule>
    {
        public TransportCostRuleBuilder()
        {
            HasKey(k => k.ID);
            HasRequired(k => k.transportRule)
                .WithMany(c => c.costRules)
                .HasForeignKey(k => k.transportRuleID);
        }
    }

    public class ScheduleRuleBuilder : EntityTypeConfiguration<ScheduleRule>
    {
        public ScheduleRuleBuilder()
        {
            HasKey(k => k.ID);            
        }
    }

}
