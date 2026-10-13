using Billing.Domain.BusinessLogic.Validators;
using Billing.Domain.DataAccess;
using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.ContractWrapper
{
    public class ContractService(IBillingContext context) : IContractService
    {
        public async Task<OperationResult> CreateContractAsync(Contract entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var (isValid, errors) = new ContractValidator(entity).ValidateCreate();
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
    }
}