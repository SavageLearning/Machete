using System.Configuration;
using System.IO;
using FluentAssertions;
using Machete.Data.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Data.Tenancy
{
    [TestClass]
    public class TenantServiceTests
    {
        public ITenantService _tenantService;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<ITenantIdentificationService> _tenantIdentificationService;
        private IConfiguration _configuration;
        private string _expectedTenant = "default";
        private Tenant _tenant;

        [TestMethod]
        public void GetCurrentTenant_ReturnsExpectedTenant()
        {
            GivenATenantService()
                .WithAConfiguration()
                .WithAnHttpContext()
                .WithATenantIdentificationService();

            WhenTenantServiceIsInstantiated()
                .AndGetCurrentTenantIsInvoked();

            ThenTenantMatchesExpected();
        }

        private TenantServiceTests WithAConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();
                
            return this;
        }

        private TenantServiceTests ThenTenantMatchesExpected()
        {
            _tenant.Name.Should().Match(_expectedTenant);
            return this;
        }

        private TenantServiceTests AndGetCurrentTenantIsInvoked()
        {
            _tenant = _tenantService.GetCurrentTenant();
            return this;
        }

        private TenantServiceTests WhenTenantServiceIsInstantiated()
        {
            _tenantService = new TenantService(_httpContextAccessor.Object, _tenantIdentificationService.Object, _configuration);
            return this;
        }

        private TenantServiceTests WithATenantIdentificationService()
        {
            _tenantIdentificationService = new Mock<ITenantIdentificationService>();
            _tenantIdentificationService
                .Setup(service => service.GetCurrentTenant(_httpContextAccessor.Object.HttpContext))
                .Returns(_expectedTenant);
            return this;
        }

        private TenantServiceTests WithAnHttpContext()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor
                .Setup(accessor => accessor.HttpContext)
                .Returns(new DefaultHttpContext());
            return this;
        }

        private TenantServiceTests GivenATenantService()
        {
            // could do some fancy delegate work here and instantiate the type later, but... time...
            return this;
        }
    }
}
