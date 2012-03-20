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
using System.Globalization;
using Machete.Web.Helpers;

namespace Machete.Test
{
    [TestClass]
    public class WorkerSigninServiceTest : ServiceTest
    {
        DispatchOptions _dOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
            _dOptions = new DispatchOptions
            {
                CI = new CultureInfo("en-US", false),
                search = "",
                date = DateTime.Today,
                dwccardnum = null,
                woid = null,
                orderDescending = true,
                sortColName = "WOID",
                displayStart = 0,
                displayLength = 20
            };
        }
        [TestMethod]
        public void DbSet_WorkerSigin_LotterySignin()
        {
            var result = _wsiServ.GetSignin(30040, DateTime.Today);
            Assert.IsNotNull(result);
        }
        /// <summary>
        /// 
        /// </summary>
        //[TestMethod]
        //public void DbSet_WorkerSignin_GetView()
        //{
        //    DateTime date = DateTime.Today;
        //    IEnumerable<WorkerSigninView> filteredWSI = _wsiServ.getView(date);
        //    IEnumerable<WorkerSigninView> result = filteredWSI.ToList();
        //    Assert.IsNotNull(filteredWSI, "WorkerSignin getView return is Null");
        //    Assert.IsNotNull(result, "WorkerSignin getview.ToList() is Null");
        //}
        /// <summary>
        /// Submit an unknown dwccardnum, verify it is recorded and returned by GetIndexView
        /// </summary>
        [TestMethod]
        public void DbSet_WorkerSigninService_Integration_GetIndexView_record_unknown_worker()
        {
            ////            
            //WorkerSignin _signin = new WorkerSignin();
            //int cardnum = 30040;
            //Worker _w = _wServ.GetWorkerByNum(cardnum);
            //_signin.dwccardnum = cardnum;
            //_signin.dateforsignin = DateTime.Today;
            //_wsiServ.CreateWorkerSignin(_signin, "TestUser");            
            //_dOptions.search = _w.dwccardnum.ToString();
            ////            
            //ServiceIndexView<WorkerSigninView> result = _wsiServ.GetIndexView(_dOptions);
            //List<WorkerSigninView> tolist = result.query.ToList();
            ////
            //Assert.AreEqual(1, result.filteredCount);
        }
        /// <summary>
        /// Filters WSI IndexView based on dwccardnum option. should return all records.
        /// </summary>
        [TestMethod]
        public void DbSet_WorkerSigninService_Intergation_GetIndexView_check_search_dwccardnum()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            ServiceIndexView<WorkerSigninView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<WorkerSigninView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkerSigninView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(3, result.filteredCount);
            Assert.AreEqual(5, result.totalCount);
        }
        /// <summary>
        /// Filter on requested grouping
        /// </summary>
        [TestMethod]
        public void DbSet_WorkerSigninService_Intergation_GetIndexView_workerRequested()
        {
            //
            //Act
            _dOptions.dwccardnum = 30040;
            _dOptions.wa_grouping = "requested";
            ServiceIndexView<WorkerSigninView> result = _wsiServ.GetIndexView(_dOptions);
            //
            //Assert
            List<WorkerSigninView> tolist = result.query.ToList();
            Assert.IsNotNull(tolist, "return value is null");
            Assert.IsInstanceOfType(result, typeof(ServiceIndexView<WorkerSigninView>));
            //Assert.AreEqual(61, tolist[0].skillID);
            Assert.AreEqual(1, result.filteredCount);
            Assert.AreEqual(5, result.totalCount);
        }
        //[TestMethod]
        //public void DbSet_TestMethod5()
        //{

        //    IEnumerable<WorkerSignin> testing = _wsiServ.GetSigninsForAssignment(DateTime.Today,
        //                                                "Jose",
        //                                                "asc",
        //                                                null,
        //                                                null);
        //    Assert.IsNotNull(testing, "null");
        //}
    }
}