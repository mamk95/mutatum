namespace Changelog.Data.Database;

using Microsoft.EntityFrameworkCore;

public class InMemoryDbContext : AppDbContext
{
    public InMemoryDbContext(DbContextOptions options) : base(options)
    {
    }
}
