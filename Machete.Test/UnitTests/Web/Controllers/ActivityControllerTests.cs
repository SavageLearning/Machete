using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Machete.Web.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.UnitTests.Controllers
{
    public class ActivityControllerTests
    {
        private ActivityController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var activityService = new Mock<IActivityService>().Object;
            var defaults = new Mock<IDefaults>().Object;
            var mapperConfig = new MapperConfiguration(config => { config.ConfigureMvc(); });
            var mapper = mapperConfig.CreateMapper();
            var tenantService = new Mock<ITenantService>().Object;
            
            _controller = new ActivityController(activityService, defaults, mapper, tenantService);
        }        
    }
}
