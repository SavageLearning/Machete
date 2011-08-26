using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Domain.Entities;

namespace Machete.Service
{
    [Serializable]
    public class MacheteServiceException : MacheteException
    {
        public string ErrorMessage { get { return base.Message.ToString(); }}
        public MacheteServiceException(string errorMessage): base(errorMessage) { }
        public MacheteServiceException(string errorMessage, Exception innerEx): base(errorMessage, innerEx) { }
    }
    public class MacheteDispatchException : MacheteServiceException 
    {
        public string ErrorMessage { get { return base.Message.ToString(); }}
        public MacheteDispatchException(string errorMessage): base(errorMessage) { }
        public MacheteDispatchException(string errorMessage, Exception innerEx): base(errorMessage, innerEx) { }
    }
    public class MacheteIntegrityException : MacheteServiceException
    {
        public string ErrorMessage { get { return base.Message.ToString(); } }
        public MacheteIntegrityException(string errorMessage) : base(errorMessage) { }
        public MacheteIntegrityException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
}
