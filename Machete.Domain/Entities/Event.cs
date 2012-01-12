using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Event : Record
    {
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        //
        //
        [Required(ErrorMessageResourceName = "eventTyperequired", ErrorMessageResourceType = typeof(Resources.Event))]
        [LocalizedDisplayName("eventType", NameResourceType = typeof(Resources.Event))]
        public int eventType { get; set; }
        //
        [LocalizedDisplayName("dateFrom", NameResourceType = typeof(Resources.Event))]
        [Required(ErrorMessageResourceName = "dateFromrequired", ErrorMessageResourceType = typeof(Resources.Event))]
        public DateTime dateFrom { get; set; }
        //
        [LocalizedDisplayName("dateTo", NameResourceType = typeof(Resources.Event))]        
        public DateTime dateTo { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Event))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Event))]
        public string notes { get; set; }

    }
}
