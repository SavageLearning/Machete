using System.Collections.Generic;

namespace Machete.Web.ViewModel.Api.Identity
{
    public class WellKnownViewModel
    {
        // 2018-01-22; moving from IdentityServer3 to Web API Core + ASP.NET Core Identity
        // GET https://identity.machetessl.org/id/.well-known/openid-configuration
        public string issuer {get;set;}
        public string jwks_uri {get;set;}
        public string authorization_endpoint {get;set;}
        public string token_endpoint {get;set;}
        public string userinfo_endpoint {get;set;}
        public string end_session_endpoint {get;set;}
        public string check_session_iframe {get;set;}
        public string revocation_endpoint {get;set;}
        public string introspection_endpoint {get;set;}
        public bool frontchannel_logout_supported {get;set;}
        public bool frontchannel_logout_session_supported {get;set;}
        public List<string> scopes_supported {get;set;}
        public List<string> claims_supported {get;set;}
        public List<string> response_types_supported {get;set;}
        public List<string> response_modes_supported {get;set;}
        public List<string> grant_types_supported {get;set;}
        public List<string> subject_types_supported {get;set;}
        public List<string> id_token_signing_alg_values_supported {get;set;}
        public List<string> code_challenge_methods_supported {get;set;}
        public List<string> token_endpoint_auth_methods_supported {get;set;}        
    }
}
