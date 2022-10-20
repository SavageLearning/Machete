using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Machete.Domain;
using Machete.Service.CustomValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Machete.Test.UnitTests.Web.Attributes
{

    [TestClass]
    public class UserEditableAttributeTests
    {
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.View), TestCategory(TC.Configs)]
        [DataRow("OrganizationName", true)]
        [DataRow("organizationname", true)]
        [DataRow("forbidden", false)]
        public void IsValidTest(object testCase, bool expectedResult)
        {
            // arrange
            UserEditableConfigAttribute validator = new UserEditableConfigAttribute();
            // act
            var actual = validator.IsValid(testCase);
            // test
            Assert.AreEqual(expectedResult, actual);
        }
    }
}
