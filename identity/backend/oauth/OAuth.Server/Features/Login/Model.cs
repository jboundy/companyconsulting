using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;

namespace OAuth.Server.Features.Login
{
    public class Request
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ReturnUrl { get; set; }
    }

    public class Response
    {
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }
}