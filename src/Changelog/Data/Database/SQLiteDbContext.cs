namespace Changelog.Data.Database;

using Microsoft.EntityFrameworkCore;

public class SQLiteDbContext : AppDbContext
{
    public SQLiteDbContext(DbContextOptions options) : base(options)
    {
    }
}
