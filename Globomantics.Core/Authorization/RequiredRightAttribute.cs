using Microsoft.AspNetCore.Authorization;

namespace Globomantics.Core.Authorization
{
    public class RequiredRightAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "RequiredRight";

        public RequiredRightAttribute(string right)
        {
            Right = right;
        }

        public string Right
        {
            get => Policy.Substring(PolicyPrefix.Length);
            set => Policy = $"{PolicyPrefix}{value}";
        }
    }
}
