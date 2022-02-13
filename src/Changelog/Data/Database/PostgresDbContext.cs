namespace Changelog.Data.Database;

using Microsoft.EntityFrameworkCore;

public class PostgresDbContext : AppDbContext
{
    public PostgresDbContext(DbContextOptions options) : base(options)
    {
    }
}
