using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Globomantics.Core.Authorization;
using Globomantics.Core.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Globomantics.Core
{
    public class Startup
    {
        private readonly AssemblyName _assembly;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _assembly = GetType().Assembly.GetName();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbConnection, SqlConnection>(db =>
                new SqlConnection(Configuration.GetConnectionString("GlobomanticsDb")));

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "mvccode";
                    options.AccessDeniedPath = "/AccessDenied";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.ResponseType = "code";
                    options.UsePkce = true;

                    AddOidcSettingsFromConfig(options);

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.ClaimActions.MapJsonKey("MfaEnabled", "MfaEnabled");
                    options.ClaimActions.MapJsonKey("CompanyId", "CompanyId");
                    options.ClaimActions.MapJsonKey("role", "role");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTicketReceived = e =>
                        {
                            e.Principal = DoClaimsTransformation(e.Principal);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAccessTokenManagement();

            services.AddHttpClient<IApiClient, ApiClient>()
                .AddUserAccessTokenHandler()
                .AddResiliencePolicies();

            services.AddSingleton<IAuthorizationPolicyProvider, CustomPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, RightRequirementHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MfaRequired",
                    p =>
                    {
                        p.RequireClaim("CompanyId");
                        p.RequireClaim("MfaEnabled", "True");
                    });
            });

            services.AddRazorPages();
        }

        private void AddOidcSettingsFromConfig(OpenIdConnectOptions options)
        {
            options.Authority = Configuration.GetValue<string>("AuthN:Authority");
            options.ClientId = Configuration.GetValue<string>("AuthN:ClientId");
            options.ClientSecret = Configuration.GetValue<string>("AuthN:ClientSecret");
            options.Scope.Clear();
            var allScopes = Configuration.GetValue<string>("AuthN:Scopes");
            foreach (var scope in allScopes.Split(' '))
            {
                options.Scope.Add(scope);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = (diagCtx, httpCtx) =>
                {
                    diagCtx.Set("Machine", Environment.MachineName);
                    diagCtx.Set("Assembly", _assembly.Name);
                    diagCtx.Set("Version", _assembly.Version);
                    diagCtx.Set("ClientIP", httpCtx.Connection.RemoteIpAddress);
                    diagCtx.Set("UserAgent", httpCtx.Request.Headers["User-Agent"]);
                    if (httpCtx.User.Identity.IsAuthenticated)
                    {
                        diagCtx.Set("UserName", httpCtx.User.Identity?.Name);
                    }
                };
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages().RequireAuthorization();
            });
        }

        private ClaimsPrincipal DoClaimsTransformation(ClaimsPrincipal argPrincipal)
        {
            var claims = argPrincipal.Claims.ToList();
            claims.Add(new Claim("somenewclaim", "something"));

            return new ClaimsPrincipal(new ClaimsIdentity(claims, argPrincipal.Identity.AuthenticationType,
                JwtClaimTypes.Name, JwtClaimTypes.Role));
        }
    }
}
