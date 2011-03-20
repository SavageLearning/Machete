using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
namespace Machete.Data
{
    public class MacheteContext : DbContext
    {
        public MacheteContext() : base("macheteConnection") { }
          //Machete here defines the database to use, by convention.
        public DbSet<Person> Persons { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<TypeOfWork> TypesOfWork { get; set; }
        public DbSet<WorkerSignin> WorkerSignins { get; set; }
        //public DbSet<Employer> Employers { get; set; }
        //public DbSet<WorkOrder> WorkOrders { get; set; }
        //public DbSet<WorkerSkill> WorkerSkills { get; set; }
        //public DbSet<WorkAssignment> WorkAssignments { get; set; }
        //public DbSet<Survey> Surveys { get; set; }
        

        public virtual void Commit()
        {
            //TODO: Catch and handle database exceptions
            
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //This calls the other builders (below)
            modelBuilder.Configurations.Add(new PersonBuilder());
            modelBuilder.Configurations.Add(new WorkerBuilder());
            modelBuilder.Configurations.Add(new WorkerSigninBuilder());
            //modelBuilder.Configurations.Add(new WorkerSigninViewBuilder());
            //modelBuilder.Entity<Race>().ToTable("LookupRace");
            //modelBuilder.Entity<EnglishLevel>().ToTable("LookupEnglishlevel");
            //modelBuilder.Entity<Income>().ToTable("LookupIncome");
            //modelBuilder.Entity<SkillLevel>().ToTable("LookupSkillLevel");
            //modelBuilder.Entity<Neighborhood>().ToTable("LookupNeighborhood");
            //modelBuilder.Entity<WorkerSignin>().ToTable("WorkerSignin");
            //modelBuilder.Entity<Employer>().ToTable("Employers");
            //modelBuilder.Entity<WorkOrder>().ToTable("WorkOrders");
            //modelBuilder.Entity<WorkerSkill>().ToTable("WorkerSkills");
            //modelBuilder.Entity<WorkAssignment>().ToTable("WorkAssignments");
            //modelBuilder.Entity<Survey>().ToTable("Surveys");
        }
    }
    public class PersonBuilder : EntityTypeConfiguration<Person>
    {
        public PersonBuilder()
        {
            ToTable("Persons");
            HasKey(k => k.ID);
            HasOptional(p => p.Worker).WithRequired().WillCascadeOnDelete();
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
        }
    }
    public class WorkerSigninBuilder : EntityTypeConfiguration<WorkerSignin>
    {
        public WorkerSigninBuilder()
        {
            HasKey(k => k.ID);
        }
    }
}
