
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Api.ViewModel
{
    public class ActivityScheduleVM : RecordVM
    {
        public int firstID { get; set; }

        [Required]
        public int name { get; set; } // lookup

        [Required]
        public int type { get; set; }

        [Required]
        public DateTime dateStart { get; set; }

        [Required]
        public DateTime dateEnd { get; set; }

        [Required]
        public string teacher { get; set; }

        [StringLength(4000)]
        public string notes { get; set; }
        public bool sunday { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
        [Required]
        public DateTime stopDate { get; set; }
        public IList<string> teachers { get; set; }
    }
}
