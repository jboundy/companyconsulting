using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Marten;
using OpenIddict.Abstractions;

namespace OAuth.Server.OIDC
{
    public class AuthorizationStore : IOpenIddictAuthorizationStore<AuthorizationDetail>
    {
        private readonly IDocumentStore _store;

        public AuthorizationStore(IDocumentStore store)
        {
            _store = store;
        }
        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<AuthorizationDetail>().LongCountAsync(cancellationToken);
        }

        public async ValueTask<long> CountAsync<TResult>(Func<IQueryable<AuthorizationDetail>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AuthorizationDetail>());
            return await queryable.LongCountAsync(cancellationToken!);
        }

        public async ValueTask CreateAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(authorization);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DeleteAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Delete(authorization);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async IAsyncEnumerable<AuthorizationDetail> FindAsync(string? subject, string? client, string? status, string? type, ImmutableArray<string>? scopes, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();

            IQueryable<AuthorizationDetail> query = session.Query<AuthorizationDetail>();

            if (!string.IsNullOrEmpty(subject))
            {
                query = query.Where(x => x.Subject == subject);
            }

            if (!string.IsNullOrEmpty(client))
            {
                query = query.Where(x => x.ClientId == client);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(x => x.Type == type);
            }

            var stream = query.ToAsyncEnumerable(cancellationToken);

            await foreach (var authorization in stream.WithCancellation(cancellationToken))
            {
                if (scopes.HasValue && !scopes.Value.IsDefaultOrEmpty)
                {
                    if (authorization.Scopes is null || !scopes.Value.All(s => authorization.Scopes.Contains(s)))
                    {
                        continue;
                    }
                }

                yield return authorization;
            }
        }

        public async IAsyncEnumerable<AuthorizationDetail> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();

            var stream = session.Query<AuthorizationDetail>()
                .Where(x => x.ClientId == identifier)
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var authorization in stream.WithCancellation(cancellationToken))
            {
                yield return authorization;
            }
        }

        public async ValueTask<AuthorizationDetail?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.LoadAsync<AuthorizationDetail>(identifier, cancellationToken);
        }

        public async IAsyncEnumerable<AuthorizationDetail> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<AuthorizationDetail>()
                .Where(auth => auth.Subject == subject)
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask<string?> GetApplicationIdAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.ClientId);
        }

        public async ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<AuthorizationDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AuthorizationDetail>(), state);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public ValueTask<DateTimeOffset?> GetCreationDateAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.CreationDate);
        }

        public ValueTask<string?> GetIdAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.Id);
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            if (authorization.Properties is null || authorization.Properties.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<string, JsonElement>.Empty);
            }

            return ValueTask.FromResult(authorization.Properties.ToImmutableDictionary());
        }

        public ValueTask<ImmutableArray<string>> GetScopesAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.Scopes.ToImmutableArray());
        }

        public ValueTask<string?> GetStatusAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.Status);
        }

        public ValueTask<string?> GetSubjectAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.Subject);
        }

        public ValueTask<string?> GetTypeAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(authorization.Type);
        }

        public ValueTask<AuthorizationDetail> InstantiateAsync(CancellationToken cancellationToken)
        {
            var authorization = new AuthorizationDetail
            {
                Id = Guid.NewGuid().ToString()
            };

            return ValueTask.FromResult(authorization);
        }

        public async IAsyncEnumerable<AuthorizationDetail> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            IQueryable<AuthorizationDetail> query = session.Query<AuthorizationDetail>();

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            var stream = query.ToAsyncEnumerable(cancellationToken);
            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<AuthorizationDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AuthorizationDetail>(), state);
            var stream = queryable.ToAsyncEnumerable(cancellationToken);

            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async ValueTask<long> PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            var prunableAuthorizations = await session.Query<AuthorizationDetail>()
                    .Where(auth => auth.Status == OpenIddictConstants.Statuses.Revoked
                                || auth.Status == OpenIddictConstants.Statuses.Inactive
                                || (auth.CreationDate != null && auth.CreationDate <= threshold))
                    .Select(auth => auth.Id)
                    .ToListAsync(cancellationToken);

            if (prunableAuthorizations.Count == 0)
            {
                return 0;
            }

            foreach (var id in prunableAuthorizations)
            {
                session.Delete<AuthorizationDetail>(id);
            }

            await session.SaveChangesAsync(cancellationToken);

            return prunableAuthorizations.Count;
        }

        public async ValueTask<long> RevokeAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            IQueryable<AuthorizationDetail> query = session.Query<AuthorizationDetail>();
            if (!string.IsNullOrEmpty(subject))
            {
                query = query.Where(x => x.Subject == subject);
            }

            if (!string.IsNullOrEmpty(client))
            {
                query = query.Where(x => x.ClientId == client);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(x => x.Type == type);
            }
            var authorizations = await query.ToListAsync(cancellationToken);

            if (authorizations.Count == 0)
            {
                return 0;
            }
            foreach (var authorization in authorizations)
            {
                authorization.Status = OpenIddictConstants.Statuses.Revoked;
                session.Store(authorization);
            }

            await session.SaveChangesAsync(cancellationToken);

            return authorizations.Count;
        }

        public async ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            var authorizations = await session.Query<AuthorizationDetail>()
                .Where(auth => auth.ClientId == identifier)
                .ToListAsync(cancellationToken);

            if (authorizations.Count == 0)
            {
                return 0;
            }

            foreach (var authorization in authorizations)
            {
                authorization.Status = OpenIddictConstants.Statuses.Revoked;
                session.Store(authorization);
            }

            await session.SaveChangesAsync(cancellationToken);

            return authorizations.Count;
        }

        public async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            var authorizations = await session.Query<AuthorizationDetail>()
                .Where(auth => auth.Subject == subject)
                .ToListAsync(cancellationToken);

            if (authorizations.Count == 0)
            {
                return 0;
            }

            foreach (var authorization in authorizations)
            {
                authorization.Status = OpenIddictConstants.Statuses.Revoked;
                session.Store(authorization);
            }

            await session.SaveChangesAsync(cancellationToken);

            return authorizations.Count;
        }

        public ValueTask SetApplicationIdAsync(AuthorizationDetail authorization, string? identifier, CancellationToken cancellationToken)
        {
            authorization.ClientId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetCreationDateAsync(AuthorizationDetail authorization, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            authorization.CreationDate = date;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPropertiesAsync(AuthorizationDetail authorization, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            authorization.Properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetScopesAsync(AuthorizationDetail authorization, ImmutableArray<string> scopes, CancellationToken cancellationToken)
        {
            authorization.Scopes = scopes.IsDefault ? [] : [.. scopes];
            return ValueTask.CompletedTask;
        }

        public ValueTask SetStatusAsync(AuthorizationDetail authorization, string? status, CancellationToken cancellationToken)
        {
            authorization.Status = status;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetSubjectAsync(AuthorizationDetail authorization, string? subject, CancellationToken cancellationToken)
        {
            authorization.Subject = subject;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetTypeAsync(AuthorizationDetail authorization, string? type, CancellationToken cancellationToken)
        {
            authorization.Type = type;
            return ValueTask.CompletedTask;
        }

        public async ValueTask UpdateAsync(AuthorizationDetail authorization, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(authorization);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}