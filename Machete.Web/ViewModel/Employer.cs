using Machete.Web.Helpers;

namespace Machete.Web.ViewModel
{

    public class Employer : Service.DTO.Employer
    {
        public string tabref { get; set; }
        public string tablabel { get; set; }
        public IDefaults def { get; set; }

    }
}