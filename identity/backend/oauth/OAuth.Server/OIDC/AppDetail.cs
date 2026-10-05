using System.Globalization;
using System.Text.Json;

namespace OAuth.Server.OIDC
{
    public class AppDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? DisplayName { get; set; }
        public List<string> Permissions { get; set; } = new();
        public List<string> RedirectUris { get; set; } = new();
        public List<string> PostLogoutRedirectUris { get; set; } = new();
        public string? Type { get; set; }
        public List<string> Requirements { get; set; } = new();
        public string? ApplicationType { get; set; }
        public string? ConsentType { get; internal set; }
        public Dictionary<CultureInfo, string> DisplayNames { get; set; } = new();
        public string? JsonWebKeySetJson { get; internal set; }
        public Dictionary<string, JsonElement> Properties { get; set; } = new();
        public Dictionary<string, string> Settings { get; set; } = new();
    }
}