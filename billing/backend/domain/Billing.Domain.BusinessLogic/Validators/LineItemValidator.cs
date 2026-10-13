using Billing.Domain.Models;

namespace Billing.Domain.BusinessLogic.Validators
{
    public class LineItemValidator(LineItem lineItem)
    {
        private readonly LineItem _lineItem = lineItem;
        private readonly List<string> _errors = [];

        //TODO: need to validate that the invoice attached to the line item can be edited
        public (bool IsValid, IReadOnlyList<string> Errors) ValidateCreate()
        {
            ItemPriceGreaterThanZero()
            .QuantityGreaterThanZero()
            .InvoiceIdGreaterThanZero();
            return (_errors.Count == 0, _errors);
        }

        public (bool IsValid, IReadOnlyList<string> Errors) ValidateUpdate()
        {
            ItemPriceGreaterThanZero()
            .QuantityGreaterThanZero()
            .InvoiceIdGreaterThanZero()
            .LineItemIdIsNotZero();
            return (_errors.Count == 0, _errors);
        }

        private LineItemValidator ItemPriceGreaterThanZero()
        {
            if (_lineItem.ItemPrice == 0)
            {
                _errors.Add($"Item price for {_lineItem.Description} must be above 0.");
            }
            return this;
        }

        private LineItemValidator QuantityGreaterThanZero()
        {
            if (_lineItem.Quantity == 0)
            {
                _errors.Add($"Quantity for {_lineItem.Description} must be greater than 0.");
            }
            return this;
        }

        private LineItemValidator InvoiceIdGreaterThanZero()
        {
            if (_lineItem.InvoiceId == 0)
            {
                _errors.Add($"Invoice Id for for {_lineItem.Description} is invalid.");
            }
            return this;
        }

        private LineItemValidator LineItemIdIsNotZero()
        {
            if (_lineItem.Id == 0)
            {
                _errors.Add($"LineItem Id for for {_lineItem.Description} is invalid.");
            }
            return this;
        }

    }
}