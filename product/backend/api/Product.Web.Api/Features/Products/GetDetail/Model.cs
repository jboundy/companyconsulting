using FastEndpoints;

namespace Product.Web.Api.Features.Products.GetDetail
{
    public sealed class Request
    {
        public int ProductId { get; set; }
    }

    public sealed class Response
    {
        public required ProductDetailDTO ProductDetailDTO { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public sealed class ProductDetailDTO
    {


    }
}