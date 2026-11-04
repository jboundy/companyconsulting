using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Billing.Domain.Taxes.Converters;

namespace Billing.Domain.Taxes.Models;

[JsonConverter(typeof(TaxCodeJsonConverter))]
public readonly partial record struct TaxCode
{
    public string Value { get; }

    public static readonly TaxCode DigitalSaaS = new("DIGITAL_SAAS");
    public static readonly TaxCode PhysicalGood = new("PHYSICAL_GOOD");
    public static readonly TaxCode ProfessionalService = new("PROFESSIONAL_SERVICE");
    public static readonly TaxCode TaxExempt = new("TAX_EXEMPT");

    [GeneratedRegex(@"^[A-Z0-9_]+$")]
    private static partial Regex RegexCheck();

    public TaxCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tax code cannot be null or whitespace.", nameof(value));
        }

        string cleaned = value.Trim().ToUpperInvariant();
        if (!RegexCheck().IsMatch(cleaned))
        {
            throw new ArgumentException($"Tax code '{value}' contains invalid characters. Use only letters, numbers, and underscores.", nameof(value));
        }

        Value = cleaned;
    }

    public static implicit operator string(TaxCode taxCode) => taxCode.Value;
    public static explicit operator TaxCode(string value) => new(value);
    public override string ToString() => Value;
}