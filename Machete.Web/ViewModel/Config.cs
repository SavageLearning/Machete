using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Config : Domain.Lookup
    {
        public IDefaults def { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string recordid { get; set; }
    }

    public class ConfigList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string category { get; set; }
        public string selected { get; set; }
        public string text_EN { get; set; }
        public string text_ES { get; set; }
        public string subcategory { get; set; }
        public string level { get; set; }
        public string ltrCode { get; set; }
        public string dateupdated { get; set; }
        public string updatedby { get; set; }
        public string recordid { get; set; }    
        public bool active {get; set; }
    }
}