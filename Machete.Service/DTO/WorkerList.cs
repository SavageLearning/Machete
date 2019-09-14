using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class WorkerList
    {
        public int ID { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public int dwccardnum { get; set; }
        public bool? active { get; set; }
        public DateTime memberexpirationdate { get; set; }
        public string memberStatusEN { get; set; }
        public string memberStatusES { get; set; }
        public DateTime dateupdated { get; set; }
        public int memberStatusID { get; set; }

    }
}
