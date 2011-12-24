using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;

namespace Machete.Data
{
    public class MacheteInitializer : DropCreateDatabaseIfModelChanges<MacheteContext>
    //public class MacheteInitializer : CreateDatabaseIfNotExists<MacheteContext>
    {
        
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookups
            MacheteLookup.Initialize(DB);
            DB.SaveChanges();
            DB.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX [dateTimeofWork] ON [dbo].[WorkOrders] ([dateTimeofWork] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
        }
    }
    public class TestInitializer : DropCreateDatabaseAlways<MacheteContext>
    {
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookup tables with static data
            MacheteLookup.Initialize(DB);
            DB.SaveChanges();
        }
    }
}
