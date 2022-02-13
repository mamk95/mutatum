namespace Changelog.Data.Database;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class MsSqlDbContext : AppDbContext
{
    public MsSqlDbContext(DbContextOptions options) : base(options)
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "Reduces readability")]
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /*
         * In MsSQL you might get an error when executing the migration SQLs.
         * >>>> Warning! The maximum key length for a clustered index is 900 bytes. The
         * >>>> index 'PK_AspNetUserRoles' has maximum length of 1800 bytes. For some combination
         * >>>> of large values, the insert/update operation will fail.
         * The below lines fixes this problem by shortening the key length for ASP.NET Identity.
         * Note that it needs to be enabled before running the command for creating Entity
         * Framework migrations.
        */

        const int length = 128;

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(m => m.Id).HasMaxLength(length);
            entity.Property(m => m.Email).HasMaxLength(length);
            entity.Property(m => m.NormalizedEmail).HasMaxLength(length);
            entity.Property(m => m.NormalizedUserName).HasMaxLength(length);
            entity.Property(m => m.UserName).HasMaxLength(length);
        });
        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(m => m.Id).HasMaxLength(length);
            entity.Property(m => m.Name).HasMaxLength(length);
            entity.Property(m => m.NormalizedName).HasMaxLength(length);
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.Property(m => m.LoginProvider).HasMaxLength(length);
            entity.Property(m => m.ProviderKey).HasMaxLength(length);
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.Property(m => m.UserId).HasMaxLength(length);
            entity.Property(m => m.RoleId).HasMaxLength(length);
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.Property(m => m.UserId).HasMaxLength(length);
            entity.Property(m => m.LoginProvider).HasMaxLength(length);
            entity.Property(m => m.Name).HasMaxLength(length);
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.Property(m => m.UserId).HasMaxLength(length);
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.Property(m => m.LoginProvider).HasMaxLength(length);
            entity.Property(m => m.ProviderKey).HasMaxLength(length);
            entity.Property(m => m.UserId).HasMaxLength(length);
        });
    }
}
