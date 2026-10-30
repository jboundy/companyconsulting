using Marten;

namespace Product.Domain.DataAccess
{
    public class ProductContext<T>(IDocumentSession session) where T : class, IProductContext
    {

    }
}