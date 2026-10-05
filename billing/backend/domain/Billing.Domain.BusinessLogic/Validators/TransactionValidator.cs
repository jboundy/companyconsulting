using System.Transactions;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class TransactionValidator(Transaction transaction)
    {
        private readonly Transaction _transaction = transaction;
        private readonly List<string> _errors = [];

        public (bool IsValid, IReadOnlyList<string> Errors) Validate()
        {
            return (_errors.Count == 0, _errors);
        }
    }
}