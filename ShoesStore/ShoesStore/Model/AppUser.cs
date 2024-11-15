using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
