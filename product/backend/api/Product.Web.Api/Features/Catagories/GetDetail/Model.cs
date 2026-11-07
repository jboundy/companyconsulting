using Product.Domain.Models;
using FastEndpoints;

namespace Product.Web.Api.Features.Categories.GetDetail
{
    public sealed class Request
    {
        public int CategoryId { get; set; }
    }

    public sealed class Response
    {
        public required CategoryDetailDTO CategoryDetailDTO { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public sealed class CategoryDetailDTO
    {


    }
}