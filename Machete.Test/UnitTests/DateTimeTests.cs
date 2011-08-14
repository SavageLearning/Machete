using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.UnitTests
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            DateTime result;
            var test1 = DateTime.TryParse("5/11/11 9:00", out result);
            Assert.IsNotNull(result, "null");
        }
    }
}
