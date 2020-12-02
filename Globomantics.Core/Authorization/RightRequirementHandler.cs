using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Globomantics.Core.Authorization
{
    public class RightRequirementHandler : AuthorizationHandler<RightRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            RightRequirement requirement)
        {
            var role = context.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
            {
                context.Fail();
            }
            else if (await RoleHasRight(context.User, role, requirement.Right))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        private Task<bool> RoleHasRight(ClaimsPrincipal user, string role, string right)
        {
            var rightsForRoles = new Dictionary<string, List<string>>
            {
                {"admin", new List<string> {"ViewMembers", "UpdateMembers", "ViewProfile"}},
                {"general", new List<string> {"ViewProfile", "SeeDashboard" }}
            };
            // this could be a cached db lookup or something based on userid, a list of roles, or whatever.
            return Task.FromResult(
                rightsForRoles.ContainsKey(role) &&
                rightsForRoles[role].Exists(a => a == right));

            // based on role , companyid, or other claims - look up requested right 
            // return existence of right for that combo
        }
    }
}
