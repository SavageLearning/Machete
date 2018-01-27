﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class Record : ICloneable
    {
        public string createdby { get; set; }
        public string datecreated { get; set; }
        public string dateupdated { get; set; }
        public int id { get; set; }
        public string updatedby { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}