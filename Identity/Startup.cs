using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Machete.Data;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

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
                        LoginPageLinks = new LoginPageLink[] {
                            new LoginPageLink{
                                Text = "Register",
                                Href = "localregistration"
                            }
                        }
                    }
                });
            });
        }
        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
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
    }
}