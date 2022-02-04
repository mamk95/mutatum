using Microsoft.EntityFrameworkCore;

namespace Changelog.Data.Database
{
    public class MariaDbContext : AppDbContext
    {
        public MariaDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
