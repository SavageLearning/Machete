#region COPYRIGHT
// File:     Image.cs
// Author:   Savage Learning, LLC.
// Created:  2012/10/27 
// License:  GPL v3
// Project:  Machete.Domain
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
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Image : Record
    {
        public byte[] ImageData { get; set; }
        [StringLength(30)]
        public string ImageMimeType { get; set; }
        [StringLength(255)]
        public string filename { get; set; }
        public byte[] Thumbnail { get; set; }
        [StringLength(30)]
        public string ThumbnailMimeType { get; set; }
        [StringLength(30)]
        public string parenttable { get; set; }
        [StringLength(20)]
        public string recordkey { get; set; }
    }
}
