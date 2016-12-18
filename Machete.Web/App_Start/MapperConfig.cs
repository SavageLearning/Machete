using AutoMapper;
using Machete.Domain;
using Machete.Service;
using DTO = Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.Resources;
using Machete.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Machete.Web
{
    public class MapperConfig
    {
        MapperConfiguration cfg;
        Mapper map;
        public MapperConfig()
        {
            cfg = new MapperConfiguration(c => {
                c.CreateMap<jQueryDataTableParam, viewOptions>()
                    .ForMember(vo => vo.CI, opt => opt.Ignore())
                    .ForMember(vo => vo.authenticated, opt => opt.Ignore())
                    .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                    .ForMember(vo => vo.emailID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("emailID")) ? null : (int?)Convert.ToInt32(dt.searchColName("emailID"))))
                    .ForMember(vo => vo.onlineSource, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("onlineSource")) ? null : dt.searchColName("onlineSource")))
                    .ForMember(vo => vo.status, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("status")) ? null : (int?)Convert.ToInt32(dt.searchColName("status"))))
                    .ForMember(vo => vo.EmployerID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("EID")) ? null : (int?)Convert.ToInt32(dt.searchColName("EID"))))
                    .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                    .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                    .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                    .ForMember(vo => vo.date, opt => opt.MapFrom(dt => dt.todaysdate == null ? null : (DateTime?)DateTime.Parse(dt.todaysdate)))
                    .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                    .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength > 0 ? dt.iDisplayLength : 10))
                    .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 == "asc" ? false : true));

                #region WoCombined
                // Splitting Combined into parts
                c.CreateMap<EmployerWoCombined, Domain.Employer>();
                // Splitting Combined into parts
                c.CreateMap<EmployerWoCombined, Domain.WorkOrder>();
                // re-combineing to view model object
                c.CreateMap<Domain.Employer, EmployerWoCombined>();
                // re-combineing to view model object
                c.CreateMap<Domain.WorkOrder, EmployerWoCombined>()
                    .ForMember(wo => wo.wo_city, opt => opt.MapFrom(e => e.city))
                    .ForMember(wo => wo.wo_state, opt => opt.MapFrom(e => e.state))
                    .ForMember(wo => wo.wo_phone, opt => opt.MapFrom(e => e.phone))
                    .ForMember(wo => wo.wo_zipcode, opt => opt.MapFrom(e => e.zipcode));
                #endregion
                c.CreateMap<Email, EmailView>()
                    .ForMember(ev => ev.statusID, opt => opt.MapFrom(e => e.statusID));
                c.CreateMap<EmailView, Email>()
                    .ForMember(e => e.updatedby, opt => opt.Ignore())
                    .ForMember(e => e.createdby, opt => opt.Ignore())
                    .ForMember(e => e.datecreated, opt => opt.Ignore())
                    .ForMember(e => e.dateupdated, opt => opt.Ignore());
                //
                // Employer
                c.CreateMap<Domain.Employer, ViewModel.Employer>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                    .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                    //.ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                    //.ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                    .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                    .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
                c.CreateMap<Domain.Employer, DTO.EmployerList>();
                c.CreateMap<DTO.EmployerList, ViewModel.EmployerList>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Employer/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.name))
                    .ForMember(v => v.active, opt => opt.MapFrom(d => Convert.ToString(d.active)))
                    .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                    .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                    .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                    .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource.ToString()));
                //
                // WorkOrder
                c.CreateMap<Domain.WorkOrder, ViewModel.WorkOrder>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                        Machete.Web.Resources.WorkOrders.tabprefix +
                        Convert.ToString(d.paperOrderNum) +
                        " @ " + d.workSiteAddress1))
                    .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                    .ForMember(v => v.WOID, opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum)))
                    .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                    .ForMember(v => v.dateupdatedstring, opt => opt.MapFrom(d => System.String.Format("{0:MM/dd/yyyy HH:mm:ss}", d.dateupdated)))
                    .ForMember(v => v.onlineSource, opt => opt.MapFrom(d => d.onlineSource ? Shared.True : Shared.False))
                    .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)));
                c.CreateMap<Domain.WorkOrder, DTO.WorkOrderList>()
                    .ForMember(v => v.WAcount, opt => opt.MapFrom(d => d.workAssignments.Count()))
                    .ForMember(v => v.emailSentCount, opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Email.iSent || e.statusID == Email.iReadyToSend).Count()))
                    .ForMember(v => v.emailErrorCount, opt => opt.MapFrom(d => d.Emails.Where(e => e.statusID == Email.iTransmitError).Count()))
                ;
                c.CreateMap<DTO.WorkOrderList, ViewModel.WorkOrderList>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkOrder/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d => 
                        Machete.Web.Resources.WorkOrders.tabprefix +
                        Convert.ToString(d.paperOrderNum) +
                        " @ " + d.workSiteAddress1))
                    .ForMember(v => v.EID, opt => opt.MapFrom(d => Convert.ToString(d.EmployerID)))
                    .ForMember(v => v.WOID, opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum)))
                    .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => Convert.ToString(d.dateTimeofWork)))
                    .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                    ;
                //
                // WorkAssignment
                c.CreateMap<Domain.WorkAssignment, ViewModel.WorkAssignment>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkAssignment/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d => 
                        Resources.WorkAssignments.tabprefix + 
                        System.String.Format("{0,5:D5}", d.workOrder == null ? 0 : d.workOrder.paperOrderNum) +
                        "-" + System.String.Format("{0,2:D2}", d.pseudoID)))
                ;
                c.CreateMap<Domain.WorkAssignment, DTO.WorkAssignmentList>()
                     .ForMember(v => v.employername, opt => opt.MapFrom(d => d.workOrder.Employer.name))
                     .ForMember(v => v.earnings, opt => opt.MapFrom(d => (d.days * d.surcharge) + (d.hourlyWage * d.hours * d.days)))
                     .ForMember(v => v.maxEarnings, opt => opt.MapFrom(d =>
                        d.hourRange == null ? 0 : (d.days * d.surcharge) + (d.hourlyWage * (int)d.hourRange * d.days)))
                    .ForMember(v => v.paperOrderNum, opt => opt.MapFrom(d => d.workOrder.paperOrderNum))
                     //.ForMember(v => v.assignedWorker, opt => opt.MapFrom(d =>
                     //   d.workerAssigned == null ? "" :
                     //   Convert.ToString(d.workerAssigned.dwccardnum))) // + " " + PersonFullName(d.workerAssigned.Person))) //TODO:2016: replace person full name
                ;
                c.CreateMap<DTO.WorkAssignmentList, ViewModel.WorkAssignmentList>()
                    .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/WorkAssignment/Edit/" + Convert.ToString(d.ID)))
                    .ForMember(v => v.tablabel, opt => opt.MapFrom(d =>
                        Resources.WorkAssignments.tabprefix +
                        System.String.Format("{0,5:D5}", d.paperOrderNum) +
                        "-" + System.String.Format("{0,2:D2}", d.pseudoID)))
                    .ForMember(v => v.WOID, opt => opt.MapFrom(d => d.workOrderID))
                    .ForMember(v => v.WAID, opt => opt.MapFrom(d => d.ID))
                    .ForMember(v => v.WID, opt => opt.MapFrom(d => d.workerAssignedID))
                    .ForMember(v => v.pWAID, opt => opt.MapFrom(d => System.String.Format("{0,5:D5}", d.paperOrderNum) +
                        "-" + System.String.Format("{0,2:D2}", d.pseudoID)))
                    .ForMember(v => v.englishlevel, opt => opt.MapFrom(d => Convert.ToString(d.englishLevelID)))
                    .ForMember(v => v.skill, opt => opt.MapFrom(d => Convert.ToString(d.skillID)))
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
                //
                // WorkerSignin
                c.CreateMap<Domain.WorkerSignin, DTO.WorkerSigninList>()
                    .ForMember(v => v.lotterySequence, opt => opt.MapFrom(d => d.lottery_sequence))
                    .ForMember(v => v.englishlevel, opt => opt.MapFrom(d => d == null ? 0 : d.worker.englishlevelID))
                    .ForMember(v => v.waid, opt => opt.MapFrom(d => d.WorkAssignmentID))
                    .ForMember(v => v.skill1, opt => opt.MapFrom(d => d == null ? null : d.worker.skill1))
                    .ForMember(v => v.skill2, opt => opt.MapFrom(d => d == null ? null : d.worker.skill2))
                    .ForMember(v => v.skill3, opt => opt.MapFrom(d => d == null ? null : d.worker.skill3))
                    .ForMember(v => v.fullname, opt => opt.MapFrom(d => 
                        d.worker.Person.firstname1 + " " +
                        d.worker.Person.firstname2 + " " +
                        d.worker.Person.lastname1 + " " +
                        d.worker.Person.lastname2 ))
                ;

            });
        }
        public static string AssignmentStatus(DTO.WorkAssignmentList d)
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


        public IMapper getMapper()
        {
            return map ?? (map = new Mapper(cfg));
        }


    }
}