using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class Contract
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("status")]
        public StatusCodes Status { get; set; }

        [JsonPropertyName("start_date")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateOnly EndDate { get; set; }

        [JsonPropertyName("renew")]
        public bool Renew { get; set; }
    }

    public enum StatusCodes
    {
        Draft,
        Active,
        Expired,
        Canceled
    }
}