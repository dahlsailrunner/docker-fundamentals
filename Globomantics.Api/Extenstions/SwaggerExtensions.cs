using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Globomantics.Api.Extenstions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration config, IApiVersionDescriptionProvider provider)
        {
            var clientId = config.GetValue<string>("AuthN:SwaggerClientId");
            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Globomantics API {description.GroupName.ToUpperInvariant()}");
                        options.RoutePrefix = string.Empty;
                    }
                    options.DocumentTitle = "Globomantics API Documentation";
                    options.OAuthClientId(clientId);
                    options.OAuthAppName("Globomantics");
                    options.OAuthUsePkce();
                });

            return app;
        }
    }
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IConfiguration _config;
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IConfiguration config, IApiVersionDescriptionProvider provider)
        {
            _config = config;
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var disco = GetDiscoveryDocument();

            var apiScope = _config.GetValue<string>("AuthN:ApiName");
            var scopes = apiScope.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var additionalScopes = _config.GetValue<string>("AuthN:AdditionalScopes");
            scopes.AddRange(additionalScopes.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());

            var oauthScopeDic = new Dictionary<string, string>();
            foreach (var scope in scopes)
            {
                oauthScopeDic.Add(scope, $"Resource access: {scope}");
            }
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Globomantics API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    });
            }
            options.EnableAnnotations();

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                        TokenUrl = new Uri(disco.TokenEndpoint),
                        Scopes = oauthScopeDic
                    }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                    },
                    oauthScopeDic.Keys.ToArray()
                }
            });
        }
        private DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            var client = new HttpClient();
            var authority = _config.GetValue<string>("AuthN:Authority");
            return client.GetDiscoveryDocumentAsync(authority)
                .GetAwaiter()
                .GetResult();
        }
    }
}
