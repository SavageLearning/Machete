using System;
using System.Collections.Generic;
using System.Web.Http;
using Thinktecture.IdentityModel.Mvc;

namespace Machete.Api
{
    [Route("values")]
    public class ValuesController : ApiController
    {
        private static readonly Random _random = new Random();
        [ClaimsAuthorization(ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValue ="Administrator")]
        public IEnumerable<string> Get()
        {
            var random = new Random();
            throw new Exception("err-ror");
            return new[]
            {
                _random.Next(0, 10).ToString(),
                _random.Next(0, 10).ToString()
            };
        }
    }
}