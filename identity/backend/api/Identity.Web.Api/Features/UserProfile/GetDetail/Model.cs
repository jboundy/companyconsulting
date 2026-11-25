using FastEndpoints;
using FluentValidation;
using Identity.Domain.Models;

namespace Identity.Web.Api.Features.UserProfile.GetDetail
{
    public sealed class Request
    {
        public required string UserId { get; set; }
    }

    public sealed class Response
    {

    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Must Provide User Id");
        }
    }

    public sealed class UserProfileDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DisplayName { get; set; }
        public Locale Locale { get; set; } = Locale.Default;
        public TimeZoneStruct TimeZone { get; set; } = TimeZoneStruct.Default;
        public required UserPreferences Preferences { get; set; }
    }

}