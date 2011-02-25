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

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Worker>().ToTable("WorkerInfo");
        }
    }
}
