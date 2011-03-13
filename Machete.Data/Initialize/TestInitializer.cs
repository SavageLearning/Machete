using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Database;
using Machete.Domain;

namespace Machete.Data
{
    public class TestInitializer : DropCreateDatabaseAlways<MacheteContext>
    {
        protected override void Seed(MacheteContext macheteDB)
        {
            //Initialize Lookup tables with static data
            MacheteLookup.Initialize(macheteDB);
        }
    }
}
