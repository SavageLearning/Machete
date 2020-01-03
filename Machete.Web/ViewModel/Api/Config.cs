using System;
using System.ComponentModel.DataAnnotations;
using Machete.Web.ViewModel.Api;

namespace Machete.Web.Controllers.Api
{
public class ConfigVM : RecordVM {
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

