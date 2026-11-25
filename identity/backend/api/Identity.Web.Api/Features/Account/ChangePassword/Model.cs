using FastEndpoints;
using FluentValidation;

namespace Identity.Web.Api.Features.Account.ChangePassword
{
    public sealed class Request
    {
        public required string UserId { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }


    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Must Provide User Id");
        }
    }
}