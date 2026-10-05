using Billing.Domain.Models;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class TaxValidator(Tax tax)
    {
        private readonly Tax _tax = tax;
        private readonly List<string> _errors = [];

        public (bool IsValid, IReadOnlyList<string> Errors) Validate()
        {
            return (_errors.Count == 0, _errors);
        }
    }
}