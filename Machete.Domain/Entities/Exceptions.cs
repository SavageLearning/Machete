using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
