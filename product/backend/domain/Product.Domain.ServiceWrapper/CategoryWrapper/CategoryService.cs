using Product.Domain.BusinessLogic.Validators;
using Product.Domain.DataAccess;
using Product.Domain.Models;

namespace Product.Domain.ServiceWrapper.CategoryWrapper
{
    public class CategoryService(IProductContext context) : ICategoryService
    {
        public async Task<OperationResult> CreateCategoryAsync(Category entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new CategoryValidator(entity).ValidateCreate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Name}': {e}"));
            }

            if (allErrors.Count > 0)
            {
                return OperationResult.Failure(allErrors);
            }

            context.Create(entity);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateCategoryAsync(Category entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new CategoryValidator(entity).ValidateUpdate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Name}': {e}"));
            }

            if (allErrors.Count > 0)
            {
                return OperationResult.Failure(allErrors);
            }

            await context.Update(entity);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }
    }
}