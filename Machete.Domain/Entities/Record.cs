using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Record
    {
        public int ID { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        [StringLength(30)]
        public string Createdby { get; set; }
        [StringLength(30)]
        public string Updatedby { get; set; }

        public Record() {}

        public void updatedby(string user)
        {            
            dateupdated = DateTime.Now;  
            Updatedby = user;
        }
        public void createdby(string user)
        {
            datecreated = DateTime.Now;
            Createdby = user;
            updatedby(user);
        }
    }
}
