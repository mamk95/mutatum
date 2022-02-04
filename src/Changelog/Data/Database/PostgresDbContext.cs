using Microsoft.EntityFrameworkCore;

namespace Changelog.Data.Database
{
    public class PostgresDbContext : AppDbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
