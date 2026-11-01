using Product.Domain.Models;

namespace Product.Domain.BusinessLogic.Validators
{
    public class ProductValidator(ProductRecord product)
    {
        private readonly ProductRecord _product = product;
        private readonly List<string> _errors = [];

        public (bool IsValid, IReadOnlyList<string> Errors) ValidateCreate()
        {
            return (_errors.Count == 0, _errors);
        }

        public (bool IsValid, IReadOnlyList<string> Errors) ValidateUpdate()
        {
            return (_errors.Count == 0, _errors);
        }
    }
}