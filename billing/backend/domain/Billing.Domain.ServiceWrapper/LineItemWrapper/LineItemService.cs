using Billing.Domain.BusinessLogic.Validators;
using Billing.Domain.DataAccess;
using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper
{
    public class LineItemService(IBillingContext context) : ILineItemService
    {
        public async Task<OperationResult> CreateLineItemsAsync(List<LineItem> entities, CancellationToken ct)
        {
            var lineItems = new List<LineItem>();
            var allErrors = new List<string>();

            foreach (var item in entities)
            {
                var (isValid, errors) = new LineItemValidator(item).ValidateCreate();
                if (!isValid)
                {
                    allErrors.AddRange(errors.Select(e => $"Item '{item.Description}': {e}"));
                    continue;
                }

                lineItems.Add(item);
            }

            if (allErrors.Count > 0)
            {
                return OperationResult.Failure(allErrors);
            }

            await context.CreateMany(lineItems);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateLineItemAsync(LineItem entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new LineItemValidator(entity).ValidateUpdate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Description}': {e}"));
                return OperationResult.Failure(allErrors);
            }
            await context.Update(entity);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteLineItemAsync(int id, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var record = await context.GetById<LineItem>(id, ct);
            if (record == null)
            {
                allErrors.Add("Line Item does not exist.");
                return OperationResult.Failure(allErrors);
            }

            await context.Delete(record);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }
    }
}