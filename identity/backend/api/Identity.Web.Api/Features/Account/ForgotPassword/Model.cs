using FastEndpoints;
using FluentValidation;

namespace Identity.Web.Api.Features.Account.ForgotPassword
{
    public sealed class Request
    {
        public required string UserId { get; set; }
    }


    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Must Provide User Id");
        }
    }
}