using Machete.Service.Tenancy;
using Microsoft.AspNetCore.Mvc;

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

        public static T ExtractFromDataObject<T>(object obj) where T : class
        {
            var data = obj
                .GetType()
                .GetProperty("data")
                .GetValue(obj, null) as T;
            return data;
        }

        public static T ExtractFromUntypedProp<T>(object obj, string propname)
        {
            var data = obj
                .GetType()
                .GetProperty(propname)
                .GetValue(obj, null);
            return (T)data;
        }  

        public static bool HasDataProperty(ObjectResult result) =>
            result?.Value.GetType().GetProperty("data") != null;
        
    }
}
