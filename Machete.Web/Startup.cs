using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Machete.Web.Startup))]
namespace Machete.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //var api = new Machete.Api.Startup();
            //api.Configuration(app);
        }
    }
}
