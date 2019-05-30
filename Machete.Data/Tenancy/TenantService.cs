using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Machete.Data.Tenancy
{
    public interface ITenantService
    {
        Tenant GetCurrentTenant();
        IEnumerable<Tenant> GetAllTenants();
    }

    public class TenantService : ITenantService
    {
        private readonly HttpContext _httpContext;
        private readonly ITenantIdentificationService _service;
        private IConfiguration _configuration;

        public TenantService(IHttpContextAccessor accessor, ITenantIdentificationService service, IConfiguration configuration)
        {
            _httpContext = accessor.HttpContext;
            _service = service;
            _configuration = configuration;
        }
    
        public Tenant GetCurrentTenant()
        {
            var tenantName = _service.GetCurrentTenant(_httpContext);
            return _configuration.GetTenant(tenantName);
        }

        public IEnumerable<Tenant> GetAllTenants()
        {
            var tenants = new List<Tenant>();
            var mapping = _configuration.GetTenantMapping();
            
            foreach (var tenant in mapping.Tenants) 
                tenants.Add(_configuration.GetTenant(tenant.Value));

            return tenants;
        }
    }
}
