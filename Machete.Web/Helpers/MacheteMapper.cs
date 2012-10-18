using System;
using System.Linq;
using AutoMapper;
using Machete.Service;
using System.Globalization;
using Machete.Web.ViewModel;
using Machete.Domain;


namespace Machete.Web.Helpers
{
    public static class MacheteMapper
    {

        public static void Initialize()
        {
            Mapper.CreateMap<jQueryDataTableParam, viewOptions>()
                .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                .ForMember(vo => vo.CI, opt => opt.Ignore())
                .ForMember(vo => vo.authenticated, opt => opt.Ignore())
                .ForMember(vo => vo.status, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("status")) ? (int?)null : Convert.ToInt32(dt.searchColName("status"))))
                .ForMember(vo => vo.EmployerID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("EID")) ? (int?)null : Convert.ToInt32(dt.searchColName("EID"))))
                .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                .ForMember(vo => vo.date, opt => opt.MapFrom(dt => dt.todaysdate == null ? null : (DateTime?)DateTime.Parse(dt.todaysdate)))
                .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength))
                .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 == "asc" ? false : true));
            // Splitting Combined into parts
            Mapper.CreateMap<EmployerWoConbined, Employer>()
                .ForMember(e => e.WorkOrders, opt => opt.Ignore())
                .ForMember(e => e.active, opt => opt.Ignore())
                .ForMember(e => e.ID, opt => opt.Ignore())
                .ForMember(e => e.datecreated, opt => opt.Ignore())
                .ForMember(e => e.dateupdated, opt => opt.Ignore())
                .ForMember(e => e.Createdby, opt => opt.Ignore())
                .ForMember(e => e.Updatedby, opt => opt.Ignore());
            // Splitting Combined into parts
            Mapper.CreateMap<EmployerWoConbined, WorkOrder>()
                .ForMember(wo => wo.EmployerID, opt => opt.Ignore())
                .ForMember(wo => wo.Employer, opt => opt.Ignore())
                .ForMember(wo => wo.workAssignments, opt => opt.Ignore())
                .ForMember(wo => wo.workerRequests, opt => opt.Ignore())
                .ForMember(wo => wo.paperOrderNum, opt => opt.Ignore())
                .ForMember(wo => wo.waPseudoIDCounter, opt => opt.Ignore())
                //.ForMember(wo => wo.contactName, opt => opt.Ignore())
                .ForMember(wo => wo.status, opt => opt.Ignore())
                .ForMember(wo => wo.permanentPlacement, opt => opt.Ignore())
                .ForMember(wo => wo.transportMethodID, opt => opt.Ignore())
                .ForMember(wo => wo.transportFee, opt => opt.Ignore())
                .ForMember(wo => wo.transportFeeExtra, opt => opt.Ignore())
                .ForMember(wo => wo.ID, opt => opt.Ignore())
                .ForMember(wo => wo.datecreated, opt => opt.Ignore())
                .ForMember(wo => wo.dateupdated, opt => opt.Ignore())
                .ForMember(wo => wo.Createdby, opt => opt.Ignore())
                .ForMember(wo => wo.Updatedby, opt => opt.Ignore());
            // re-combineing to view model object
            Mapper.CreateMap<Employer, EmployerWoConbined>()
                .ForMember(e => e.contactName, opt => opt.Ignore())
                .ForMember(e => e.workSiteAddress1, opt => opt.Ignore())
                .ForMember(e => e.workSiteAddress2, opt => opt.Ignore())
                .ForMember(e => e.wo_city, opt => opt.Ignore())
                .ForMember(e => e.wo_state, opt => opt.Ignore())
                .ForMember(e => e.wo_phone, opt => opt.Ignore())
                .ForMember(e => e.wo_zipcode, opt => opt.Ignore())
                .ForMember(e => e.typeOfWorkID, opt => opt.Ignore())
                .ForMember(e => e.englishRequired, opt => opt.Ignore())
                .ForMember(e => e.englishRequiredNote, opt => opt.Ignore())
                .ForMember(e => e.lunchSupplied, opt => opt.Ignore())
                .ForMember(e => e.description, opt => opt.Ignore())
                .ForMember(e => e.dateTimeofWork, opt => opt.Ignore())
                .ForMember(e => e.timeFlexible, opt => opt.Ignore());
            // re-combineing to view model object
            Mapper.CreateMap<WorkOrder, EmployerWoConbined>()
                .ForMember(wo => wo.business, opt => opt.Ignore())
                .ForMember(wo => wo.name, opt => opt.Ignore())
                .ForMember(wo => wo.address1, opt => opt.Ignore())
                .ForMember(wo => wo.address2, opt => opt.Ignore())
                .ForMember(wo => wo.cellphone, opt => opt.Ignore())
                .ForMember(wo => wo.email, opt => opt.Ignore())
                .ForMember(wo => wo.referredby, opt => opt.Ignore())
                .ForMember(wo => wo.referredbyOther, opt => opt.Ignore())
                .ForMember(wo => wo.blogparticipate, opt => opt.Ignore())
                .ForMember(wo => wo.notes, opt => opt.Ignore())
                .ForMember(wo => wo.wo_city, opt => opt.MapFrom(c => c.city))
                .ForMember(wo => wo.wo_state, opt => opt.MapFrom(c => c.state))
                .ForMember(wo => wo.wo_phone, opt => opt.MapFrom(c => c.phone))
                .ForMember(wo => wo.wo_zipcode, opt => opt.MapFrom(c => c.zipcode));

        }
    }
}