using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShoesStore.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder buider)
        {
            base.OnModelCreating(buider);

            var customerRoleId = "89fc7c43-b04b-49ed-8c74-365afd2996e1";
            var adminRoleId = "64fe4e7f-29b4-4b5a-8d1a-ba411d6dcc28";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = customerRoleId
                },
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                }
            };
            buider.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "224d8d3f-0fcb-4477-b4c9-14683a0378a7";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "Admin123",
                Email = "Admin@gmail.com",
                NormalizedEmail = "Admin@gmail.com".ToUpper(),
                NormalizedUserName = "ADMIN123"
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            buider.Entity<IdentityUser>().HasData(admin);

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = customerRoleId
                }
            };
            buider.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}