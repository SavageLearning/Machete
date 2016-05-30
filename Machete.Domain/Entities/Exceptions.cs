#region COPYRIGHT
// File:     Exceptions.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
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
using System;

namespace Machete.Domain.Entities
{
    [Serializable]
    public class MacheteException : Exception
    {
        public string ErrorMessage
        {
            get
            {
                return base.Message.ToString();
            }
        }

        public MacheteException(string errorMessage)
            : base(errorMessage) { }

        public MacheteException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public static class RootException
    {
        //
        // GET: /GetRootException/
        public static string Get(Exception e, string prefix)
        {
            Exception ee = e;
            string messages = prefix + " Exception: \"" + e.Message + "\"";
            while (ee.InnerException != null)
            {
                messages = messages + "\r\nInner exception: \"" + ee.Message + "\"";
                ee = ee.InnerException;
            }
            messages = messages + "\r\nInnermost exception: \"" + ee.Message + "\"";
            return messages;
        }
    }
}
