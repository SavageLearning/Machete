namespace Machete.Web.ViewModel.Api.Identity
{
    public class ExternalLoginAccessToken
    {
        // {
        //   "access_token":"{token}",
        //   "token_type":"bearer",
        //   "expires_in":5130398
        // }
        public string access_token { get; set; }
        public string bearer { get; set; }
        public int expires_in { get; set; }
    }
}
