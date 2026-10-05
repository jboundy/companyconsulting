using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class Transaction
    {
        private decimal _amount;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("transaction_type")]
        public TransactionType TransactionType { get; set; }

        [JsonPropertyName("action_date")]
        public DateTime ActionDate { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount
        {
            get => _amount;
            set => _amount = Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }
}