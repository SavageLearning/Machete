namespace Machete.Data.Tenancy
{
    public class Tenant
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ReadOnlyConnectionString { get; set; }
    }
}
