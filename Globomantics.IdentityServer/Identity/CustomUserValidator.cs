using System.Data;
using System.Threading.Tasks;
using Dapper;
using Globomantics.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

namespace Globomantics.IdentityServer.Identity
{
    public class CustomUserValidator : IUserValidator<CustomUser>
    {
        private readonly IDbConnection _db;

        public CustomUserValidator(IDbConnection db)
        {
            _db = db;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<CustomUser> manager, CustomUser user)
        {
            var knownCompanyMember = _db.QuerySingleOrDefault<CompanyMember>(
                @"SELECT * FROM dbo.CompanyMembers WHERE MemberEmail = @Email",
                new {Email = user.Email});
            if (knownCompanyMember != null)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "UnknownMember",
                Description = "Provided username is not a valid company member."
            }));
        }
    }
}
