namespace Machete.Web.ViewModel.Api.Identity
{
    /// <summary>
    /// <para>A class for storing the JSON response from an OIDC provider using Newtonsoft's deserializer.</para>
    /// <para>string access_token</para>
    /// <para>string bearer</para>
    /// <para>int expires_in</para>
    /// </summary>
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
