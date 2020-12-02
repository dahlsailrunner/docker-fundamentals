using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Globomantics.Core.Authorization
{
    public class CustomPolicyProvider : IAuthorizationPolicyProvider
    {
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

        public CustomPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(RequiredRightAttribute.PolicyPrefix, 
                StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new 
                        RightRequirement(policyName.Substring(RequiredRightAttribute.PolicyPrefix.Length)))
                    .Build();

                return Task.FromResult(policy);
            }

            if (policyName == RequiresMfaChallengeAttribute.PolicyName)
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new MfaChallengeRequirement())
                    .Build();

                return Task.FromResult(policy);
            }

            return BackupPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme, "oidc")
                .RequireAuthenticatedUser()
                .Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
