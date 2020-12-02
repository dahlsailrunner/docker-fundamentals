using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Globomantics.IdentityServer.Identity
{
    public partial class CustomUserStore : IUserTwoFactorStore<CustomUser>,
                                           IUserAuthenticatorKeyStore<CustomUser>,
                                           IUserTwoFactorRecoveryCodeStore<CustomUser>
    {
        private const string AuthenticatorKeyName = "AuthenticatorKey";
        private const string RecoveryCodesName = "RecoveryCodes";
        private const string ProviderName = "Globomantics";

        public Task SetTwoFactorEnabledAsync(CustomUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(CustomUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetAuthenticatorKeyAsync(CustomUser user, string key, CancellationToken cancellationToken)
        {
            return PersistUserTokenAsync(user, AuthenticatorKeyName, key);
        }

        public Task<string> GetAuthenticatorKeyAsync(CustomUser user, CancellationToken cancellationToken)
        {
            if (_db.State != ConnectionState.Open) _db.Open();

            return _db.QuerySingleOrDefaultAsync<string>(
                @"
SELECT Value FROM dbo.UserToken 
WHERE UserId = @UserId 
AND LoginProvider = @ProviderName
AND Name = @AuthenticatorKeyName",
                new { user.UserId, ProviderName, AuthenticatorKeyName });
        }

        public Task ReplaceCodesAsync(CustomUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var key = string.Join(";", recoveryCodes);

            return PersistUserTokenAsync(user, RecoveryCodesName, key);
        }

        public Task<bool> RedeemCodeAsync(CustomUser user, string code, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountCodesAsync(CustomUser user, CancellationToken cancellationToken)
        {
            if (_db.State != ConnectionState.Open) _db.Open();

            var x = await _db.ExecuteScalarAsync<string>(
                @"
SELECT Value 
FROM dbo.UserToken 
WHERE UserId = @UserId 
AND LoginProvider = @ProviderName
AND Name = @RecoveryCodesName",
                new { user.UserId, ProviderName, RecoveryCodesName });

            return (string.IsNullOrEmpty(x)) ? 0 : x.Split(";").Count();
        }

        private Task<int> PersistUserTokenAsync(CustomUser user, string name, string key)
        {
            return _db.ExecuteAsync(@"
MERGE dbo.UserToken WITH (SERIALIZABLE) AS t
USING (VALUES (@UserId, @ProviderName, @Name, @Key)) AS u (UserId, LoginProvider, Name, Value)
    ON u.UserId = t.UserId
    AND u.LoginProvider = t.LoginProvider
    AND u.Name = t.Name
WHEN MATCHED THEN 
    UPDATE SET t.Value = u.Value
WHEN NOT MATCHED THEN
    INSERT (UserId, LoginProvider, Name, Value) 
    VALUES (u.UserId, u.LoginProvider, u.Name, u.Value);",
                new { user.UserId, ProviderName, name, key });
        }
    }
}
