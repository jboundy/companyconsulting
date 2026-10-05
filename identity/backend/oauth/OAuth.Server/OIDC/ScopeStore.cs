using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Marten;
using OpenIddict.Abstractions;

namespace OAuth.Server.OIDC
{
    public class ScopeStore : IOpenIddictScopeStore<ScopeDetail>
    {
        private readonly IDocumentStore _store;

        public ScopeStore(IDocumentStore store)
        {
            _store = store;
        }

        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<ScopeDetail>().LongCountAsync(cancellationToken);
        }

        public async ValueTask<long> CountAsync<TResult>(Func<IQueryable<ScopeDetail>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<ScopeDetail>());
            return await queryable.LongCountAsync(cancellationToken!);
        }

        public async ValueTask CreateAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(scope);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DeleteAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Delete(scope);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask<ScopeDetail?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.LoadAsync<ScopeDetail>(identifier, cancellationToken);
        }

        public async ValueTask<ScopeDetail?> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            return await session.Query<ScopeDetail>()
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async IAsyncEnumerable<ScopeDetail> FindByNamesAsync(ImmutableArray<string> names, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (names.IsEmpty())
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<ScopeDetail>()
                .Where(app => names.Contains(app.Name))
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<ScopeDetail> FindByResourceAsync(string resource, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(resource))
            {
                yield break;
            }

            await using var session = _store.QuerySession();
            var query = session.Query<ScopeDetail>()
                .Where(app => app.Resources.Contains(resource))
                .ToAsyncEnumerable(cancellationToken);

            await foreach (var item in query.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public async ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<ScopeDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<ScopeDetail>(), state);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public ValueTask<string?> GetDescriptionAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(scope.Description);
        }

        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            if (scope.Descriptions is null || scope.Descriptions.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<CultureInfo, string>.Empty);
            }

            return ValueTask.FromResult(scope.Descriptions.ToImmutableDictionary());
        }

        public ValueTask<string?> GetDisplayNameAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(scope.DisplayName);
        }

        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            if (scope.DisplayNames is null || scope.DisplayNames.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<CultureInfo, string>.Empty);
            }

            return ValueTask.FromResult(scope.DisplayNames.ToImmutableDictionary());
        }

        public ValueTask<string?> GetIdAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(scope.Id)!;
        }

        public ValueTask<string?> GetNameAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(scope.Name);
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            if (scope.Properties is null || scope.Properties.Count == 0)
            {
                return ValueTask.FromResult(ImmutableDictionary<string, JsonElement>.Empty);
            }

            return ValueTask.FromResult(scope.Properties.ToImmutableDictionary());
        }

        public ValueTask<ImmutableArray<string>> GetResourcesAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            if (scope.Resources is null || scope.Resources.Count == 0)
            {
                return ValueTask.FromResult(ImmutableArray<string>.Empty);
            }

            return ValueTask.FromResult(scope.Resources.ToImmutableArray());
        }

        public ValueTask<ScopeDetail> InstantiateAsync(CancellationToken cancellationToken)
        {
            var scope = new ScopeDetail
            {
                Id = Guid.NewGuid().ToString()
            };

            return ValueTask.FromResult(scope);
        }

        public async IAsyncEnumerable<ScopeDetail> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            IQueryable<ScopeDetail> query = session.Query<ScopeDetail>();

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

        public async IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<ScopeDetail>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            await using var session = _store.QuerySession();
            var queryable = query(session.Query<ScopeDetail>(), state);
            var stream = queryable.ToAsyncEnumerable(cancellationToken);

            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask SetDescriptionAsync(ScopeDetail scope, string? description, CancellationToken cancellationToken)
        {
            scope.Description = description;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetDescriptionsAsync(ScopeDetail scope, ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
        {
            scope.Descriptions = descriptions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetDisplayNameAsync(ScopeDetail scope, string? name, CancellationToken cancellationToken)
        {
            scope.DisplayName = name;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetDisplayNamesAsync(ScopeDetail scope, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
        {
            scope.DisplayNames = names.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetNameAsync(ScopeDetail scope, string? name, CancellationToken cancellationToken)
        {
            scope.Name = name;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPropertiesAsync(ScopeDetail scope, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            scope.Properties = properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetResourcesAsync(ScopeDetail scope, ImmutableArray<string> resources, CancellationToken cancellationToken)
        {
            scope.Resources = resources.IsDefault ? [] : [.. resources];
            return ValueTask.CompletedTask;
        }

        public async ValueTask UpdateAsync(ScopeDetail scope, CancellationToken cancellationToken)
        {
            await using var session = _store.LightweightSession();
            session.Store(scope);
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}