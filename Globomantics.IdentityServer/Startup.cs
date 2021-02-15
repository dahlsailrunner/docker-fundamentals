using System;
using System.Data;
using System.Net.Http;
using System.Reflection;
using Globomantics.IdentityServer.Identity;
using Globomantics.IdentityServer.Initialization;
using Globomantics.IdentityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;

namespace Globomantics.IdentityServer
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
            services.AddRazorPages();

            services.AddScoped<IDbConnection, SqlConnection>(db =>
                new SqlConnection(Configuration.GetConnectionString("GlobomanticsDb")));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies(o =>
            {
                o.ApplicationCookie.Configure(opts =>
                {
                    opts.LoginPath = "/Account/Login";
                    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    opts.LogoutPath = "/Account/Logout";
                });
                o.ExternalCookie.Configure(opts =>
                {
                    opts.LoginPath = "/Account/Login";
                    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    opts.LogoutPath = "/Account/Logout";
                });
            });

            const int considerPwned = 1000;
            services.AddPwnedPasswordHttpClient(minimumFrequencyToConsiderPwned: considerPwned)
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(1)));

            services.AddScoped<IPasswordHasher<CustomUser>, CustomPasswordHasher>();

            services.AddTransient<IConfigureOptions<IdentityOptions>, CustomIdentityOptions>();

            services.AddIdentityCore<CustomUser>()
                .AddSignInManager<SignInManager<CustomUser>>()
                //.AddUserManager<UserManager<CustomUser>>()
                .AddUserManager<CustomUserManager>()
                .AddUserStore<CustomUserStore>()
                // not including phone number provider
                .AddTokenProvider<DataProtectorTokenProvider<CustomUser>>(TokenOptions.DefaultProvider)
                .AddTokenProvider<EmailTokenProvider<CustomUser>>(TokenOptions.DefaultEmailProvider)
                .AddTokenProvider<AuthenticatorTokenProvider<CustomUser>>(TokenOptions.DefaultAuthenticatorProvider)
                .AddDefaultUI()
                .AddPwnedPasswordValidator<CustomUser>(options =>
                {
                    options.ErrorMessage =
                        $"Cannot use passwords that have been pwned more than {considerPwned} times.";
                })
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>();

            var connStr = Configuration.GetConnectionString("IS4DbConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.AccessTokenJwtType = string.Empty;
                    options.EmitStaticAudienceClaim = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                })
                //.AddInMemoryApiResources(InitialConfiguration.GetApis())
                //.AddInMemoryApiScopes(InitialConfiguration.GetApiScopes())
                //.AddInMemoryIdentityResources(InitialConfiguration.GetIdentityResources())
                //.AddInMemoryClients(InitialConfiguration.GetClients())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connStr, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connStr, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<CustomUser>();

            services.AddTransient<IEmailSender>(s => new EmailSender(Configuration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplyDatabaseSchema();
            app.PopulateDatabaseIfEmpty();

            var forwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwardedHeaderOptions.KnownNetworks.Clear();
            forwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedHeaderOptions);

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
                    if (httpCtx.User.Identity?.IsAuthenticated == true)
                    {
                        diagCtx.Set("UserName", httpCtx.User.Identity?.Name);
                    }
                };
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
