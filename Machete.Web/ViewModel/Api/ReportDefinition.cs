using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Machete.Web.ViewModel.Api
{
    public class ReportDefinitionVM : RecordVM
    {
        public object columns { get; set; }
        [Required]
        public object inputs { get; set; }
        [Required]
        public string name          { get; set; } // used in URLs, needs to be url-friendly, no spaces
        [Required]
        public string commonName    { get; set; } // used for dropdowns and titles
        [JsonIgnore] // !!todo deprecate. Not in use
        public string title         { get; set; } // if null use commonName
        public string description   { get; set; }
        [MinLength(5)]
        [Required(ErrorMessage = "Query is required.", AllowEmptyStrings = false)]
        public string sqlquery      { get; set; }
        [Required]
        public string category      { get; set; }
        [JsonIgnore] // !!todo deprecate. Not in use
        public string subcategory   { get; set; }
        public string inputsJson { get; set; } // Which search inputs to display for this report
        public string columnsJson { get; set; } // must match order in sqlquery
        
    }
}
