using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billing.Domain.ServiceWrapper.ContractWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Contracts.Create
{
    public sealed class Handler(IContractService service) : IWolverineHandler
    {
        IContractService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.CreateContractAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}