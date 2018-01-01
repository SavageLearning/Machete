using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class ScheduleRule : Record
    {
        [Required]
        public int day { get; set; }
        public int leadHours { get; set; }
        public int minStartMin { get; set; }
        public int maxEndMin { get; set; }
    }
}
