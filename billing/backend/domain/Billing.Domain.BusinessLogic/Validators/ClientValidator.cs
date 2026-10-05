using Billing.Domain.Models;
using Consulting.Encryption;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class ClientValidator(Client client, Dictionary<string, int> taxClients)
    {
        private readonly Client _client = client;
        private readonly Dictionary<string, int> _taxClients = taxClients;
        private readonly List<string> _errors = [];
        private record AddressRecord(string AddressOne,
            string? AddressTwo,
            string City,
            string State,
            int Zip,
            int? ZipExtension
        );

        public (bool IsValid, IReadOnlyList<string> Errors) ValidateUpdate()
        {
            NoDuplicateTaxId()
            .AddressMatch();
            return (_errors.Count == 0, _errors);
        }

        public (bool IsValid, IReadOnlyList<string> Errors) ValidateCreate()
        {
            TaxIdMustBeUnique()
            .AddressMatch();
            return (_errors.Count == 0, _errors);
        }

        public ClientValidator TaxIdMustBeUnique()
        {
            var decryptedValue = EncryptionWrapper.Decrypt(_client.TaxId);
            if (_taxClients.ContainsKey(decryptedValue))
            {
                _errors.Add("Duplicate Tax Id found.");
            }
            return this;
        }

        public ClientValidator NoDuplicateTaxId()
        {
            var decryptedValue = EncryptionWrapper.Decrypt(_client.TaxId);
            var clientId = _taxClients.ContainsKey(decryptedValue) == true ? _taxClients[decryptedValue] : 0;
            if (_client.Id != clientId && clientId != 0)
            {
                _errors.Add("Cannot have duplicate tax id for a client.");
            }
            return this;
        }

        public ClientValidator AddressMatch()
        {
            if (_client.MailingAddress == null) return this;
            if (!_client.IsBillingMailingMatch) return this;

            var billingRecord = new AddressRecord(
                 _client.BillingAddress.AddressOne,
                 _client.BillingAddress.AddressTwo,
                 _client.BillingAddress.City,
                 _client.BillingAddress.State,
                 _client.BillingAddress.Zip,
                 _client.BillingAddress.ZipExtension
             );

            var mailingRecord = new AddressRecord(
                _client.MailingAddress.AddressOne,
                _client.MailingAddress.AddressTwo,
                _client.MailingAddress.City,
                _client.MailingAddress.State,
                _client.MailingAddress.Zip,
                _client.MailingAddress.ZipExtension
            );

            if (billingRecord != mailingRecord)
            {
                _errors.Add("Billing and mailing addresses do not match.");
            }

            return this;
        }
    }
}