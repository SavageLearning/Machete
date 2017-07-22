using AutoMapper;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [AllowAnonymous]
    public class OnlineOrdersController : ApiController
    {
        private readonly IWorkOrderService serv;
        private readonly IMapper map;
        public OnlineOrdersController(IOnlineOrdersService serv, IMapper map)
        {
           // this.serv = serv;
            this.map = map;
        }
        // GET: api/OnlineOrders
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/OnlineOrders/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OnlineOrders
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OnlineOrders/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OnlineOrders/5
        public void Delete(int id)
        {
        }
    }
}
