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
                ClientName = "Machete UI (local)",
                ClientId = "machete-ui-local",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "http://localhost:4200/auth.html",
                    "http://localhost:4200/authorize",
                    "http://localhost:4200/silent-renewal.html",
                    "http://localhost:4200/silent-renewal",
                    "http://localhost:4200/V2/auth.html",
                    "http://localhost:4200/V2/authorize",
                    "http://localhost:4200/V2/silent-renewal.html",
                    "http://localhost:4200/V2/silent-renewal"

                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:4200/logout",
                    "http://localhost:4200/V2/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200",
                    "http://localhost:4200/V2"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "Machete UI (local-embedded)",
                ClientId = "machete-ui-local-embedded",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "http://localhost:4213/V2/auth.html",
                    "http://localhost:4213/V2/authorize",
                    "http://localhost:4213/V2/silent-renewal.html",
                    "http://localhost:4213/V2/silent-renewal"

                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:4213/V2/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4213/V2"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "Machete UI (cloud-chaim)",
                ClientId = "machete-ui-cloud-chaim",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "https://chaim.machetessl.org/V2/auth.html",
                    "https://chaim.machetessl.org/V2/authorize",
                    "https://chaim.machetessl.org/V2/silent-renewal.html",
                    "https://chaim.machetessl.org/V2/silent-renewal"

                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "https://chaim.machetessl.org/V2/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "https://chaim.machetessl.org/V2"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "Machete UI (cloud-test)",
                ClientId = "machete-ui-cloud-test",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "https://test.machetessl.org/V2/auth.html",
                    "https://test.machetessl.org/V2/authorize",
                    "https://test.machetessl.org/V2/silent-renewal.html",
                    "https://test.machetessl.org/V2/silent-renewal"

                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "https://test.machetessl.org/V2/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "https://test.machetessl.org/V2"
                },

                AllowAccessToAllScopes = true,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                Enabled = true,
                ClientName = "Machete casa UI",
                ClientId = "machete-casa-prod",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "https://casa.machetessl.org/V2/auth.html",
                    "https://casa.machetessl.org/V2/authorize",
                    "https://casa.machetessl.org/V2/silent-renewal.html",
                    "https://casa.machetessl.org/V2/silent-renewal"

                },
                // Valid URLs after logging out
                PostLogoutRedirectUris = new List<string>
                {
                    "https://casa.machetessl.org/V2/logout"
                },
                AllowedCorsOrigins = new List<string>
                {
                    "https://casa.machetessl.org/V2"
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
