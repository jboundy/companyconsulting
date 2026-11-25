using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Identity.Domain.Models;
using Identity.Domain.ServiceWrapper;
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
            var errors = new List<string>();
            var response = new Response();
            if (domainUser != null)
            {
                errors.Add("User email already exists");
                response.Result = OperationResult.Failure(errors);
                await Send.ResponseAsync(response, 400, ct);
                return;
            }

            var newUser = Map.ToEntity(req);
            var result = await _userManager.CreateAsync(newUser, req.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(x => x.Description));
                response.Result = OperationResult.Failure(errors);
                await Send.ResponseAsync(response, 400, ct);
                return;
            }
            response.Result = OperationResult.Success();
            await Send.OkAsync(response, ct);
        }
    }
}