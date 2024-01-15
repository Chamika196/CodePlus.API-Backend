using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePlus.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "371d2ba9-3a30-4834-a3ae-1f8aca62ffd4";
            var writerRoleId = "95b9a1ae-2db2-474d-bb61-cff1e80573dd";

            //create reader and writer role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId 
                }
            };

            //Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //create an Admin user
            var adminUserId = "be77c671-dff6-4cac-84cf-5c8131f68f83";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codeplus.com",
                Email = "admin@codeplus.com",
                NormalizedEmail = "admin@codeplus.com",
                NormalizedUserName = "admin@codeplus.com"
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles to Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId= adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId= adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
