using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Marten;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace OAuth.Server.OIDC
{
    public class AppStore : IOpenIddictApplicationStore<AppDetail>
    {
        private readonly IDocumentStore _store;

        public AppStore(IDocumentStore store)
        {
            _store = store;
        }

        public async ValueTask<AppDetail?> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<AppDetail>()
                .FirstOrDefaultAsync(x => x.Id == identifier, cancellationToken);
        }

        public async ValueTask<AppDetail?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.LoadAsync<AppDetail>(identifier, cancellationToken);
        }

        public async ValueTask CreateAsync(AppDetail application, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(application);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<AppDetail>().LongCountAsync(cancellationToken);
        }

        public async ValueTask<long> CountAsync<TResult>(Func<IQueryable<AppDetail>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AppDetail>());
            return await queryable.LongCountAsync(cancellationToken!);
        }

        public async ValueTask DeleteAsync(AppDetail application, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Delete(application);
            await session.SaveChangesAsync(cancellationToken);
        }

        async ValueTask<AppDetail?> IOpenIddictApplicationStore<AppDetail>.FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.LoadAsync<AppDetail>(identifier, cancellationToken);
        }

        public async IAsyncEnumerable<AppDetail> FindByPostLogoutRedirectUriAsync([StringSyntax("Uri")] string uri, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<AppDetail>()
                .Where(app => app.PostLogoutRedirectUris.Contains(uri))
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<AppDetail> FindByRedirectUriAsync([StringSyntax("Uri")] string uri, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<AppDetail>()
                .Where(app => app.RedirectUris.Contains(uri))
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask<string?> GetApplicationTypeAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.ApplicationType);
        }

        public async ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<AppDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AppDetail>(), state);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public ValueTask<string?> GetClientIdAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.ClientId);
        }

        public ValueTask<string?> GetClientSecretAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.ClientSecret);
        }

        public ValueTask<string?> GetClientTypeAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.Type);
        }

        public ValueTask<string?> GetConsentTypeAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.ConsentType);
        }

        public ValueTask<string?> GetDisplayNameAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.DisplayName);
        }

        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.DisplayNames is null || application.DisplayNames.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<CultureInfo, string>.Empty);
            }
            var builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();
            foreach (var (culture, name) in application.DisplayNames)
            {
                builder.Add(culture, name);
            }
            return ValueTask.FromResult(builder.ToImmutable());
        }

        public ValueTask<string?> GetIdAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.Id)!;
        }

        public ValueTask<JsonWebKeySet?> GetJsonWebKeySetAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.JsonWebKeySetJson))
            {
                return ValueTask.FromResult<JsonWebKeySet?>(null);
            }
            var jwks = JsonWebKeySet.Create(application.JsonWebKeySetJson);

            return ValueTask.FromResult<JsonWebKeySet?>(jwks);
        }

        public ValueTask<ImmutableArray<string>> GetPermissionsAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.Permissions is null || application.Permissions.Count == 0)
            {
                return ValueTask.FromResult(ImmutableArray<string>.Empty);
            }

            return ValueTask.FromResult(application.Permissions.ToImmutableArray());
        }

        public ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(AppDetail application, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(application.PostLogoutRedirectUris.ToImmutableArray());
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.Properties is null || application.Properties.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<string, JsonElement>.Empty);
            }

            return ValueTask.FromResult(application.Properties.ToImmutableDictionary());
        }

        public ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.RedirectUris is null || application.RedirectUris.Count == 0)
            {
                return ValueTask.FromResult(ImmutableArray<string>.Empty);
            }

            return ValueTask.FromResult(application.RedirectUris.ToImmutableArray());
        }

        public ValueTask<ImmutableArray<string>> GetRequirementsAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.Requirements is null || application.Requirements.Count == 0)
            {
                return ValueTask.FromResult(ImmutableArray<string>.Empty);
            }

            return ValueTask.FromResult(application.Requirements.ToImmutableArray());
        }

        public ValueTask<ImmutableDictionary<string, string>> GetSettingsAsync(AppDetail application, CancellationToken cancellationToken)
        {
            if (application.Settings is null || application.Settings.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<string, string>.Empty);
            }

            return ValueTask.FromResult(application.Settings.ToImmutableDictionary());
        }

        public ValueTask<AppDetail> InstantiateAsync(CancellationToken cancellationToken)
        {
            var application = new AppDetail
            {
                Id = Guid.NewGuid().ToString()
            };

            return ValueTask.FromResult(application);
        }

        public async IAsyncEnumerable<AppDetail> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            IQueryable<AppDetail> query = session.Query<AppDetail>();

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

        public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<AppDetail>, TState, IQueryable<TResult>> query, TState state, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<AppDetail>(), state);
            var stream = queryable.ToAsyncEnumerable(cancellationToken);

            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask SetApplicationTypeAsync(AppDetail application, string? type, CancellationToken cancellationToken)
        {
            application.ApplicationType = type;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetClientIdAsync(AppDetail application, string? identifier, CancellationToken cancellationToken)
        {
            application.ClientId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetClientSecretAsync(AppDetail application, string? secret, CancellationToken cancellationToken)
        {
            application.ClientSecret = secret;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetClientTypeAsync(AppDetail application, string? type, CancellationToken cancellationToken)
        {
            application.Type = type;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetConsentTypeAsync(AppDetail application, string? type, CancellationToken cancellationToken)
        {
            application.ConsentType = type;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetDisplayNameAsync(AppDetail application, string? name, CancellationToken cancellationToken)
        {
            application.DisplayName = name;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetDisplayNamesAsync(AppDetail application, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
        {
            application.DisplayNames = names.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetJsonWebKeySetAsync(AppDetail application, JsonWebKeySet? set, CancellationToken cancellationToken)
        {
            application.JsonWebKeySetJson = set is not null
                    ? JsonSerializer.Serialize(set)
                    : null;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPermissionsAsync(AppDetail application, ImmutableArray<string> permissions, CancellationToken cancellationToken)
        {
            application.Permissions = permissions.IsDefault ? [] : [.. permissions];
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPostLogoutRedirectUrisAsync(AppDetail application, ImmutableArray<string> uris, CancellationToken cancellationToken)
        {
            application.PostLogoutRedirectUris = uris.IsDefault ? [] : [.. uris];
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPropertiesAsync(AppDetail application, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            application.Properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetRedirectUrisAsync(AppDetail application, ImmutableArray<string> uris, CancellationToken cancellationToken)
        {
            application.RedirectUris = uris.IsDefault ? [] : [.. uris];
            return ValueTask.CompletedTask;
        }

        public ValueTask SetRequirementsAsync(AppDetail application, ImmutableArray<string> requirements, CancellationToken cancellationToken)
        {
            application.Requirements = requirements.IsDefault ? [] : [.. requirements];
            return ValueTask.CompletedTask;
        }

        public ValueTask SetSettingsAsync(AppDetail application, ImmutableDictionary<string, string> settings, CancellationToken cancellationToken)
        {
            application.Settings = settings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public async ValueTask UpdateAsync(AppDetail application, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(application);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}