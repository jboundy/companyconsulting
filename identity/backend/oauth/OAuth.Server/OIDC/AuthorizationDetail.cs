using System.Text.Json;
using Marten.Schema;

namespace OAuth.Server.OIDC
{
    public class AuthorizationDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ApplicationId { get; set; }
        public string? Subject { get; set; }
        public string? ClientId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public List<string> Scopes { get; set; } = new();
        public DateTimeOffset? CreationDate { get; set; } = DateTimeOffset.UtcNow;
        public Dictionary<string, JsonElement> Properties { get; set; } = new();

        [Version]
        public Guid Version { get; set; }
    }
}