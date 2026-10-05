using System.Linq.Expressions;

namespace Billing.Domain.DataAccess
{
    public interface IBillingContext
    {
        void Create<T>(T entity);
        Task CreateMany<T>(ICollection<T> entities);
        Task<T?> GetById<T>(int id, CancellationToken ct);
        Task<IReadOnlyList<T>> GetRecordsByIds<T>(int[] ids, CancellationToken ct);
        Task<IReadOnlyList<T>> QueryAllRecords<T>(CancellationToken ct);
        Task<IReadOnlyList<TResult>> QueryAllRecords<T, TResult>(Expression<Func<T, TResult>> select, CancellationToken ct);
        Task<IReadOnlyList<TResult>> QueryRecords<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select, CancellationToken ct);
        Task<IReadOnlyList<TResult>> QueryRecords<T, TResult>(Expression<Func<T, TResult>> select, CancellationToken ct);

        Task<IReadOnlyList<(TOuter Outer, TInner? Inner)>> LeftJoinAsync<TOuter, TInner, TKey>(
            Expression<Func<TOuter, bool>> outerWhere,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            CancellationToken ct)
            where TOuter : class
            where TInner : class;

        Task<IReadOnlyList<(TOuter Outer, TInner1? Inner1, TInner2? Inner2)>> LeftJoinAsync<
             TOuter, TInner1, TInner2, TKey1, TKey2>(
             Expression<Func<TOuter, bool>> outerWhere,
             Expression<Func<TOuter, TKey1>> outer1KeySelector,
             Expression<Func<TInner1, TKey1>> inner1KeySelector,
             Expression<Func<TOuter, TKey2>> outer2KeySelector,
             Expression<Func<TInner2, TKey2>> inner2KeySelector,
             CancellationToken ct)
             where TOuter : class
             where TInner1 : class
             where TInner2 : class;

        Task Update<T>(T entity);
        Task Delete<T>(T entity);
        Task SaveChanges(CancellationToken ct);

    }
}