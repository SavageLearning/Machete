#region COPYRIGHT
// File:     Exceptions.cs
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
using Machete.Domain.Entities;
using System;

namespace Machete.Service
{
    [Serializable]
    public class MacheteServiceException : MacheteException
    {
        new public string ErrorMessage { get { return base.Message.ToString(); }}
        public MacheteServiceException(string errorMessage): base(errorMessage) { }
        public MacheteServiceException(string errorMessage, Exception innerEx): base(errorMessage, innerEx) { }
    }
    public class MacheteDispatchException : MacheteServiceException 
    {
        new public string ErrorMessage { get { return base.Message.ToString(); }}
        public MacheteDispatchException(string errorMessage): base(errorMessage) { }
        public MacheteDispatchException(string errorMessage, Exception innerEx): base(errorMessage, innerEx) { }
    }
    public class MacheteIntegrityException : MacheteServiceException
    {
        new public string ErrorMessage { get { return base.Message.ToString(); } }
        public MacheteIntegrityException(string errorMessage) : base(errorMessage) { }
        public MacheteIntegrityException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }

    public class MacheteNullObjectException : MacheteServiceException
    {
        new public string ErrorMessage { get { return base.Message.ToString(); } }
        public MacheteNullObjectException(string errorMessage) : base(errorMessage) { }
        public MacheteNullObjectException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }

    public class MacheteInvalidInputException : MacheteServiceException
    {
        new public string ErrorMessage { get { return base.Message.ToString(); } }
        public MacheteInvalidInputException(string errorMessage) : base(errorMessage) { }
        public MacheteInvalidInputException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }

    public class MacheteValidationException : MacheteServiceException
    {
        new public string ErrorMessage { get { return base.Message.ToString(); } }
        public MacheteValidationException(string errorMessage) : base(errorMessage) { }
        public MacheteValidationException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
}
