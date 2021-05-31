#region COPYRIGHT
// File:     ImageService.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Service
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

using AutoMapper;
using Machete.Service;
using Machete.Service.Infrastructure;
using Machete.Domain;

namespace Machete.Service
{
    public interface IImageService : IService<Image> {}
    public class ImageService : ServiceBase2<Image>, IImageService
    {
        public ImageService(IDatabaseFactory db, IMapper map) : base(db, map) {}
    }
}
