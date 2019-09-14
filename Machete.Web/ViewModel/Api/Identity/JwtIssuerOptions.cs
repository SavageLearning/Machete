using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Machete.Web.ViewModel.Api.Identity
{
    // https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login
    // https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/Models/JwtIssuerOptions.cs
    public class JwtIssuerOptions
    {
        // https://tools.ietf.org/html/rfc7519#section-4.1
        
        /// <summary>
        /// 4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 4.1.3.  "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime Expiration => IssuedAt.AddSeconds(ValidFor);

        /// <summary>
        /// 4.1.5.  "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// 4.1.6.  "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// 4.1.7. "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        
        // Machete 1.13 tokens included OpenID values: (see https://www.iana.org/assignments/jwt/jwt.xhtml)
        // so, implemented here:       
        
        /// <summary>
        /// _OpenID Connect Core 1.0, §2. "nonce" Value used to associate a Client session with an ID Token.
        /// see: https://security.stackexchange.com/questions/188166/why-does-openid-connect-oidc-use-a-nonce-claim-instead-of-the-jti-regist
        /// Set by client in the request.
        /// </summary>
        public string Nonce { get; set; } // TODO stricter Guid enforcement

        /// <summary>
        /// _OpenID Connect Core 1.0, §2. "auth_time" Time when the authentication occurred
        /// </summary>
        public DateTime AuthTime => DateTime.Now; // although, in 1.13, they were different? Perhaps time of original?
        
        // not implemented: at_hash, no idea how to generate this, hoping it's built in :p
        /// _OpenID Connect Core 1.0, §2. "at_hash" Access Token Hash Value
        
        /// <summary>
        /// _OpenID Connect Core 1.0, §2. "amr"; Authentication Methods References
        /// </summary>
        public string[] AuthenticationMethodsReference => new[] { "password" };

        /// <summary>
        /// _OpenID Connect Front-Channel Logout 1.0, §3. "sid" Session ID
        /// </summary>
        public Func<Task<string>> SidGenerator => 
            () => Task.FromResult(Guid.NewGuid().ToString());

        // and now, these additional values:

        /// <summary>
        /// "role", an array of strings containing the roles of which the user is a member
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// "idp"; (IdentityServer value: "idsrv")
        /// </summary>
        public string IdentityProvider => "machete";
        
        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public int ValidFor { get; set; } = (int)TimeSpan.FromMinutes(120).TotalSeconds;

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
