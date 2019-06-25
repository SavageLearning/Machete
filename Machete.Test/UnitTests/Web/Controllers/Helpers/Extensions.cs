using Machete.Data.Tenancy;

namespace Machete.Test.UnitTests.Controllers.Helpers
{
    public static class UnitTestExtensions
    {
        public static Tenant TestingTenant => new Tenant
        {
            ConnectionString = "fakeconnectionstring",
            Name = "faketenant",
            ReadOnlyConnectionString = "fakeconnectionstring",
            Timezone = "America/Los_Angeles"
        };
    }
}
