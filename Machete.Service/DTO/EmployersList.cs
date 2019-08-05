using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class EmployersList
    {
        public int ID { get; set; }
        public bool active { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string zipcode { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public bool onlineSource { get; set; }
    }
}
