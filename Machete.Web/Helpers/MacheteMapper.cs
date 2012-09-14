using System;
using System.Linq;
using AutoMapper;
using Machete.Service;
using System.Globalization;


namespace Machete.Web.Helpers
{
    public static class MacheteMapper
    {

        public static void Initialize()
        {
            Mapper.CreateMap<jQueryDataTableParam, viewOptions>()
                .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                .ForMember(vo => vo.CI, opt => opt.Ignore())
                .ForMember(vo => vo.status, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("status")) ? (int?)null : Convert.ToInt32(dt.searchColName("status"))))
                .ForMember(vo => vo.EmployerID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("EID")) ? (int?)null : Convert.ToInt32(dt.searchColName("EID"))))
                .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                .ForMember(vo => vo.date, opt => opt.MapFrom(dt => dt.todaysdate == null ? null : (DateTime?)DateTime.Parse(dt.todaysdate)))
                .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength))
                .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 == "asc" ? false : true));

            //Unmapped members were found. Review the types and members below.
            //Add a custom mapping expression, ignore, add a custom resolver, or modify the source/destination type
            //========================================================================
            //jQueryDataTableParam -> viewOptions (Destination member list)
            //Machete.Web.Helpers.jQueryDataTableParam -> Machete.Service.viewOptions (Destination member list)
            //------------------------------------------------------------------------
            //EmployerID
            //CI
            //search
            //date
            //woid
            //orderDescending
            //displayStart
            //displayLength
            //showOrdersPending
        }
    }
}