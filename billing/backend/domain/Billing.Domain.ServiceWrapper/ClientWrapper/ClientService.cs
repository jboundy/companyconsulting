using Billing.Domain.BusinessLogic.Validators;
using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Consulting.Encryption;

namespace Billing.Domain.ServiceWrapper.ClientWrapper
{
    public sealed record ClientTaxInfo(string TaxId, int Id);

    public class ClientService(IBillingContext context) : IClientService
    {
        public async Task<OperationResult> CreateClientAsync(Client entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var taxClients = await BuildTaxIdToClientLookup(ct);
            var (isValid, errors) = new ClientValidator(entity, taxClients).ValidateCreate();
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

        public async Task<OperationResult> UpdateClientAsync(Client entity, CancellationToken ct)
        {
            var allErrors = new List<string>();
            var taxClients = await BuildTaxIdToClientLookup(ct);
            var (isValid, errors) = new ClientValidator(entity, taxClients).ValidateUpdate();
            if (!isValid)
            {
                allErrors.AddRange(errors.Select(e => $"Item '{entity.Name}': {e}"));
                return OperationResult.Failure(allErrors);
            }
            await context.Update(entity);
            await context.SaveChanges(ct);
            return OperationResult.Success();
        }

        private async Task<Dictionary<string, int>> BuildTaxIdToClientLookup(CancellationToken ct)
        {
            var clientData = await context.QueryAllRecords<Client, ClientTaxInfo>(
                    cl => new ClientTaxInfo(cl.TaxId, cl.Id),
                    ct);

            return clientData.ToDictionary(
                    x => EncryptionWrapper.Encrypt(x.TaxId),
                    x => x.Id
                );
        }
    }


}