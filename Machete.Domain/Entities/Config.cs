using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class Config : Record
    {
        public Config()
        {
            idString = "config";
        }

        [StringLength(50)]
        [Required]
        public string key { get; set; }
        [StringLength(5000)]
        [Required]
        public string value { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public Boolean publicConfig { get; set; } 
    }
}
