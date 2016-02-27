using Machete.Data.Infrastructure;
using Machete.Service;
using Machete.Web;
using Machete.Web.Helpers;
using Machete.Web.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Machete.Test.Integration.System
{
    [TestClass]
    public class LookupCacheTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void Test()
        {
            //var app = new MvcApplication();
            //IUnityContainer container = app.GetUnityContainer();
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //Lookups.Initialize(new LookupCache(), container.Resolve<IDatabaseFactory>());
        }



    }
}
