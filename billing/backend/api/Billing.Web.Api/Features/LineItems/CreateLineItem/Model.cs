using System.Text.Json.Serialization;
using Billing.Domain.Models;
using Billing.Domain.ServiceWrapper;
using FastEndpoints;
using FluentValidation;

namespace Billing.Web.Api.Features.LineItems.CreateLineItem
{
    public class Request
    {
        public List<LineItemDTO> LineItems { get; set; } = [];
    }

    public class Response
    {
        public required OperationResult Result { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.LineItems.Where(x => x.InvoiceId == 0)).Empty().WithMessage("Collection contains invalid invoice id.");
            RuleFor(x => x.LineItems.Where(x => x.Quantity == 0)).Empty().WithMessage("Collection contains invalid quantity.");
            RuleFor(x => x.LineItems.Where(x => x.ItemPrice == 0)).Empty().WithMessage("Collection contains invalid item price.");
        }
    }

    public class LineItemDTO
    {
        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("item_price")]
        public decimal ItemPrice { get; set; }
    }

    public class CreateLineItemMapper : RequestMapper<Request, List<LineItem>>
    {
        public override List<LineItem> ToEntity(Request r)
        {
            return r.LineItems.Select(dto => new LineItem
            {
                InvoiceId = dto.InvoiceId,
                Description = dto.Description,
                Quantity = dto.Quantity,
                ItemPrice = dto.ItemPrice
            }).ToList();
        }
    }
}