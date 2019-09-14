using System.Collections.Generic;
using FluentAssertions;
using Machete.Data.Tenancy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.UnitTests.Data.Tenancy
{
    [TestClass]
    public class TenantMappingTests
    {
        [TestMethod]
        public void TenantMappingDefinition()
        {
            var allowDefault = true;
            var @default = "DefaultConnection";
            var tenants = new Dictionary<string, string>
            {
                { "key", "value" }
            };

            var mapping = new TenantMapping
            {
                AllowDefault = allowDefault,
                Default = @default,
                Tenants = tenants
            };

            mapping.Should().NotBeNull();
            mapping.Should().BeOfType(typeof(TenantMapping));
            mapping.AllowDefault.Should().BeTrue();
            mapping.Default.Should().Be("DefaultConnection");
        }
    }
}