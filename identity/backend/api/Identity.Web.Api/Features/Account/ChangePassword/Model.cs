using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;

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

        }
    }
}