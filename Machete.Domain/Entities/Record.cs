using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Domain
{
    public class Record
    {
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
        public string Createdby { get; set; }
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
