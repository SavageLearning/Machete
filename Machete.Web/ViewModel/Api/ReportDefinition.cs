namespace Machete.Web.ViewModel.Api
{
    public class ReportDefinitionVM : RecordVM
    {
        public object columns { get; set; }
        public object inputs { get; set; }
        public string name          { get; set; } // used in URLs, needs to be url-friendly, no spaces
        public string commonName    { get; set; } // used for dropdowns and titles
        public string title         { get; set; } // if null use commonName
        public string description   { get; set; }
        public string sqlquery      { get; set; }
        public string category      { get; set; }
        public string subcategory   { get; set; }
        public string inputsJson { get; set; } // Which search inputs to display for this report
        public string columnsJson { get; set; } // must match order in sqlquery
        
    }
}