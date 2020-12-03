using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Globomantics.Api.Extenstions
{
    public static class GlobomanticsRequestLogging
    {

        public static IApplicationBuilder UseGlobomanticsStyleRequestLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = ExcludeHealthChecks;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
                    var user = httpContext.User.Identity;
                    if (user != null && user.IsAuthenticated)
                    {
                        var ci = user as ClaimsIdentity;
                        var email = ci?.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
                        diagnosticContext.Set("UserName", email);
                        diagnosticContext.Set("UserId",ci?.FindFirst(JwtClaimTypes.Subject)?.Value);
                    }
                };
            });

        }

        private static LogEventLevel ExcludeHealthChecks(HttpContext ctx, double _, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            var endpoint = ctx.GetEndpoint();
            if (endpoint != null) 
            {
                return string.Equals(endpoint.DisplayName, "Health checks", StringComparison.Ordinal);
            }
            return false; // No endpoint, so not a health check endpoint
        }
    }
}
