using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Machete.Data;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using Serilog;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(Machete.Identity.Startup))]
namespace Machete.Identity
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Trace()
               .CreateLogger();

            app.Map("/admin", adminApp =>
            {
                 

                var factory = new IdentityManager.Configuration.IdentityManagerServiceFactory();
                factory.ConfigureSimpleIdentityManagerService("MacheteConnection");

                adminApp.UseIdentityManager(new IdentityManager.Configuration.IdentityManagerOptions()
                {
                    Factory = factory
                });
            });

            app.Map("/id", core =>
            {
                var factory = Factory.Configure();
                factory.UserService = new Registration<IUserService, UserService>();
                factory.Register(new Registration<UserManager>());
                factory.Register(new Registration<UserStore>());
                factory.Register(new Registration<MacheteContext>(resolver => new MacheteContext("MacheteConnection")));

                core.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Machete IdentityServer",
                    SigningCertificate = Certificate.Get(),
                    RequireSsl = true,
                    Factory = factory,

                    AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        EnablePostSignOutAutoRedirect = true,
                        IdentityProviders = ConfigureIdentityProviders,
                        //LoginPageLinks = new LoginPageLink[] {
                        //    new LoginPageLink{
                        //        Text = "Register",
                        //        Href = "localregistration"
                        //    }
                        //}
                    }
                });
            });
        }
        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GoogleClientId"]))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    AuthenticationType = "Google",
                    Caption = "Sign-in with Google",
                    SignInAsAuthenticationType = signInAsType,

                    ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
                });
            }
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["FacebookAppId"]))
            {

                var fb = new FacebookAuthenticationOptions
                {
                    AuthenticationType = "Facebook",
                    Caption = "Sign-in with Facebook",
                    SignInAsAuthenticationType = signInAsType,

                    AppId = ConfigurationManager.AppSettings["FacebookAppId"],
                    AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"],
                    Provider = new FacebookAuthenticationProvider()
                    {
                        OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(
                                new System.Security.Claims.Claim(
                                    "urn:facebook:access_token",
                                    context.AccessToken, ClaimValueTypes.String, "Facebook"));

                            return Task.FromResult(0);
                        }
                    }
                };
                fb.Scope.Add("email");
                app.UseFacebookAuthentication(fb);
            }

            //var y = new YahooAuthenticationOptions()
            //{
            //    AuthenticationType = "Yahoo",
            //    Caption = "Sign-in with Yahoo",
            //    SignInAsAuthenticationType = signInAsType,
            //    Provider = new YahooAuthenticationProvider()
            //    {
            //        OnAuthenticated = (context) =>
            //        {
            //            context.Identity.AddClaim(
            //                new Claim("AccessToken", context.AccessToken)
            //                    );
            //            return Task.FromResult(0);
            //        }
            //    },
            //    ConsumerKey = ConfigurationManager.AppSettings["YahooConsumerKey"],
            //    ConsumerSecret = ConfigurationManager.AppSettings["YahooConsumerSecret"]
            //};
            //app.UseYahooAuthentication(y);

            //var pp = new PayPalAuthenticationOptions()
            //{
            //    AuthenticationType = "PayPal",
            //    Caption = "Sign-in with PayPal",
            //    SignInAsAuthenticationType = signInAsType,
            //    Provider = new PayPalAuthenticationProvider()
            //    {

            //    },
            //    ClientId = ConfigurationManager.AppSettings["PayPalClientId"],
            //    ClientSecret = ConfigurationManager.AppSettings["PayPalClientSecret"]
            //};
            //app.UsePayPalAuthentication(pp);
        }
    }
}