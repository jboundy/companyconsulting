using System.Text.Json;
using System.Text.Json.Serialization;
using Billing.Domain.Taxes.Models;

namespace Billing.Domain.Taxes.Converters;

public class TaxCodeJsonConverter : JsonConverter<TaxCode>
{
    public override TaxCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        return string.IsNullOrEmpty(stringValue) ? throw new JsonException("Invalid tax code.") : new TaxCode(stringValue);
    }

    public override void Write(Utf8JsonWriter writer, TaxCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
