using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Machete.Api
{
    [Route("values")]
    public class ValuesController : ApiController
    {
        private static readonly Random _random = new Random();
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue ="Administrator")]
        public IEnumerable<string> Get()
        {
            var random = new Random();
            return new[]
            {
                _random.Next(0, 10).ToString(),
                _random.Next(0, 10).ToString()
            };
        }
    }
}