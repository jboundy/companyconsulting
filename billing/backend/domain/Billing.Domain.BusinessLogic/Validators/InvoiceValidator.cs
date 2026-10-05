using Billing.Domain.Models;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class InvoiceValidator(Invoice invoice)
    {
        private readonly Invoice _invoice = invoice;
        private readonly List<string> _errors = [];

        public (bool IsValid, IReadOnlyList<string> Errors) Validate()
        {
            return (_errors.Count == 0, _errors);
        }
    }
}