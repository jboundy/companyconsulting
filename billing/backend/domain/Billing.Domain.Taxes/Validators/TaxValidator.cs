using Billing.Domain.Taxes.Models;

namespace Billing.Domain.Taxes.Validators;

public class TaxValidator(TaxRule tax)
{
    private readonly TaxRule _tax = tax;
    private readonly List<string> _errors = [];

    public (bool IsValid, IReadOnlyList<string> Errors) ValidateCreate()
    {
        return (_errors.Count == 0, _errors);
    }
}