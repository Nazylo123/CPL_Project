using Microsoft.AspNetCore.Identity;

namespace ShoesStore.IRepository
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
