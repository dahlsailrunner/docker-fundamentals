using Microsoft.AspNetCore.Authorization;

namespace Globomantics.Core.Authorization
{
    public class RightRequirement : IAuthorizationRequirement
    {
        public string Right { get; }

        public RightRequirement(string right)
        {
            Right = right;
        }
    }
}
