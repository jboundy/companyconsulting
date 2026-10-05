using Identity.Domain.Models;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace OAuth.Server.Stores
{
    public class UserStore : IUserStore<DomainUser>, IUserRoleStore<DomainUser>
    {
        private readonly IDocumentSession _session;

        public UserStore(IDocumentSession session)
        {
            _session = session;
        }

        public Task AddToRoleAsync(DomainUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Roles ??= [];
            if (!user.Roles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
            {
                user.Roles.Add(roleName);
            }
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            _session.Store(user);
            await _session.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            _session.Delete(user);
            await _session.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public async Task<DomainUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Guid.TryParse(userId, out var id))
            {
                return await _session.LoadAsync<DomainUser>(id, cancellationToken);
            }
            return null;
        }

        public async Task<DomainUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _session.Query<DomainUser>()
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string?> GetNormalizedUserNameAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<IList<string>> GetRolesAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IList<string> roles = user.Roles ?? new List<string>();
            return Task.FromResult(roles);
        }

        public Task<string> GetUserIdAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<DomainUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var users = await _session.Query<DomainUser>()
            .Where(u => u.Roles.Contains(roleName))
            .ToListAsync(cancellationToken);

            return users.ToList();
        }

        public Task<bool> IsInRoleAsync(DomainUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var isInRole = user.Roles?.Any(r => r.Equals(roleName, StringComparison.OrdinalIgnoreCase)) ?? false;
            return Task.FromResult(isInRole);
        }

        public Task RemoveFromRoleAsync(DomainUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Roles?.RemoveAll(r => r.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(DomainUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(DomainUser user, string? userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(DomainUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            _session.Update(user);
            await _session.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
    }
}