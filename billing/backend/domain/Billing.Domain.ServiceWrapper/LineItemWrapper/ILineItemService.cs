using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper
{
    public interface ILineItemService
    {
        Task<OperationResult> CreateLineItemsAsync(List<LineItem> entities, CancellationToken ct);
        Task<OperationResult> UpdateLineItemAsync(LineItem entity, CancellationToken ct);
    }
}