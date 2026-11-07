
using Product.Domain.Models;

namespace Product.Domain.ServiceWrapper.CategoryWrapper
{
    public interface ICategoryService
    {
        Task<OperationResult> CreateCategoryAsync(Category entity, CancellationToken ct);
        Task<OperationResult> UpdateCategoryAsync(Category entity, CancellationToken ct);
    }
}