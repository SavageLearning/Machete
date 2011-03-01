using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Machete.Domain.Resources;

namespace Machete.Domain
{
    public class Employer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Zipcode {get; set; }
        public string Email { get; set; }
        public string Referredby { get; set; }
        public Guid Createdby { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }
}