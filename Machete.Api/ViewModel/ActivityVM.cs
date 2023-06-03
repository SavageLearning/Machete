using Machete.Api.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Api.ViewModel
{
    public class ActivityVM : RecordVM
    {
        [Required]
        public int nameID { get; set; }
        // This is the simplest hack i can come up with to keep search on these two Lookups (previously implement in the app)
        // but push it to the database.
        [StringLength(50)]
        public string nameEN { get; set; }
        [StringLength(50)]
        public string nameES { get; set; }
        //
        [Required]
        public int typeID { get; set; }
        //
        [StringLength(50)]
        public string typeEN { get; set; }
        //
        [StringLength(50)]
        public string typeES { get; set; }
        //
        [Required]
        public DateTime dateStart { get; set; }
        //
        [Required]
        public DateTime dateEnd { get; set; }

        public bool recurring { get; set; }
        public int firstID { get; set; }
        //
        [Required]
        public string teacher { get; set; }
        //
        [StringLength(4000)]
        public string notes { get; set; }

        public IList<string> teachers { get; set; }
    }

    public class ActivityListVM : ListVM
    {
        public string name { get; set; }
        public string type { get; set; }
        public string count { get; set; }
        public string teacher { get; set; }
        public string dateStart { get; set; }
        public string dateEnd { get; set; }
    }
}
