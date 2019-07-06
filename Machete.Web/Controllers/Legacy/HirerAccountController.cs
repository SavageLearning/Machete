using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers
{
    public class HirerAccountController : Controller
    {
        // GET: HirerAccount
        public void Index()
        {
            Response.Redirect("/V2/welcome", true);
        }
    }
}