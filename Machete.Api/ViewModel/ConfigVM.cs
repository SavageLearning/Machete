using System;
using System.ComponentModel.DataAnnotations;
using Machete.Api.ViewModel;

namespace Machete.Api.ViewModel
{
    public class ConfigVM : RecordVM
    {
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

    public class ConfigListVM : ListVM
    {

    }
}