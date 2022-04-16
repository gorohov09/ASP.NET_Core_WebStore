
using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";

        public const string DefaultAdminPassword = "AdPAss_123";

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public DateTime? Birthday { get; set; }

        public string? AboutMySelf { get; set; }
    }
}
