using System.Text.Json.Serialization;
using Billing.Domain.Models;
using Billing.Domain.ServiceWrapper;
using FastEndpoints;

namespace Billing.Web.Api.Features.LineItems.UpdateLineItem
{
    public class Request
    {
        public required LineItemUpdateDTO LineItemUpdate { get; set; }
    }

    public class Response
    {
        public required OperationResult Result { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            // RuleFor(x => x.LineItemUpdate.InvoiceId).Equals(0).WithMessage("Collection contains invalid invoice id.");
            // RuleFor(x => x.LineItemUpdate.Quantity == 0).WithMessage("Collection contains invalid quantity.");
            // RuleFor(x => x.LineItemUpdate.ItemPrice == 0).WithMessage("Collection contains invalid item price.");
        }
    }

    public class LineItemUpdateDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("item_price")]
        public decimal ItemPrice { get; set; }
    }

    public class UpdateLineItemMapper : RequestMapper<Request, LineItem>
    {
        public override LineItem ToEntity(Request r)
        {
            return new LineItem
            {
                Id = r.LineItemUpdate.Id,
                InvoiceId = r.LineItemUpdate.InvoiceId,
                Description = r.LineItemUpdate.Description,
                Quantity = r.LineItemUpdate.Quantity,
                ItemPrice = r.LineItemUpdate.ItemPrice
            };
        }
    }
}