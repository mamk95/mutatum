using Microsoft.EntityFrameworkCore;

namespace Changelog.Data.Database
{
    public class SQLiteDbContext : AppDbContext
    {
        public SQLiteDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
