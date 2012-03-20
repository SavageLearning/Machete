using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Service
{
    [Serializable()]
    public class MissingReferenceException : System.Exception
    {
        public MissingReferenceException() : base() { }
        public MissingReferenceException(string message) : base(message) { }
        public MissingReferenceException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected MissingReferenceException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
