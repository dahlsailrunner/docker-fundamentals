using System.Collections.Generic;
using System.Linq;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Globomantics.IdentityServer.Initialization
{
    // based on code found here: 
    // https://github.com/IdentityServer/IdentityServer4.Demo/blob/main/src/IdentityServer4Demo/Config.cs
    public static class InitialConfiguration
    {
        public static void PopulateDatabaseIfEmpty(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in GetApiScopes())
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }

                    context.SaveChanges();
                }
            }
        }

        public static IEnumerable<Client> GetClients()
        {
            
            return new List<Client>
            {
                new Client
                {
                    ClientId = "globo-core",
                    ClientName = "Globomantics Core UI (Code with PKCE)",

                    RedirectUris = { "https://localhost:44320/signin-oidc" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowedScopes = { "openid", "profile", "glob_profile", "email", "glob_api" },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                new Client
                {
                    ClientId = "globo-swagger",
                    ClientName = "Swagger UI for the Globomantics API",
                    RedirectUris = { "https://localhost:44376/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "https://notused" },

                    RequireConsent = false,
                    AllowedCorsOrigins = new List<string> { "https://localhost:44376" },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = { "openid", "profile", "email", "glob_profile", "glob_api"   }
                }
            };
        }
        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("glob_profile", new List<string>{"MfaEnabled", "CompanyId", JwtClaimTypes.Role})
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("glob_api"),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("glob_api", "Globomantics API")
                {
                    Scopes = { "glob_api" }
                }
            };
        }
    }
}
