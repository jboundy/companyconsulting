using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.ClientWrapper
{
    public interface IClientService
    {
        Task<OperationResult> CreateClientAsync(Client entity, CancellationToken ct);
        Task<OperationResult> UpdateClientAsync(Client entity, CancellationToken ct);
    }
}