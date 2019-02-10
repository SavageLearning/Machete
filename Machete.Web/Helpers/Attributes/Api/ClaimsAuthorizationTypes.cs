namespace Machete.Web.Helpers.Api
{
    public struct CAType
    {
        // used in Lookups.key ; max length 30 characters
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string nameidentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    }
}
