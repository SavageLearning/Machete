using Microsoft.Extensions.Configuration;

namespace Machete.Service.Tenancy
{
    public static class TenantConfigurationExtensions
    {
        public static TenantMapping GetTenantMapping(this IConfiguration configuration)
        {
            return configuration.GetSection("Tenants").Get<TenantMapping>();
        }

        public static Tenant GetTenant(this IConfiguration configuration, string tenantName)
        {
            return new Tenant
            {
                Name = tenantName,
                ConnectionString = configuration.GetConnectionString(tenantName),
                ReadOnlyConnectionString = configuration.GetConnectionString($"{tenantName}ReadOnly"),
                Timezone = configuration.GetSection("Timezones")[tenantName]
            };
        }
    }
}
