using System.Collections.Generic;
using FluentAssertions;
using Machete.Data.Tenancy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.UnitTests.Data.Tenancy
{
    [TestClass]
    public class TenantTests
    {
        [TestMethod]
        public void TenantMappingDefinition()
        {
            var name = "blueprint";
            var connectionString = "sillyFakeConnectionString";

            var tenant = new Tenant
            {
                Name = name,
                ConnectionString = connectionString
            };

            tenant.Should().NotBeNull();
            tenant.Should().BeOfType(typeof(Tenant));
            tenant.Name.Should().Be("blueprint");
            tenant.ConnectionString.Should().Be("sillyFakeConnectionString");
        }
    }
}
