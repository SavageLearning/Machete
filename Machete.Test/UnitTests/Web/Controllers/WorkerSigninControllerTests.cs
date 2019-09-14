using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Test.UnitTests.Controllers.Helpers;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers
{
    public class WorkerSigninControllerTests
    {
        private WorkerSigninController _controller;
        private Tenant _tenant = UnitTestExtensions.TestingTenant;

        [TestInitialize]
        public void TestInitialize()
        {
            var wsServ = new Mock<IWorkerSigninService>();
            var mapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            var map = mapperConfig.CreateMapper();

            var tenantService = new Mock<ITenantService>();
            tenantService.Setup(service => service.GetCurrentTenant()).Returns(_tenant);
            tenantService.Setup(service => service.GetAllTenants()).Returns(new List<Tenant> {_tenant});
            
            _controller = new WorkerSigninController(wsServ.Object, tenantService.Object, map);
        }
    }
}
