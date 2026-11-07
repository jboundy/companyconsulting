using Product.Domain.Models;

namespace Product.Domain.BusinessLogic.Validators
{
    public class CategoryValidator(Category category)
    {
        private readonly Category _category = category;
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