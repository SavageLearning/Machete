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
        public MacheteContext() : base("Machete") { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
