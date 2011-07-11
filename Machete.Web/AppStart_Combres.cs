[assembly: WebActivator.PreApplicationStartMethod(typeof(Machete.Web.AppStart_Combres), "Start")]
namespace Machete.Web {
	using System.Web.Routing;
	using Combres;
	
    public static class AppStart_Combres {
        public static void Start() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}