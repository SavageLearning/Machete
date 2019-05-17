using System.IO;
using FluentAssertions;
using Machete.Data.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.UnitTests.Data.Tenancy
{
    [TestClass]
    public class TenantIdentificationServiceTests
    {
        private IConfiguration _configuration;
        private ITenantIdentificationService _service;
        private HttpContext _httpContext;
        private string _tenant;
        private string _expectedHost = "blueprint.navaja.net";
        private string _expectedTenant = "blueprint";

        public TenantIdentificationServiceTests()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Host = new HostString(_expectedHost);
        }

        [TestMethod]
        public void GetCurrentTenant_ReturnsExpectedTenant()
        {
            GivenATenantIdentificationService()
                .WithAMicrosoftExtensionsConfiguration();

            WhenTenantIdentificationServiceIsInstantiated()
                .AndGetCurrentTenantIsInvoked();

            ThenTenantMatchesExpected();
        }

        private TenantIdentificationServiceTests ThenTenantMatchesExpected()
        {
            _tenant.Should().Match(_expectedTenant);
            return this;
        }

        private TenantIdentificationServiceTests AndGetCurrentTenantIsInvoked()
        {
            _tenant = _service.GetCurrentTenant(_httpContext);
            return this;
        }

        private TenantIdentificationServiceTests WhenTenantIdentificationServiceIsInstantiated()
        {
            _service = new TenantIdentificationService(_configuration);
            return this;
        }

        private TenantIdentificationServiceTests WithAMicrosoftExtensionsConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configurationBuilder.Build();
             
            return this;
        }

        private TenantIdentificationServiceTests GivenATenantIdentificationService()
        {
            return this;
        }
    }
}
