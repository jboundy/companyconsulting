using Billing.Domain.Models;
using Consulting.Encryption;
using Marten;

namespace Billing.Domain.DataAccess.Cache
{
    public class ClientData(IDocumentSession session, IBillingCacheService cacheService)
    {
        public async Task<Dictionary<string, Client>> GetClientTaxData()
        {
            var taxCache = cacheService.Get<Dictionary<string, Client>>(CacheKeys.ClientTax);
            if (taxCache?.Count > 0) return taxCache;
            var newCache = new Dictionary<string, Client>();
            var clients = await session.Query<Client>().ToListAsync();

            clients.AsParallel().ForAll((client) =>
            {
                var decryptedTaxId = EncryptionWrapper.Decrypt(client.TaxId);
                newCache.TryAdd(decryptedTaxId, client);
            });

            return cacheService.Create(CacheKeys.ClientTax, newCache, new TimeSpan(2, 0, 0));
        }

        public async Task AddClientToCache(Client client)
        {
            var taxCache = cacheService.Get<Dictionary<string, Client>>(CacheKeys.ClientTax);
            if (taxCache.IsEmpty())
            {
                await GetClientTaxData();
            }
            var taxId = EncryptionWrapper.Decrypt(client.TaxId);
            if (taxCache?.ContainsKey(taxId) == true) return;
            taxCache?.Add(taxId, client);
            cacheService.Create(CacheKeys.ClientTax, taxCache, new TimeSpan(2, 0, 0));
        }
    }
}