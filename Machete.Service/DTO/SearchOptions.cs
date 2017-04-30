using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class SearchOptions
    {
        public DateTime? beginDate { get; set; }
        public DateTime? endDate { get; set; }
        public string idOrName { get; set; }

    }
}
