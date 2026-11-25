using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Identity.Domain.Models;
using Marten.Linq.CreatedAt;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Api.Features.Account.Create
{
    public class Endpoint(UserManager<DomainUser> userManager) : Endpoint<Request, Response, CreateDomainUserMapper>
    {
        private readonly UserManager<DomainUser> _userManager = userManager;

        public override void Configure()
        {
            Post("account/create");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var domainUser = await _userManager.FindByEmailAsync(req.Email);

            if (domainUser != null)
            {
                await Send.ResponseAsync("User email already exists", 400, ct);
                return;
            }
            var newUser = Map.ToEntity(req);
            var result = await _userManager.CreateAsync

            if (!result.Succeeded)
            {
                await Send.ResponseAsync("Invalid password change.", 400, ct);
                return;
            }

            await Send.OkAsync(null, ct);
        }
    }
}