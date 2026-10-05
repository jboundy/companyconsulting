using System.Globalization;
using System.Text.Json;

namespace OAuth.Server.OIDC
{
    public class ScopeDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public List<string> Resources { get; set; } = new();
        public string? Description { get; set; }
        public Dictionary<CultureInfo, string> Descriptions { get; set; } = new();
        public Dictionary<CultureInfo, string> DisplayNames { get; set; } = new();
        public Dictionary<string, JsonElement> Properties { get; set; } = new();
    }
}