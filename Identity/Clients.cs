using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Identity
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
            new Client
            {
                Enabled = true,
                ClientName = "JS Client",
                ClientId = "js",
                Flow = Flows.Implicit,

                RedirectUris = new List<string>
                {
                    "http://localhost:56668/popup.html",
                    // The new page is a valid redirect page after login
                    "http://localhost:56668/silent-renew.html",
                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:56668/index.html",
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:56668",
                    "http://localhost:4200"

                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "JMurphzyo!",
                ClientId = "oidc-client-poc",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "http://localhost:4200/auth.html",
                    // The new page is a valid redirect page after login
                    "http://localhost:4200/silent-renew.html"
                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:4200/index.html"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200"

                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "Machete UI (local)",
                ClientId = "machete-ui-local",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "http://localhost:4200/auth.html",
                    // The new page is a valid redirect page after login
                    "http://localhost:4200/silent-renewal.html"
                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:4200/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "postman",
                ClientId = "postman",
                ClientSecrets = new List<Secret>()
                {
                    new Secret("foo".Sha256())
                },
                Flow = Flows.AuthorizationCode,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "https://www.getpostman.com/oauth2/callback"
                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "https://www.getpostman.com/oauth2/callback"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "*"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            }

        };
        }
    }
}