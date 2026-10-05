using System.Linq.Expressions;
using Marten;

namespace Billing.Domain.DataAccess
{
    public class BillingContext<T>(IDocumentSession session) where T : class, IBillingContext
    {
        public void Create(T entity)
        {
            session.Insert(entity);
        }

        public async Task CreateMany(ICollection<T> entities)
        {
            session.InsertObjects(entities);
        }

        public async Task<T?> GetById(int id, CancellationToken ct)
        {
            return await session.LoadAsync<T>(id, ct);
        }

        public async Task<IReadOnlyList<T>> GetRecordsByIds(int[] ids, CancellationToken ct)
        {
            return await session.LoadManyAsync<T>(ids);
        }

        public async Task<IReadOnlyList<T>> QueryAllRecords(CancellationToken ct)
        {
            return await session.Query<T>()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TResult>> QueryAllRecords<T, TResult>(Expression<Func<T, TResult>> select, CancellationToken ct)
            where T : notnull
            where TResult : notnull
        {
            return await session.Query<T>()
                .Select(select)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<T>> QueryRecords(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
            return await session.Query<T>()
                .Where(predicate)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TResult>> QueryRecords<T, TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> select,
            CancellationToken ct)
            where T : notnull
            where TResult : notnull
        {
            return await session.Query<T>()
                .Where(predicate)
                .Select(select)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TResult>> QueryRecords<T, TResult>(
            Expression<Func<T, TResult>> select,
            CancellationToken ct)
            where T : notnull
            where TResult : notnull
        {
            return await session.Query<T>()
                .Select(select)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<(TOuter Outer, TInner? Inner)>> LeftJoinAsync<TOuter, TInner, TKey>(
            Expression<Func<TOuter, bool>> outerWhere,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            CancellationToken ct)
            where TOuter : class
            where TInner : class
        {
            var query = session.Query<TOuter>().Where(outerWhere);

            var joined = query.GroupJoin(
                session.Query<TInner>(),
                outerKeySelector,
                innerKeySelector,
                (outer, inners) => new { outer, inners }
            )
            .SelectMany(
                x => x.inners.DefaultIfEmpty(),
                (x, inner) => new ValueTuple<TOuter, TInner?>(x.outer, inner)
            );

            return await joined.ToListAsync(ct);
        }

        public async Task<IReadOnlyList<(TOuter Outer, TInner1? Inner1, TInner2? Inner2)>> LeftJoinAsync<
            TOuter, TInner1, TInner2, TKey1, TKey2>(
            Expression<Func<TOuter, bool>> outerWhere,
            Expression<Func<TOuter, TKey1>> outer1KeySelector,
            Expression<Func<TInner1, TKey1>> inner1KeySelector,
            Expression<Func<TOuter, TKey2>> outer2KeySelector,
            Expression<Func<TInner2, TKey2>> inner2KeySelector,
            CancellationToken ct)
            where TOuter : class
            where TInner1 : class
            where TInner2 : class
        {
            var query = session.Query<TOuter>().Where(outerWhere);

            var joined1 = query.GroupJoin(
                session.Query<TInner1>(),
                outer1KeySelector,
                inner1KeySelector,
                (outer, inner1Group) => new { outer, inner1Group }
            )
            .SelectMany(
                x => x.inner1Group.DefaultIfEmpty(),
                (x, inner1) => new { x.outer, inner1 }
            );

            var adaptedOuter2KeySelector = AdaptOuterKeySelector(outer2KeySelector, joined1);

            var joined2 = joined1.GroupJoin(
                session.Query<TInner2>(),
                adaptedOuter2KeySelector,
                inner2KeySelector,
                (x, inner2Group) => new { x.outer, x.inner1, inner2Group }
            )
            .SelectMany(
                x => x.inner2Group.DefaultIfEmpty(),
                (x, inner2) => new ValueTuple<TOuter, TInner1?, TInner2?>(x.outer, x.inner1, inner2)
            );

            return await joined2.ToListAsync(ct);
        }


        public async Task Update(T entity)
        {
            session.Update(entity);
        }

        public async Task Delete(T entity)
        {
            session.Delete(entity);
        }

        public async Task SaveChanges(CancellationToken ct)
        {
            await session.SaveChangesAsync(ct);
        }

        private static Expression<Func<TAnon, TKey>> AdaptOuterKeySelector<TAnon, TOuter, TKey>(
            Expression<Func<TOuter, TKey>> selector,
            IQueryable<TAnon> _)
        {
            var param = Expression.Parameter(typeof(TAnon), "x");
            var outerProp = Expression.Property(param, "outer");
            var body = new ParameterReplacer(selector.Parameters[0], outerProp).Visit(selector.Body);
            return Expression.Lambda<Func<TAnon, TKey>>(body, param);
        }
    }

    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _target;
        private readonly Expression _replacement;

        public ParameterReplacer(ParameterExpression target, Expression replacement)
        {
            _target = target;
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _target ? _replacement : base.VisitParameter(node);
        }
    }

}