using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Test.Integration;
using Machete.Domain;
using Machete.Service;
using AutoMapper;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class OnlineOrdersServiceTest
    {
        FluentRecordBase frb;

        [ClassInitialize]
        public static void ClassInitialize(TestContext c) { }

        [TestInitialize]
        public void TestInitialize()
        {
            frb = new FluentRecordBase();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void AutoMapper_OnlineOrder()
        {
            //
            //Arrange
            var wo = frb.CloneOnlineOrder();
            var map = frb.ToApiMapper();
            //
            //Act
            var result = map.Map<Api.ViewModel.WorkOrder, Machete.Domain.WorkOrder>(wo);
            //
            //Assert
            Assert.IsNotNull(result, "DTO.WorkOrderList is Null");
            Assert.IsTrue(result.GetType() == typeof(Machete.Domain.WorkOrder));
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void CreateOnlineOrder_Succeeds()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "98118";
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_bus").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<Machete.Domain.WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5; wa.ID = 1;
            wo.workAssignments.Add(wa);
            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "CreateOnlineOrder_Succeeds");
            //
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.workAssignments);
            Assert.IsTrue(result.workAssignments.Count() == 1);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        [ExpectedException(typeof(MacheteValidationException))]
        public void CreateOnlineOrder_WA_empty_throws_error()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "98118";
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_bus").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<WorkAssignment>();
            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "CreateOnlineOrder_WA_empty_throws_error");

        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        [ExpectedException(typeof(MacheteValidationException))]
        public void CreateOnlineOrder_wrong_cost_throws_error()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "98118";
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_bus").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<Machete.Domain.WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 0;
            wo.workAssignments.Add(wa);
            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "CreateOnlineOrder_wrong_cost_throws_error");
            //
            // Assert
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateOnlineOrder_unknown_zipcode_throws_error()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "12345";
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_bus").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<Machete.Domain.WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5;
            wo.workAssignments.Add(wa);
            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "CreateOnlineOrder_unknown_zipcode_throws_error");
            //
            // Assert
        }


        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void CreateOnlineOrder_verify_tiered_pricing()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "98118"; // affects transport cost 
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_van").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<Machete.Domain.WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 15; wa.ID = 1;
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5; wa.ID = 2;
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 0; wa.ID = 3;
            wo.workAssignments.Add(wa);

            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "verify_tiered_pricing");
            //
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.workAssignments);
            Assert.IsTrue(result.workAssignments.Count() == 3);
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        [ExpectedException(typeof(MacheteValidationException))]
        public void CreateOnlineOrder_tiered_IDs_throws_error()
        {
            //
            // Arrange
            var e = frb.ToEmployer();
            var wo = frb.CloneDomainWorkOrder();
            var tpServ = frb.ToServ<ITransportProvidersService>();

            wo.zipcode = "98118"; // affects transport cost 
            wo.EmployerID = e.ID;
            var ll = tpServ.GetMany(a => a.key == "transport_bus").SingleOrDefault();
            wo.transportProviderID = ll.ID;
            wo.workAssignments = new List<Machete.Domain.WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 15; wa.ID = 1;
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5; wa.ID = 2;
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 0; wa.ID = 2;
            wo.workAssignments.Add(wa);

            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "tiered_IDs_throws_error");
            //
            // Assert
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void CreateOnlineOrder_get_workorder()
        {
            // arrange
            var wo = frb.ToWorkOrder();
            var serv = frb.ToServ<IOnlineOrdersService>();
            // act
            var result = serv.Get(wo.ID);
            // 
            Assert.IsNotNull(result);
        }
    }
    
}
