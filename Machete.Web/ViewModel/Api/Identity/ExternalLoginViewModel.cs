namespace Machete.Web.ViewModel.Api.Identity
{
    /// <summary>
    /// <para>A class for handling data sent to the API by an external OAuth2 provider.
    /// Exposes the following properties:</para>
    /// <para>string Code</para>
    /// <para>string State</para>
    /// <para>string Scope</para>
    /// </summary>
    public class ExternalLoginViewModel
    {
        public string Code { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
    }
}
