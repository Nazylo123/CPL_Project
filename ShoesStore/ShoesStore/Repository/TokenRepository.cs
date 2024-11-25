using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ShoesStore.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoesStore.Repository

{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // tao ra 1 list claim de lu thong tin va vai tro cua user
            var claims = new List<Claim>
            {   new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };
            claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
