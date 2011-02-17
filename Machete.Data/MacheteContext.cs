using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
namespace Machete.Data
{
    public class MacheteContext : DbContext
    {
        public MacheteContext() : base("Machete2") { } //Machete here defines the database to use, by convention.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Person> Persons { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
