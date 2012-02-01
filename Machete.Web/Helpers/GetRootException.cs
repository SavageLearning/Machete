using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;

namespace Machete.Web.Helpers
{
    public static class RootException
    {
        ////
        //// GET: /GetRootException/
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
