using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class ActivityList
    {
        public int ID { get; set; }
        public int nameID { get; set; }
        public string nameEN { get; set; }
        public string nameES { get; set; }
        public int typeID { get; set; }
        public string typeEN { get; set; }
        public string typeES { get; set; }
        public int count { get; set; }
        public string teacher { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public DateTime dateupdated { get; set; }
        public string updatedby { get; set; }
        public bool recurring { get; set; }
    }
}
