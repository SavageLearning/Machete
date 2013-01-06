#region COPYRIGHT
// File:     MacheteMapper.cs
// Author:   Savage Learning, LLC.
// Created:  2012/12/29 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
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
                .ForMember(vo => vo.CI, opt => opt.Ignore())
                .ForMember(vo => vo.authenticated, opt => opt.Ignore())
                .ForMember(vo => vo.personID, opt => opt.MapFrom(dt => dt.personID ?? 0))
                .ForMember(vo => vo.onlineSource, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("onlineSource")) ? null : dt.searchColName("onlineSource")))
                .ForMember(vo => vo.status, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("status")) ? null : (int?)Convert.ToInt32(dt.searchColName("status"))))
                .ForMember(vo => vo.EmployerID, opt => opt.MapFrom(dt => string.IsNullOrEmpty(dt.searchColName("EID")) ? null : (int?)Convert.ToInt32(dt.searchColName("EID"))))
                .ForMember(vo => vo.sortColName, opt => opt.MapFrom(dt => dt.sortColName()))
                .ForMember(vo => vo.dwccardnum, opt => opt.MapFrom(dt => Convert.ToInt32(dt.dwccardnum)))
                .ForMember(vo => vo.woid, opt => opt.MapFrom(dt => Convert.ToInt32(dt.searchColName("WOID"))))
                .ForMember(vo => vo.date, opt => opt.MapFrom(dt => dt.todaysdate == null ? null : (DateTime?)DateTime.Parse(dt.todaysdate)))
                .ForMember(vo => vo.displayStart, opt => opt.MapFrom(dt => dt.iDisplayStart))
                .ForMember(vo => vo.displayLength, opt => opt.MapFrom(dt => dt.iDisplayLength))
                .ForMember(vo => vo.orderDescending, opt => opt.MapFrom(dt => dt.sSortDir_0 == "asc" ? false : true));
            #region WoCombined
            // Splitting Combined into parts
            Mapper.CreateMap<EmployerWoCombined, Employer>()
                .IgnoreAllNonExisting();
            // Splitting Combined into parts
            Mapper.CreateMap<EmployerWoCombined, WorkOrder>()
                .IgnoreAllNonExisting();
            // re-combineing to view model object
            Mapper.CreateMap<Employer, EmployerWoCombined>()
                .IgnoreAllNonExisting();
            // re-combineing to view model object
            Mapper.CreateMap<WorkOrder, EmployerWoCombined>()
                .ForMember(wo => wo.wo_city, opt => opt.MapFrom(c => c.city))
                .ForMember(wo => wo.wo_state, opt => opt.MapFrom(c => c.state))
                .ForMember(wo => wo.wo_phone, opt => opt.MapFrom(c => c.phone))
                .ForMember(wo => wo.wo_zipcode, opt => opt.MapFrom(c => c.zipcode))
                .IgnoreAllNonExisting();
            #endregion
        }
        // Thank you stackoverflow, allows IgnoreAllNonExisting!
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(
                    this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps =
                Mapper.GetAllTypeMaps().First(
                    x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}