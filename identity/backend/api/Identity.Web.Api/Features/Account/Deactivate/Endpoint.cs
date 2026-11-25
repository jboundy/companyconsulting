using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Api.Features.Account.Deactivate
{
    public class Endpoint(UserManager<DomainUser> userManager) : Endpoint<Request>
    {
        private readonly UserManager<DomainUser> _userManager = userManager;

        public override void Configure()
        {
            Post("account/deactivate");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var domainUser = await _userManager.FindByIdAsync(req.UserId);

            if (domainUser == null)
            {
                await Send.ResponseAsync("Unable to find user", 400, ct);
                return;
            }

            var lockoutResult = await _userManager.SetLockoutEnabledAsync(domainUser, true);
            var lockoutDateResult = await _userManager.SetLockoutEndDateAsync(domainUser, DateTimeOffset.MaxValue);

            if (!lockoutResult.Succeeded || !lockoutDateResult.Succeeded)
            {
                await Send.ResponseAsync("Unable to deactivate user.", 400, ct);
                return;
            }

            await Send.OkAsync(null, ct);
        }
    }
}