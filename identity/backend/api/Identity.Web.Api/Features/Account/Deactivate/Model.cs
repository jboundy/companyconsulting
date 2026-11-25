using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Web.Api.Features.Account.Deactivate
{
    public sealed class Request
    {
        public required string UserId { get; set; }
    }
}