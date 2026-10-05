using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;

namespace OAuth.Server.Features.OAuth.Authorize
{
    public class Request
    {
        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }

        [JsonPropertyName("code_challenge")]
        public required string CodeChallenge { get; set; }

        [JsonPropertyName("code_challenge_method")]
        public required string CodeChallengeMethod { get; set; }

        [JsonPropertyName("redirect_uri")]
        public required string RedirectUri { get; set; }

        [JsonPropertyName("state")]
        public required string State { get; set; }
    }

    public class Response
    {
        public required string RedirectTarget { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ClientId).Empty().WithMessage("Client Id is required.");
            RuleFor(x => x.CodeChallenge).Empty().WithMessage("Code Challenge is required.");
            RuleFor(x => x.CodeChallengeMethod).Empty().WithMessage("Code Challenge Method is required.");
            RuleFor(x => x.RedirectUri).Empty().WithMessage("Redirect Uri is required.");
            RuleFor(x => x.State).Empty().WithMessage("State is required.");
        }
    }

    public class AuthCode
    {
        public string ClientId { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string RedirectUri { get; set; }
        public DateTime Expiry { get; set; }
    }

}