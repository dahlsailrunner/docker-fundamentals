using Microsoft.AspNetCore.Authorization;

namespace Globomantics.Core.Authorization
{
    public class RequiresMfaChallengeAttribute : AuthorizeAttribute
    {
        public const string PolicyName = "MfaChallengeRequirement";

        public RequiresMfaChallengeAttribute()
        {
            Policy = PolicyName;
        }
    }
}
