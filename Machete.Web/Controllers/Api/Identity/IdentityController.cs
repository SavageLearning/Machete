using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Machete.Data;
using Machete.Web.Helpers.Api;
using Machete.Web.Helpers.Api.Identity;
using Machete.Web.ViewModel.Api.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Machete.Web.Controllers.Api.Identity
{
    #region [Summary]
    ///<summary>
    /// 
    /// This controller exists outside the Machete.Api.Controllers because it serves a different purpose and because the
    /// routes are defined separately. Its fields and dependencies were created to replicate the functionality of
    /// IdentityServer3 without having to modify the https://github.com/SavageLearning/machete-ui project beyond port
    /// mappings and domain names. As such, the namespace division exists to call out that this could be (and once was)
    /// a separate project.
    ///
    /// After going down the path of implementing full-on JWT auth using the AngularJS oidc-client library, we decided
    /// to use cookie auth to save time. So currently, we only issue a JWT token for a user that is already authenticated
    /// using cookies. There's a branch with the working JWT implementation here:
    ///
    /// https://github.com/chaim1221/Machete/blob/feature/api-jwt/Api/Identity/Controllers/IdentityController.cs#L82
    ///
    /// Sources used:
    /// https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login
    /// https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/Controllers/AccountsController.cs (MIT)
    /// https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/Controllers/AuthController.cs (MIT)
    /// https://github.com/IdentityServer/IdentityServer3 (Apache-2.0)
    /// https://github.com/IdentityServer/IdentityServer4 (Apache-2.0)
    /// 
    /// </summary>
    #endregion [Summary]
    [Route("id")]
    public class IdentityController : Controller
    {
        private readonly UserManager<MacheteUser> _userManager;
        private readonly SignInManager<MacheteUser> _signinManager;

        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly List<IdentityRole> _roles;
        private readonly IConfiguration _configuration;

        public IdentityController(UserManager<MacheteUser> userManager,
            SignInManager<MacheteUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
            IConfiguration configuration
        )
        {
            ThrowIfInvalidOptions(jwtOptions.Value);

            _userManager = userManager;
            _signinManager = signInManager;

            _roles = roleManager.Roles.ToList();
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _configuration = configuration;
        }
        
        [HttpGet]
        [Route("authorize")]
        public async Task<IActionResult> Authorize()
        {
            if (!User.Identity.IsAuthenticated) return await Task.FromResult(new UnauthorizedResult());
            
            var verifiedUser = await _userManager.FindByNameAsync(User.Identity.Name);
            foreach (var role in _roles)
            {
                var hasRole = await _userManager.IsInRoleAsync(verifiedUser, role.Name);
                if (hasRole) verifiedUser.Roles.Add(role);
            }
            _jwtOptions.Issuer = Routes.GetHostFrom(Request).IdentityRoute();
            _jwtOptions.Nonce = Guid.NewGuid().ToString(); // corners were cut; this is supposed to identify the client
            var claimsIdentity = await _jwtFactory.GenerateClaimsIdentity(verifiedUser, _jwtOptions);
            var jwt = await _jwtFactory.GenerateEncodedToken(claimsIdentity, _jwtOptions);
            
            return await Task.FromResult<IActionResult>(
                new OkObjectResult(new { access_token = jwt })
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel model)
        {
            if (!ValidateLogin(model)) return BadRequest(ModelState);
            
            await VerifyClaimsExistFor(model.UserName);
            
            var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.Remember, false);

            if (result?.Succeeded == true) return await Task.FromResult(new OkResult());

            ModelState.TryAddModelError("login_failure", "Invalid username or password.");
            return BadRequest(ModelState);
        }
        
        // https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow/
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/index?view=aspnetcore-2.2#environment-variables-configuration-provider
        [HttpGet]
        [Route("signin-facebook")]
        public async Task<IActionResult> FacebookLogin([FromQuery] ExternalLoginViewModel viewModel)
        {
            var host = Routes.GetHostFrom(Request);

            if (viewModel.State == _configuration["Authentication:State"])
            {
                var httpClient = new HttpClient();
                var appId = _configuration["Authentication:Facebook:AppId"];
                var redirectUri = Routes.FacebookSignin(host);
                var appSecret = _configuration["Authentication:Facebook:AppSecret"];

                var tokenResponse = await httpClient.GetAsync(
                    $"https://graph.facebook.com/v3.2/oauth/access_token?" +
                    $"client_id={appId}&" +
                    $"redirect_uri={redirectUri}&" +
                    $"client_secret={appSecret}&" +
                    $"code={viewModel.Code}");

                await SigninByEmailAsync(tokenResponse, httpClient, "facebook", redirectUri);
            }
            // They're either logged in now, or they aren't.
            return await Task.FromResult<IActionResult>(new RedirectResult(host.V2AuthorizationEndpoint()));
        }

        // https://developers.google.com/identity/protocols/OAuth2WebServer
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/index?view=aspnetcore-2.2#environment-variables-configuration-provider
        [HttpGet]
        [Route("signin-google")]
        public async Task<IActionResult> GoogleLogin([FromQuery] ExternalLoginViewModel viewModel)
        {
            var host = Routes.GetHostFrom(Request);

            if (viewModel.State == _configuration["Authentication:State"])
            {
                var httpClient = new HttpClient();
                var appId = _configuration["Authentication:Google:ClientId"];
                var redirectUri = $"{host}id/signin-google"; // TODO
                var appSecret = _configuration["Authentication:Google:ClientSecret"];

// TODO refactor; I apologize for the copy-paste code but we are out of time for auth
                var content = new StringContent($@"
{{
  ""code"": ""{viewModel.Code}"",
  ""client_id"": ""{appId}"",
  ""client_secret"": ""{appSecret}"",
  ""redirect_uri"": ""{redirectUri}"",
  ""grant_type"": ""authorization_code""
}}
"
                );

                // TODO we *should* get this URL from https://accounts.google.com/.well-known/openid-configuration
                var tokenResponse = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
                
                await SigninByEmailAsync(tokenResponse, httpClient, "google", redirectUri);
            }
            // They're either logged in now, or they aren't.
            return await Task.FromResult<IActionResult>(new RedirectResult(host.V2AuthorizationEndpoint()));
        }

      //private
        private async Task SigninByEmailAsync(HttpResponseMessage tokenResponse, HttpClient httpClient, string provider, string redirectUri)
        {
            var tokenResponseContent = tokenResponse.Content.ReadAsStringAsync();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenObject = JsonConvert.DeserializeObject<ExternalLoginAccessToken>(tokenResponseContent.Result);

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenObject.access_token);

                string profileResponseUrl;
                switch (provider)
                {
                    case "facebook":
                        profileResponseUrl = $"https://graph.facebook.com/me?" +
                                             $"fields=name,email&" +
                                             $"access_token={tokenObject.access_token}";
                        break;
                    case "google":
                        // TODO we *should* get this URL from https://accounts.google.com/.well-known/openid-configuration
                        profileResponseUrl = "https://openidconnect.googleapis.com/v1/userinfo";
                        break;
                    default:
                        throw new Exception("Unable to parse provider.");
                }
                
                var profileResponse = await httpClient.GetAsync(profileResponseUrl);
                var profileResponseContent = profileResponse.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<ExternalLoginProfile>(profileResponseContent.Result);
                var user = await _userManager.FindByEmailAsync(profile.email);
                if (user == null)
                {
                    var name = profile.name.Split(' ');
                    var macheteUser = new MacheteUser
                    {
                        UserName = profile.email,
                        Email = profile.email, // Machete pls
                        FirstName = name[0],
                        LastName = name[1] // Chaim pls
                    };
                    await _userManager.CreateAsync(macheteUser);
                    user = await _userManager.FindByEmailAsync(profile.email);
                    await _userManager.AddToRoleAsync(user, "Hirer");
                    /* **** me */
                }

                await VerifyClaimsExistFor(user.UserName);
                await _signinManager.SignInAsync(user, true);
            }
            else throw new AuthenticationException(
              $"Results: {tokenResponseContent.Result}\nRedirectUri: {redirectUri}\nRequest Scheme: {Environment.GetEnvironmentVariable("MACHETE_USE_HTTPS_SCHEME")}"
            );
        }

        //https://www.c-sharpcorner.com/article/claim-based-and-policy-based-authorization-with-asp-net-core-2-1/
        private async Task VerifyClaimsExistFor(string username)
        {
            // They are probably using an email, but not necessarily. Either way, it should be "username" in the db.
            var user = await _userManager.FindByNameAsync(username);
            
            var claims = await _userManager.GetClaimsAsync(user);
            var claimsList = claims.Select(claim => claim.Type).ToList();
            
            if (!claimsList.Contains(CAType.nameidentifier))
                await _userManager.AddClaimAsync(user, new Claim(CAType.nameidentifier, user.Id));           
            if (!claimsList.Contains(CAType.email))
                await _userManager.AddClaimAsync(user, new Claim(CAType.email, user.Email));
            // In the above we use the user.Email regardless of UserName. TODO inform them if a discrepancy exists.
        }

        private bool ValidateLogin(CredentialsViewModel creds)
        {
            if (!ModelState.IsValid)
                ModelState.TryAddModelError("invalid_request", "Invalid request.");
            if (string.IsNullOrEmpty(creds.UserName) || string.IsNullOrEmpty(creds.Password))
                ModelState.TryAddModelError("login_failure", "Invalid username or password.");
            return ModelState.ErrorCount == 0;
        }
        
        // GET: /id/logoff
        [AllowAnonymous]
        [HttpGet]
        [Route("logoff")]
        public async Task<IActionResult> LogOff()
        {
            // TODO: This still isn't working with the proxy. Figure out how to manually expire the cookies maybe?
            await _signinManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            
            return await Task.FromResult<IActionResult>(
                new OkObjectResult(new { data = Routes.GetHostFrom(Request).V2AuthorizationEndpoint() })
            );
        }
        
        [HttpGet]
        [Route(".well-known/openid-configuration")]
        public async Task<IActionResult> OpenIdConfiguration()
        {
            var host = Routes.GetHostFrom(Request);

            var viewModel = new WellKnownViewModel();
            viewModel.issuer = host.IdentityRoute();
            viewModel.jwks_uri = host.JsonWebKeySetEndpoint();
            // These are endpoints that should be implemented if we're switching to JWT auth:
//            viewModel.authorization_endpoint = host.AuthorizationEndpoint();
//            viewModel.token_endpoint = host.TokenEndpoint();
//            viewModel.userinfo_endpoint = host.UserInfoEndpoint();
//            viewModel.end_session_endpoint = host.EndSessionEndpoint();
//            viewModel.check_session_iframe = host.CheckSessionEndpoint();
//            viewModel.revocation_endpoint = host.RevocationEndpoint();
//            viewModel.introspection_endpoint = host.IntrospectionEndpoint();
            viewModel.frontchannel_logout_supported = true;
            viewModel.frontchannel_logout_session_supported = true;
            viewModel.scopes_supported =
                new List<string> {"openid", "profile", "email", "roles", "offline_access", "api"};
            viewModel.claims_supported =
                new List<string> {
                    "sub", "name", "family_name", "given_name", "middle_name", "nickname",
                    "preferred_username", "profile", "picture", "website", "gender", "birthdate",
                    "zoneinfo", "locale", "updated_at", "email", "email_verified", "role"
                };
            viewModel.response_types_supported =
                new List<string> {
                    "code", "token", "id_token", "id_token token", "code id_token", "code token",
                    "code id_token token"
                };
            viewModel.response_modes_supported = new List<string> {"form_post", "query", "fragment"};
            viewModel.grant_types_supported =
                new List<string> {"authorization_code", "client_credentials", "password", "refresh_token", "implicit"};
            viewModel.subject_types_supported = new List<string> {"public"};
            viewModel.id_token_signing_alg_values_supported = new List<string> {"RS256"};
            viewModel.code_challenge_methods_supported = new List<string> {"plain", "S256"};
            viewModel.token_endpoint_auth_methods_supported =
                new List<string> {"client_secret_post", "client_secret_basic"};

            return await Task.FromResult(new JsonResult(viewModel));
        }

        // Surprisingly little documentation exists on how to make one of these
        // https://github.com/IdentityServer/IdentityServer4/blob/63a50d7838af25896fbf836ea4e4f37b5e179cd8/src/ResponseHandling/Default/DiscoveryResponseGenerator.cs
        [HttpGet]
        [Route(".well-known/jwks")]
        public async Task<IActionResult> JsonWebKeySet()
        {
            JwksViewModel webKey = new JwksViewModel();
            var key = _jwtOptions.SigningCredentials.Key;

            if (key is RsaSecurityKey rsaKey) {
                var parameters = rsaKey.Rsa?.ExportParameters(false) ?? rsaKey.Parameters;
                var exponent = Base64UrlEncoder.Encode(parameters.Exponent);
                var modulus = Base64UrlEncoder.Encode(parameters.Modulus);

                webKey = new JwksViewModel {
                    kty = "RSA",
                    use = "sig",
                    kid = rsaKey.KeyId,
                    e = exponent,
                    n = modulus,
//                    alg = algorithm
                };
            }

            return await Task.FromResult(new JsonResult(new {
                    keys = new List<JwksViewModel> { webKey }
                })
            );
        }
        
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= 0) {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null) {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null) {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
