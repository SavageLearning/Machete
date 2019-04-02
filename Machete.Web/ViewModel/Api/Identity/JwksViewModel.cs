using System.Collections.Generic;

namespace Machete.Web.ViewModel.Api.Identity {
    public class JwksViewModel
    {
        public string kty { get; set; }
        public string use { get; set; }
        public string kid { get; set; }
        public string x5t { get; set; }
        public string e { get; set; }
        public string n { get; set; } 
        public List<string> x5c { get; set; }
    }
}
