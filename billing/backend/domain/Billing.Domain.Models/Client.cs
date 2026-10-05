using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class Client
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Encrypted TaxId/FEIN/Federal Identification
        /// </summary>
        [JsonPropertyName("taxId")]
        public required string TaxId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public required string Email { get; set; }

        [JsonPropertyName("phone_number")]
        [Phone(ErrorMessage = "Phone Number format is invalid.")]
        public required string? PhoneNumber { get; set; }

        [JsonPropertyName("billing_address")]
        public required Address BillingAddress { get; set; }

        [JsonPropertyName("mailing_address")]
        public required Address? MailingAddress { get; set; }

        [JsonPropertyName("is_billing_mailing_match")]
        public bool IsBillingMailingMatch { get; set; }
    }
}