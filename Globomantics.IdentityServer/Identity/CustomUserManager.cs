using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Globomantics.IdentityServer.Identity
{
    public class CustomUserManager : UserManager<CustomUser>
    {
        //https://github.com/dotnet/aspnetcore/blob/a190fd34854542266956b1af980c19afacb95feb/src/Identity/Extensions.Core/src/UserManager.cs
        public CustomUserManager(IUserStore<CustomUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<CustomUser> passwordHasher, 
            IEnumerable<IUserValidator<CustomUser>> userValidators, 
            IEnumerable<IPasswordValidator<CustomUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<CustomUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, 
                keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> AccessFailedAsync(CustomUser user)
        {
            ThrowIfDisposed();
            var store = GetUserLockoutStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // If this puts the user over the threshold for lockout,
            // lock them out and reset the access failed count
            var count = await store.IncrementAccessFailedCountAsync(user, CancellationToken);
            if (count < Options.Lockout.MaxFailedAccessAttempts || !user.LockoutEnabled)
            {
                return await UpdateUserAsync(user);
            }
            Logger.LogWarning(12, "User is locked out.");
            await store.SetLockoutEndDateAsync(user, 
                DateTimeOffset.UtcNow.Add(Options.Lockout.DefaultLockoutTimeSpan),
                CancellationToken);
            await store.ResetAccessFailedCountAsync(user, CancellationToken);
            return await UpdateUserAsync(user);
        }

        private IUserLockoutStore<CustomUser> GetUserLockoutStore()
        {
            if (!(Store is IUserLockoutStore<CustomUser> cast))
            {
                throw new NotSupportedException("User store does not implement IUserLockoutStore.");
            }
            return cast;
        }
    }
}
