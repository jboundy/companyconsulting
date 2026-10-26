using Billing.Domain.BusinessLogic.Validators;
using Billing.Domain.DataAccess;
using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.ProductWrapper
{
    public class ProductService(IBillingContext context) : IProductService
    {
        public async Task<OperationResult> CreateProductAsync(Product entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new ProductValidator(entity).ValidateCreate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Description}': {e}"));
            }

            if (allErrors.Count > 0)
            {
                return OperationResult.Failure(allErrors);
            }

            context.Create(entity);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateProductAsync(Product entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new ProductValidator(entity).ValidateUpdate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Description}': {e}"));
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