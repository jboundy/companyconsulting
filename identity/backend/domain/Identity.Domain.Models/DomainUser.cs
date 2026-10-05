using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Models
{
    public class DomainUser : IdentityUser, IClaimsUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IList<string> RoleClaims { get; set; } = [];
        public List<string> Roles { get; set; } = new();
    }
}