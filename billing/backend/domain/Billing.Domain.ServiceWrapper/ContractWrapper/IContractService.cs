using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.ContractWrapper
{
    public interface IContractService
    {
        Task<OperationResult> CreateContractAsync(Contract entity, CancellationToken ct);
    }
}