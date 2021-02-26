using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Machete.Domain;
using Machete.Service;
using Machete.Test.Integration.Fluent;
using Machete.Web.ViewModel.Api;

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
            frb = FluentRecordBaseFactory.Get();
        }

        [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        public void AutoMapper_OnlineOrder()
        {
            //Arrange
            var wo = frb.CloneOnlineOrder();
            var map = frb.ToApiMapper();
            
            //Act
            var result = map.Map<WorkOrderVM, WorkOrder>(wo);

            //Assert
            Assert.IsNotNull(result, "DTO.WorkOrderList is Null");
            Assert.IsTrue(result.GetType() == typeof(WorkOrder));
        }

        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        // TODO: Understand how this is supposed to work, because EF Core will not just ignore the ID assigned by code
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
            wo.workAssignments = new List<WorkAssignment>();
            var wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5; 
            wa.ID = 1; // this causes EF Core to fail
            wo.workAssignments.Add(wa);
            var serv = frb.ToServ<IOnlineOrdersService>();

            // 
            // Act
            var result = serv.Create(wo, "CreateOnlineOrder_Succeeds");
            //
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.workAssignments);
            Assert.IsTrue(result.workAssignments.Count == 1);
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


        [Ignore, TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
        // TODO: See note at test above. We can't explicitly assign values to the identity column. Why?
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
            wa.transportCost = 15;
            wa.ID = 1; // EF Core failure
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 5;
            wa.ID = 2; // EF Core failure
            wo.workAssignments.Add(wa);
            wa = frb.CloneDomainWorkAssignment();
            wa.transportCost = 0;
            wa.ID = 3; // EF Core failure
            wo.workAssignments.Add(wa);

            var serv = frb.ToServ<IOnlineOrdersService>();

            // Act
            var result = serv.Create(wo, "verify_tiered_pricing");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.workAssignments);
            Assert.IsTrue(result.workAssignments.Count == 3);
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
