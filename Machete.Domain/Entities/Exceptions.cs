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
