using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class LookupList
    {
        public int ID { get; set; }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string category { get; set; }
        public bool selected { get; set; }
        public string text_EN { get; set; }
        public string text_ES { get; set; }
        public string subcategory { get; set; }
        public int? level { get; set; }
        public string ltrCode { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public string recordid { get; set; }
        public bool active { get; set; }
    }
}
