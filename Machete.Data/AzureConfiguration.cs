using Machete.Data.Logging;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;

namespace Machete.Data
{
    class AzureConfiguration : DbConfiguration
    {
        public AzureConfiguration()
        {
            SetExecutionStrategy(
                "System.Data.SqlClient", 
                () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
            DbInterception.Add(new MacheteInterceptorLogging());
        }
    }
}
