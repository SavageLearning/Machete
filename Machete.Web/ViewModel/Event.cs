using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machete.Web.ViewModel
{
    public class Event : Record
    {
        public Event()
        {
            idString = "event";
        }
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<JoinEventImage> JoinEventImages { get; set; }
        //
        //
        [Required(ErrorMessageResourceName = "eventTyperequired", ErrorMessageResourceType = typeof(Resources.Event))]
        [LocalizedDisplayName("eventType", NameResourceType = typeof(Resources.Event))]
        public int eventTypeID { get; set; }
        [StringLength(50)]
        public string eventTypeEN { get; set; }
        [StringLength(50)]
        public string eventTypeES { get; set; }
        //
        [LocalizedDisplayName("dateFrom", NameResourceType = typeof(Resources.Event))]
        [Required(ErrorMessageResourceName = "dateFromrequired", ErrorMessageResourceType = typeof(Resources.Event))]
        public DateTime dateFrom { get; set; }
        //
        [LocalizedDisplayName("dateTo", NameResourceType = typeof(Resources.Event))]
        public DateTime? dateTo { get; set; }
        //
        [LocalizedDisplayName("notes", NameResourceType = typeof(Resources.Event))]
        [StringLength(4000, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(Resources.Event))]
        public string notes { get; set; }

        //public class JoinEventImage : Record
        //{
        //    public int EventID { get; set; }
        //    public virtual Event Event { get; set; }

        //    public int ImageID { get; set; }
        //    public virtual Image Image { get; set; }
        //}
    }

    public class EventList
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string fileCount { get; set; }
        public string type { get; set; }
        public string dateupdated { get; set; }
        public string notes { get; set; }
        public string updatedby { get; set; }
    }

    public class JoinEventImage : Record
    {
        public int EventID { get; set; }
        public virtual Event Event { get; set; }

        public int ImageID { get; set; }
        public virtual Image Image { get; set; }
    }

    public class Image : Record
    {
        public byte[] ImageData { get; set; }
        [StringLength(30)]
        public string ImageMimeType { get; set; }
        [StringLength(255)]
        public string filename { get; set; }
        public byte[] Thumbnail { get; set; }
        [StringLength(30)]
        public string ThumbnailMimeType { get; set; }
        [StringLength(30)]
        public string parenttable { get; set; }
        [StringLength(20)]
        public string recordkey { get; set; }
    }
}