using System.ComponentModel.DataAnnotations.Schema;

namespace Machete.Data.Dynamic {
    public class QueryMetadata
    {
        public string name { get; set; }
        public bool? is_nullable { get; set; }
        public string system_type_name { get; set; }
        [NotMapped]
        public bool include { get; set; } // default value for the UI
    }
}