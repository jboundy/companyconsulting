using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OAuth.Server.OIDC
{
    public class TokenDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ClientId { get; set; }
        public string? ApplicationId { get; set; }
        public string? AuthorizationId { get; set; }
        public string? Subject { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Payload { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public string? ReferenceId { get; internal set; }
        public DateTimeOffset? RedemptionDate { get; internal set; }
        public Dictionary<string, JsonElement> Properties { get; internal set; }
    }
}