using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Database
{
    public class MySqlDbContext : AppDbContext
    {
        public MySqlDbContext(DbContextOptions options) : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            // In MySQL you might get an error when executing the migration SQLs.
            // In MySQL v5.6 and prior: "Specified key was too long; max key length is 767 bytes"
            // In MySQL v5.7 and later: "Specified key was too long; max key length is 3072 bytes"
            // The below lines fixes this problem by shortening the key length for ASP.NET Identity.
            // Note that it needs to be enabled before running the command for creating Entity
            // Framework migrations.

            var length = 128;

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(m => m.Id).HasMaxLength(length);
                entity.Property(m => m.Email).HasMaxLength(length);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(length);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(length);
                entity.Property(m => m.UserName).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(m => m.Id).HasMaxLength(length);
                entity.Property(m => m.Name).HasMaxLength(length);
                entity.Property(m => m.NormalizedName).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(length);
                entity.Property(m => m.ProviderKey).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(length);
                entity.Property(m => m.RoleId).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(length);
                entity.Property(m => m.LoginProvider).HasMaxLength(length);
                entity.Property(m => m.Name).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(length);
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(length);
                entity.Property(m => m.ProviderKey).HasMaxLength(length);
                entity.Property(m => m.UserId).HasMaxLength(length);
            });
        }
	}
}
