using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Machete.Data;
using Machete.Web.Helpers;
using Machete.Domain;

namespace Machete.Test.Helpers
{
    [TestClass]
    public class LookupsTests
    {
        Mock<MacheteContext> _db;
        [TestMethod]
        public void HelperLookups_getDefaultID_valid_returns_int()
        {
            //arrange
            //string category = "foo";
            int fakeint = 42;
            _db.Setup(d => d.Lookups.Single(s => s.selected == true && 
                                                 s.category == It.IsAny<string>()
                                           ).ID)
                        .Returns(fakeint);
            //act

            //assert

        }
        [TestMethod]
        public void LookupsHelper_get_valid_returns_selectlist()
        {
            //arrange
            //string category = "foo";
            //string locale = "en";
            List<Lookup> fakelist = new List<Lookup>();
            _db.Setup(d => d.Lookups.ToList().Where(s => s.category == It.IsAny<string>()))
                .Returns(fakelist);
            //Machete.Web.Helpers.Lookups.Initialize();
            //act
            //var result = Lookups.get(category, locale);
            //assert
        }
    }
}
