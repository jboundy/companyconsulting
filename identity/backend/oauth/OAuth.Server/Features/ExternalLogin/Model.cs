using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Server.Features.ExternalLogin
{
    public class Request
    {
        public required string Provider { get; set; }
    }
}