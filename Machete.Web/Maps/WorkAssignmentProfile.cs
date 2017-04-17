﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Machete.Web.Maps
{
    public class WorkAssignmentProfile : MacheteProfile
    {
        public WorkAssignmentProfile()
        {
            CreateMap<Domain.WorkAssignment, ViewModel.WorkAssignment>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkAssignment/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                    Resources.WorkAssignments.tabprefix +
                    System.String.Format("{0,5:D5}-{1,2:D2}", d.workOrder.paperOrderNum, d.pseudoID)))
            ;
            CreateMap<Domain.WorkAssignment, Service.DTO.WorkAssignmentList>()
                 .ForMember(v => v.employername, opt => opt.MapFrom(d => d.workOrder.Employer.name))
                 .ForMember(v => v.earnings, opt => opt.MapFrom(d => d.minEarnings))
                 .ForMember(v => v.maxEarnings, opt => opt.MapFrom(d => d.maxEarnings))
                 .ForMember(v => v.paperOrderNum, opt => opt.MapFrom(d => d.workOrder.paperOrderNum))
                 .ForMember(v => v.assignedWorker, opt => opt.MapFrom(d => d.workerAssigned == null ? null : d.workerAssigned.Person.fullName))
                 .ForMember(v => v.requestedList, opt => opt.MapFrom(d => 
                    d.workOrder.workerRequests.Select( a=> a.workerRequested.fullNameAndID)))

            ;
            CreateMap<Service.DTO.WorkAssignmentList, ViewModel.WorkAssignmentList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkAssignment/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                    Resources.WorkAssignments.tabprefix +
                    System.String.Format("{0,5:D5}-{1,2:D2}", d.paperOrderNum, d.pseudoID)))
                .ForMember(v => v.WOID, opt => opt.MapFrom(d => d.workOrderID))
                .ForMember(v => v.WAID, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.WID, opt => opt.MapFrom(d => d.workerAssignedID))
                .ForMember(v => v.pWAID, opt => opt.MapFrom(d => d.fullWAID))
                .ForMember(v => v.englishlevel, opt => opt.MapFrom(d => Convert.ToString(d.englishLevelID)))
                .ForMember(v => v.skill, opt => opt.MapFrom(d => getCI() == "ES" ? d.skillES : d.skillEN))
                .ForMember(v => v.hourlywage, opt => opt.MapFrom(d => System.String.Format("${0:f2}", d.hourlyWage)))
                .ForMember(v => v.hours, opt => opt.MapFrom(d => Convert.ToString(d.hours)))
                .ForMember(v => v.hourRange, opt => opt.MapFrom(d => Convert.ToString(d.hourRange)))
                .ForMember(v => v.days, opt => opt.MapFrom(d => Convert.ToString(d.days)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => Convert.ToString(
                    d.dateTimeofWork.AddHours(
                        Convert.ToDouble(WebConfigurationManager.AppSettings["TimeZoneDifferenceFromPacific"])))))
                .ForMember(v => v.timeofwork, opt => opt.MapFrom(d =>
                    d.dateTimeofWork.AddHours(
                        Convert.ToDouble(WebConfigurationManager.AppSettings["TimeZoneDifferenceFromPacific"])).ToShortTimeString()))
                .ForMember(v => v.earnings, opt => opt.MapFrom(d => System.String.Format("${0:f2}", d.earnings)))
                .ForMember(v => v.asmtStatus, opt => opt.MapFrom(d => AssignmentStatus(d)))
            ;
        }
        public static string AssignmentStatus(Service.DTO.WorkAssignmentList d)
        {
            if (d.workerAssignedID > 0 && d.workerSigninID > 0) // green
                return "completed";
            if (d.workerAssignedID == null && d.WOstatus == Domain.WorkOrder.iCompleted)
                return "incomplete";
            if (d.workerAssignedID > 0 && d.workerSigninID == null && d.WOstatus == Domain.WorkOrder.iCompleted)
                return "orphaned";
            if (d.WOstatus == Domain.WorkOrder.iCancelled)
                return "cancelled";
            if (d.WOstatus == Domain.WorkOrder.iActive) // blue
                return "active";
            return null;
        }
        public static string PersonFullName(Domain.Person p)
        {
            var rtnstr = p.firstname1 + " ";
            if (p.firstname2 != null) rtnstr = rtnstr + p.firstname2 + " ";
            rtnstr = rtnstr + p.lastname1;
            if (p.lastname2 != null) rtnstr = rtnstr + " " + p.lastname2;
            return rtnstr;
        }

    }
}