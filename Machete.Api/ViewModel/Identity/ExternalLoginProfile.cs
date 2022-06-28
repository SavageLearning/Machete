namespace Machete.Api.ViewModel.Identity
{
    public class ExternalLoginProfile
    {
        // {
        //   "email": "someuser\u0040gmail.com",
        //   "name": "Some User",
        //   "id": "84735241015705850"
        // }
        public string email { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
}
