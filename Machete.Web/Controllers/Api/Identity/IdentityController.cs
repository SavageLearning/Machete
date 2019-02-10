using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Machete.Data;
using Machete.Web.Helpers.Api;
using Machete.Web.Helpers.Api.Identity;
using Machete.Web.ViewModel.Api.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        private RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly List<IdentityRole> _roles;

        public IdentityController(UserManager<MacheteUser> userManager,
            SignInManager<MacheteUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions
        )
        {
            ThrowIfInvalidOptions(jwtOptions.Value);

            _userManager = userManager;
            _signinManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            
            _roles = roleManager.Roles.ToList();
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }
        
        [HttpGet]
        [Route("authorize")]
        public async Task<IActionResult> Authorize([FromQuery(Name = "redirect_uri")] string redirectUri)
            //, [FromQuery(Name = "nonce")] Guid nonce)
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
                new OkObjectResult(new { data = redirectUri, access_token = jwt })
            );
        }

        // POST id/accounts
        [HttpPost("accounts")] // TODO remove; for testing only
        public async Task<IActionResult> Accounts([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdentity = _mapper.Map<RegistrationViewModel, MacheteUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new JsonResult("Account created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsViewModel model)
        {
            // Validation logic
            if (!ValidateLogin(model)) return BadRequest(ModelState);

            var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.Remember, false);

            if (result?.Succeeded != true)
            {
                ModelState.TryAddModelError("login_failure", "Invalid username or password.");
                return BadRequest(ModelState);
            }

            // just trying to login
            var v2AuthEndpoint = Routes.GetHostFrom(Request).V2AuthorizationEndpoint();
            if (model.Redirect == v2AuthEndpoint) return await Task.FromResult(new OkObjectResult(model.Redirect));

            // trying to hit another page and need to go back there; Angular will handle that
            var redirectUri = $"{v2AuthEndpoint}?redirect_url={model.Redirect}";
            return await Task.FromResult(new OkObjectResult(redirectUri));
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
            await _signinManager.SignOutAsync();
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
