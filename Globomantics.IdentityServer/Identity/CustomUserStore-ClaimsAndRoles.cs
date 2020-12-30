using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Globomantics.IdentityServer.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Globomantics.IdentityServer.Identity
{
    public partial class CustomUserStore : IUserClaimStore<CustomUser>
    {
        public async Task<IList<Claim>> GetClaimsAsync(CustomUser user, CancellationToken cancellationToken)
        {
            var claims = new List<Claim>();
            //var memberInfo = await _db.QueryFirstOrDefaultAsync<CompanyMember>(
            //    "SELECT * FROM CompanyMembers WHERE MemberEmail = @LoginName",
            //    new { user.LoginName });

            //if (memberInfo != null)
            //{
            //    claims.Add(new Claim("CompanyId", Convert.ToString(memberInfo.CompanyId)));
            //}

            claims.Add(user.LoginName == "kim@mars.com"
                ? new Claim(JwtClaimTypes.Role, "admin")
                : new Claim(JwtClaimTypes.Role, "general"));

            claims.Add(new Claim("MfaEnabled", Convert.ToString(user.TwoFactorEnabled)));

            return claims;
        }

        public Task AddClaimsAsync(CustomUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(CustomUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(CustomUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CustomUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
