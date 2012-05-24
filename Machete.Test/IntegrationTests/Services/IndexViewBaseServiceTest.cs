using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Machete.Test
{
    [TestClass]
    public class IVBServiceTest : ServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {

            Database.SetInitializer<MacheteContext>(new TestInitializer());
            base.Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        //[TestMethod]
        //public void Integration_WA_Service_GetIndexView_check_search_description()
        //{
        //    //
        //    //Act
        //    dOptions.search = "foostring1";
        //    dOptions.woid = 1;
        //    dOptions.orderDescending = true;
        //    var result = _waServ.GetIndexView(dOptions);
        //    //
        //    //Assert
        //    var tolist = result.query.ToList();
        //    Assert.IsNotNull(tolist, "return value is null");
        //    Assert.IsInstanceOfType(result, typeof(dTableList<WorkAssignment>));
        //    Assert.AreEqual("foostring1", tolist[0].description);
        //    Assert.AreEqual(1, result.filteredCount);
        //    Assert.AreEqual(10, result.totalCount);
        //}

        [TestMethod]
        public void Integration_IVB_activity_getUnassociated()
        {
            //Arrange
            int id = 1;
            
            IQueryable<Activity> q = _aRepo.GetAllQ();
            //Act
            IndexViewBase.getUnassociated(id, ref q, _asRepo);
            //Assert
            Assert.IsTrue(q.Count() == 1, "foo");
        }
    }
}
