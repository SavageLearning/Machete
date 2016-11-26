using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class Record : Domain.Record
    {
        public new string dateupdated { get; set; }
        public new string Updatedby { get; set; }
    }
}
