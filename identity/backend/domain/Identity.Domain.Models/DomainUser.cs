using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Models
{
    public class DomainUser : IdentityUser, IClaimsUser
    {
        public bool IsActive { get; set; } = true;
        public IList<string> RoleClaims { get; set; } = [];
        public List<string> Roles { get; set; } = [];
    }
}