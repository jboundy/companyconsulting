using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{

    //TODO: need to add tax type what the percentage actually does? Does it go to withholding for federal/state/something else etc
    // does the type have certain logic at the end? Is the tax applied to offset a cost?
    public class Tax
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("effective_start")]
        public DateOnly EffectiveStart { get; set; }

        [JsonPropertyName("effective_end")]
        public DateOnly EffectiveEnd { get; set; }

        [JsonPropertyName("percentage")]
        public float Percentage { get; set; }
    }
}