namespace Changelog.Data.Database;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<Release> Releases { get; set; }

    public DbSet<Change> Changes { get; set; }

    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}
