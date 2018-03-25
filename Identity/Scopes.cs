using IdentityManager;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Identity
{
    public class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
        {
            StandardScopes.OpenId,
            StandardScopes.Profile,
            StandardScopes.Email,
            StandardScopes.RolesAlwaysInclude,
            StandardScopes.OfflineAccess,
            new Scope
            {
                Name = "api",

                DisplayName = "Access to API",
                Description = "This will grant you access to the API",
                ScopeSecrets = new List<Secret>
                {
                    new Secret("api-secret".Sha256())
                },
                Claims = new List <ScopeClaim>{
                    new ScopeClaim(Constants.ClaimTypes.Role, alwaysInclude: true),
                    new ScopeClaim(Constants.ClaimTypes.Email, alwaysInclude: true)
                },
                Type = ScopeType.Resource
            }
        };
        }
    }
}