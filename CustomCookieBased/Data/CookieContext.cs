using CustomCookieBased.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CustomCookieBased.Data
{
    public class CookieContext : DbContext
    {
        public CookieContext(DbContextOptions<CookieContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserConfiguraiton());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserRoleConfiguration());

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }


    }
}
