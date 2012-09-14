using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Machete.Web.Helpers;

namespace Machete.Test.UnitTests.Controllers
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void Call_Mapper_autoValidate()
        {
            MacheteMapper.Initialize();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
