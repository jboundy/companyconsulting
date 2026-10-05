using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Marten;
using OpenIddict.Abstractions;

namespace OAuth.Server.OIDC
{
    public class TokenStore : IOpenIddictTokenStore<TokenDetail>
    {
        private readonly IDocumentStore _store;

        public TokenStore(IDocumentStore store)
        {
            _store = store;
        }

        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<TokenDetail>().LongCountAsync(cancellationToken);
        }

        public async ValueTask<long> CountAsync<TResult>(Func<IQueryable<TokenDetail>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<TokenDetail>());
            return await queryable.LongCountAsync(cancellationToken!);
        }

        public async ValueTask CreateAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(token);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DeleteAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Delete(token);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async IAsyncEnumerable<TokenDetail> FindAsync(string? subject, string? client, string? status, string? type, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();

            IQueryable<TokenDetail> query = session.Query<TokenDetail>();

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

            await foreach (var token in stream.WithCancellation(cancellationToken))
            {
                yield return token;
            }
        }

        public async IAsyncEnumerable<TokenDetail> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<TokenDetail>()
                .Where(auth => auth.ApplicationId == identifier)
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<TokenDetail> FindByAuthorizationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<TokenDetail>()
                .Where(auth => auth.AuthorizationId == identifier)
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async ValueTask<TokenDetail?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.LoadAsync<TokenDetail>(identifier, cancellationToken);
        }

        public async ValueTask<TokenDetail?> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            await using var session = _store.QuerySession();

            return await session.Query<TokenDetail>()
                .FirstOrDefaultAsync(token => token.ReferenceId == identifier, cancellationToken);
        }

        public async IAsyncEnumerable<TokenDetail> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<TokenDetail>()
                .Where(auth => auth.Subject == subject)
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask<string?> GetApplicationIdAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.ApplicationId);
        }

        public async ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<TokenDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<TokenDetail>(), state);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public ValueTask<string?> GetAuthorizationIdAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.AuthorizationId);
        }

        public ValueTask<DateTimeOffset?> GetCreationDateAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.CreationDate);
        }

        public ValueTask<DateTimeOffset?> GetExpirationDateAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.ExpirationDate);
        }

        public ValueTask<string?> GetIdAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.Id);
        }

        public ValueTask<string?> GetPayloadAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.Payload);
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            if (token.Properties is null || token.Properties.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<string, JsonElement>.Empty);
            }

            return ValueTask.FromResult(token.Properties.ToImmutableDictionary());
        }

        public ValueTask<DateTimeOffset?> GetRedemptionDateAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.RedemptionDate);
        }

        public ValueTask<string?> GetReferenceIdAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.ReferenceId);
        }

        public ValueTask<string?> GetStatusAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.Status);
        }

        public ValueTask<string?> GetSubjectAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.Subject);
        }

        public ValueTask<string?> GetTypeAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(token.Type);
        }

        public ValueTask<TokenDetail> InstantiateAsync(CancellationToken cancellationToken)
        {
            var token = new TokenDetail
            {
                Id = Guid.NewGuid().ToString()
            };

            return ValueTask.FromResult(token);
        }

        public async IAsyncEnumerable<TokenDetail> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            IQueryable<TokenDetail> query = session.Query<TokenDetail>();

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

        public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<TokenDetail>, TState, IQueryable<TResult>> query, TState state, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<TokenDetail>(), state);
            var stream = queryable.ToAsyncEnumerable(cancellationToken);

            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async ValueTask<long> PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            var prunableAuthorizations = await session.Query<TokenDetail>()
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
                session.Delete<TokenDetail>(id);
            }

            await session.SaveChangesAsync(cancellationToken);

            return prunableAuthorizations.Count;
        }

        public async ValueTask<long> RevokeAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            IQueryable<TokenDetail> query = session.Query<TokenDetail>();
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

        public async ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
        {
            await using var session = _store.LightweightSession();
            var authorizations = await session.Query<TokenDetail>()
                .Where(auth => auth.ApplicationId == identifier)
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

        public async ValueTask<long> RevokeByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            var authorizations = await session.Query<TokenDetail>()
                .Where(auth => auth.AuthorizationId == identifier)
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

        public async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
        {
            await using var session = _store.LightweightSession();
            var authorizations = await session.Query<TokenDetail>()
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

        public ValueTask SetApplicationIdAsync(TokenDetail token, string? identifier, CancellationToken cancellationToken)
        {
            token.ApplicationId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetAuthorizationIdAsync(TokenDetail token, string? identifier, CancellationToken cancellationToken)
        {
            token.AuthorizationId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetCreationDateAsync(TokenDetail token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.CreationDate = date;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetExpirationDateAsync(TokenDetail token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.ExpirationDate = date;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPayloadAsync(TokenDetail token, string? payload, CancellationToken cancellationToken)
        {
            token.Payload = payload;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPropertiesAsync(TokenDetail token, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            token.Properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetRedemptionDateAsync(TokenDetail token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.RedemptionDate = date;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetReferenceIdAsync(TokenDetail token, string? identifier, CancellationToken cancellationToken)
        {
            token.ReferenceId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetStatusAsync(TokenDetail token, string? status, CancellationToken cancellationToken)
        {
            token.Status = status;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetSubjectAsync(TokenDetail token, string? subject, CancellationToken cancellationToken)
        {
            token.Subject = subject;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetTypeAsync(TokenDetail token, string? type, CancellationToken cancellationToken)
        {
            token.Type = type;
            return ValueTask.CompletedTask;
        }

        public async ValueTask UpdateAsync(TokenDetail token, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(token);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}