using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.ViewModel
{
    public class ScheduleRule : BaseModel
    {
        public int day { get; set; }
        public int leadHours { get; set; }
        public int minStartMin { get; set; }
        public int maxEndMin { get; set; }

    }
}