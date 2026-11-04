using System;
using System.Text.Json.Serialization;

namespace Billing.Domain.Taxes.Models;

public class TaxRule
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Product related tax code for reference
    /// </summary>
    public required string TaxCode { get; set; }

    /// <summary>
    /// Designated Country with the tax
    /// </summary>
    public required string CountryCode { get; set; }

    /// <summary>
    /// State code or null for all
    /// </summary>
    public string? StateOrRegion { get; set; }

    /// <summary>
    /// Descriptive code for tax
    /// <![CDATA[example: "55987" or "*" for all]]>
    /// </summary>
    [JsonPropertyName("postal_code_pattern")]
    public string? PostalCodePattern { get; set; }

    /// <summary>
    /// The designed tax rate
    /// <![CDATA[example: 0.06875m (6.875%)]]>
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    /// <summary>
    /// Readable name on the jurisdiction for the tax
    /// <![CDATA[Example: Minnesota State Tax]]>
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("effective_start")]
    public DateOnly EffectiveStart { get; set; }

    [JsonPropertyName("effective_end")]
    public DateOnly EffectiveEnd { get; set; }
}
