using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Machete.Api.ViewModel
{
    public class EventVM : RecordVM
    {
        public int PersonID { get; set; }
        public virtual PersonVM Person { get; set; }
        public virtual ICollection<JoinEventImageVM> JoinEventImages { get; set; }
        //
        //
        [Required]
        public int eventTypeID { get; set; }
        [StringLength(50)]
        public string eventTypeEN { get; set; }
        [StringLength(50)]
        public string eventTypeES { get; set; }
        //
        [Required]
        public DateTime dateFrom { get; set; }
        //
        public DateTime? dateTo { get; set; }
        //
        [StringLength(4000)]
        public string notes { get; set; }
    }

    public class EventListVM : ListVM
    {
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string fileCount { get; set; }
        public string type { get; set; }
        public string notes { get; set; }
    }

    public class JoinEventImageVM : RecordVM
    {
        public int EventID { get; set; }
        public virtual EventVM Event { get; set; }

        public int ImageID { get; set; }
        public virtual ImageVM Image { get; set; }
    }

    public class ImageVM : RecordVM
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

    public class ImageListVM : ListVM
    {

    }
}