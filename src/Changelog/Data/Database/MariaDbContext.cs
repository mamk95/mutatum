namespace Changelog.Data.Database;

using Microsoft.EntityFrameworkCore;

public class MariaDbContext : AppDbContext
{
    public MariaDbContext(DbContextOptions options) : base(options)
    {
    }
}
