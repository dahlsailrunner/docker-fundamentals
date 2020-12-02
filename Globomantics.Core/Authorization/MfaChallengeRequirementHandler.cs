using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Globomantics.Core.Authorization
{
    public class MfaChallengeRequirementHandler : AuthorizationHandler<MfaChallengeRequirement>
    {
        private readonly HttpContext _httpCtx;
        private readonly UrlEncoder _urlEncoder;

        public MfaChallengeRequirementHandler(IHttpContextAccessor httpContextAccessor, 
            UrlEncoder urlEncoder)
        {
            _httpCtx = httpContextAccessor.HttpContext;
            _urlEncoder = urlEncoder;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MfaChallengeRequirement requirement)
        {
            var amr = context.User.Claims.FirstOrDefault(c => c.Type == "amr")?.Value;

            if (string.Equals(amr, "mfa"))  // user signed in and was challenged
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var twoFactorEnabled = context.User.Claims
                .FirstOrDefault(c => c.Type == "MfaEnabled")?.Value;
            if (string.Equals(twoFactorEnabled, "true", StringComparison.InvariantCultureIgnoreCase))
            {
                // user has two-factor enabled but may not have been challenged
                //  cookie here is set by the twofactorchallenge page
                var alreadyDone = _httpCtx.Request.Cookies["MfaChallengeCompleted"];
                if (!string.IsNullOrEmpty(alreadyDone))
                {
                    // user has completed a challenge during current session
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                if (context.Resource is RouteEndpoint endpoint)
                {
                    var returnUrl = endpoint.DisplayName;
                    // TODO: handle parameters here if there are any
                    var encodedReturnUrl = _urlEncoder.Encode(returnUrl);
                    _httpCtx.Response.Redirect($"/TwoFactorChallenge?returnUrl={encodedReturnUrl}");
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            context.Fail();  // user does not have mfa enabled
            return Task.CompletedTask;
        }
    }
}
