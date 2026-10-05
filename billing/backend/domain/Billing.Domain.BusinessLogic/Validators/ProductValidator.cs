using Billing.Domain.Models;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class ProductValidator(Product product)
    {
        private readonly Product _product = product;
        private readonly List<string> _errors = [];

        public (bool IsValid, IReadOnlyList<string> Errors) Validate()
        {
            return (_errors.Count == 0, _errors);
        }
    }
}