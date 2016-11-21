using Machete.Data.Logging;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;

namespace Machete.Data
{
    class AzureConfiguration : DbConfiguration
    {
        public AzureConfiguration()
        {
            //SetExecutionStrategy(
            //    "System.Data.SqlClient", 
            //    () => new SqlAzureExecutionStrategy());
            //DbInterception.Add(new MacheteInterceptorLogging());
        }
    }
}
