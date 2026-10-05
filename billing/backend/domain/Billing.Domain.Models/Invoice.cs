using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class Invoice
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [JsonPropertyName("contract_id")]
        public int ContractId { get; set; }

        [JsonPropertyName("status")]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        [JsonPropertyName("issue_date")]
        public DateOnly IssueDate { get; set; }

        [JsonPropertyName("due_date")]
        public DateOnly DueDate { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTime? PaidAt { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public enum InvoiceStatus
    {
        Draft,
        Issued,
        Paid,
        Void,
        Uncollectible
    }
}