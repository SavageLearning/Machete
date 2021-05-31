
namespace Machete.Service.Tenancy
{
    public class Tenant
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ReadOnlyConnectionString { get; set; }
        public string Timezone { get; set; }
    }
}
