using Billing.Domain.Models;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class ContractValidator(Contract contract)
    {
        private readonly Contract _contract = contract;
        private readonly List<string> _errors = [];
        public (bool IsValid, IReadOnlyList<string> Errors) Validate()
        {
            StartDateGreaterThanEndDate();
            return (_errors.Count == 0, _errors);
        }

        private ContractValidator StartDateGreaterThanEndDate()
        {
            if (_contract.StartDate > _contract.EndDate)
            {
                _errors.Add("Start Date cannot be before End Date.");
            }

            return this;
        }
    }
}