using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class LineItem
    {
        private decimal _quantity;
        private decimal _itemPrice;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("quantity")]
        public decimal Quantity
        {
            get => _quantity;
            set => _quantity = Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        [JsonPropertyName("item_price")]
        public decimal ItemPrice
        {
            get => _itemPrice;
            set => _itemPrice = Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        [JsonPropertyName("amount")]
        public decimal Amount => Math.Round(_quantity * _itemPrice, 2, MidpointRounding.AwayFromZero);
    }
}