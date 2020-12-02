using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Globomantics.IdentityServer.Identity
{
    public partial class CustomUserStore : IUserPasswordStore<CustomUser>, 
                                   IUserEmailStore<CustomUser>,
                                   IUserSecurityStampStore<CustomUser>
    {
        private readonly IDbConnection _db;
        private bool _disposed;

        public CustomUserStore(IDbConnection db)
        {
            _db = db;
        }

        public Task<string> GetUserIdAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LoginName);
        }

        public Task SetUserNameAsync(CustomUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.LoginName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LoginName.ToUpper());
        }

        public Task SetNormalizedUserNameAsync(CustomUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            IdentityResult result;
            try
            {
                user.CreateDate = DateTime.Now;
                user.PasswordModifiedDate = user.CreateDate;
                await _db.ExecuteAsync(
                    @"
INSERT INTO GlobomanticsUser 
( LoginName, PasswordHash, PasswordModifiedDate, LastLoginDate, CreateDate, Status, SecurityStamp, 
    EmailConfirmed, TwoFactorEnabled, AccessFailedCount, LockoutEnd, LockoutEnabled )
VALUES
( @LoginName, @PasswordHash, @PasswordModifiedDate,@LastLoginDate, @CreateDate, 1, @SecurityStamp, 
    @EmailConfirmed, @TwoFactorEnabled, @AccessFailedCount, @LockoutEnd, @LockoutEnabled )",
                    user);
                result = IdentityResult.Success;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating identity.");
                result = IdentityResult.Failed();
            }
            return result;
        }

        public async Task<IdentityResult> UpdateAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            IdentityResult result;
            try
            {
                await _db.ExecuteAsync(
                    @"
UPDATE GlobomanticsUser 
SET PasswordHash = @PasswordHash 
   ,PasswordSalt = @PasswordSalt
   ,LoginName = @LoginName
   ,PasswordModifiedDate = @PasswordModifiedDate
   ,LastLoginDate = @LastLoginDate
   ,CreateDate = @CreateDate
   ,Status = @Status
   ,SecurityStamp = @SecurityStamp
   ,EmailConfirmed = @EmailConfirmed
   ,TwoFactorEnabled = @TwoFactorEnabled
   ,AccessFailedCount = @AccessFailedCount
   ,LockoutEnd = @LockoutEnd
   ,LockoutEnabled = @LockoutEnabled
WHERE UserId = @UserId",
                    user);
                result = IdentityResult.Success;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating identity.");
                result = IdentityResult.Failed();
            }
            return result;
        }

        public Task<IdentityResult> DeleteAsync(CustomUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return await _db.QuerySingleOrDefaultAsync<CustomUser>(
                "SELECT * FROM GlobomanticsUser WHERE UserId = @userId",
                new { userId });
        }

        public async Task<CustomUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (normalizedUserName == null)
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }
            var result = await _db.QuerySingleOrDefaultAsync<CustomUser>(
                "SELECT * FROM GlobomanticsUser WHERE LoginName = @LoginName",
                new { LoginName = normalizedUserName });
            return result;
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.PasswordHash = passwordHash;
            user.PasswordSalt = null;
            user.PasswordModifiedDate = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetEmailAsync(CustomUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(CustomUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<CustomUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (normalizedEmail == null)
            {
                throw new ArgumentNullException(nameof(normalizedEmail));
            }
            return FindByNameAsync(normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(CustomUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(CustomUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.SecurityStamp ?? "");
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        public void Dispose() { _disposed = true; }
        
    }
}
