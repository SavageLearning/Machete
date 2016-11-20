using Machete.Service;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Machete.Api.Controllers
{
    [ElmahHandleError]
    [System.Web.Http.Authorize]
    [RoutePrefix("api/employer")]
    public class EmployerController : ApiController
    {
        private readonly IEmployerService serv;
        private readonly IWorkOrderService woServ;
        private System.Globalization.CultureInfo CI;

        public EmployerController(IEmployerService employerService, IWorkOrderService workorderService)
        {
            this.serv = employerService;
            this.woServ = workorderService;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}