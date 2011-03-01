using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
using System.Data.Entity.ModelConfiguration;
namespace Machete.Data
{
    public class MacheteContext : DbContext
    {
        public MacheteContext() : base("mote") { } //Machete here defines the database to use, by convention.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Race> LRaces { get; set; }
        public DbSet<EnglishLevel> LEnglishLevel { get; set; }
        public DbSet<Income> LIncome { get; set; }
        public DbSet<Neighborhood> LNeighborhood { get; set; }
        public DbSet<SkillLevel> LSkillLevel { get; set; }
        public DbSet<WorkerSignin> WorkerSignins { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkerSkill> WorkerSkills { get; set; }
        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public DbSet<Survey> Surveys { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Worker>().ToTable("WorkerInfo");
            modelBuilder.Entity<Race>().ToTable("LookupRace");
            modelBuilder.Entity<EnglishLevel>().ToTable("LookupEnglishlevel");
            modelBuilder.Entity<Income>().ToTable("LookupIncome");
            modelBuilder.Entity<SkillLevel>().ToTable("LookupSkillLevel");
            modelBuilder.Entity<Neighborhood>().ToTable("LookupNeighborhood");
            modelBuilder.Entity<WorkerSignin>().ToTable("WorkerSignin");
            modelBuilder.Entity<Employer>().ToTable("Employers");
            modelBuilder.Entity<WorkOrder>().ToTable("WorkOrders");
            modelBuilder.Entity<WorkerSkill>().ToTable("WorkerSkills");
            modelBuilder.Entity<WorkAssignment>().ToTable("WorkAssignments");
            modelBuilder.Entity<Survey>().ToTable("Surveys");

        }
    }
}
