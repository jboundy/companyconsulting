using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.Web.Api.Configuration
{
    public class JwtSettings
    {
        public required string Authority { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }

    }
}