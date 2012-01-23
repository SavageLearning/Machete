using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Diagnostics;
namespace Machete.Data
{
    public class MacheteContext : DbContext
    {
        public MacheteContext() : base("macheteConnection") 
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        
          //Machete here defines the database to use, by convention.
        public DbSet<Person> Persons { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }        
        public DbSet<WorkerSignin> WorkerSignins { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkerRequest> WorkerRequests { get; set; }
        public DbSet<Event> Events {get; set;}

        

        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
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
            modelBuilder.Entity<Employer>().ToTable("Employers");
            modelBuilder.Entity<WorkOrder>().ToTable("WorkOrders");
            modelBuilder.Entity<WorkAssignment>().ToTable("WorkAssignments");
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
    public class WorkerSigninBuilder : EntityTypeConfiguration<WorkerSignin>
    {
        public WorkerSigninBuilder()
        {
            HasKey(k => k.ID);
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
}
