using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Api.Features.Account.ChangePassword
{
    public class Endpoint : Endpoint<Request>
    {
        private readonly UserManager<DomainUser> _userManager;

        public Endpoint(UserManager<DomainUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Post("account/changePassword");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var domainUser = await _userManager.FindByIdAsync(req.UserId);

            if (domainUser == null)
            {
                await Send.ResponseAsync("Unable to find user", 400, ct);
                return;
            }

            var result = await _userManager.ChangePasswordAsync(domainUser, req.CurrentPassword, req.NewPassword);

            if (!result.Succeeded)
            {
                await Send.ResponseAsync("Invalid password change.", 400, ct);
                return;
            }

            await Send.OkAsync(null, ct);
        }
    }
}