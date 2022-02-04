using Microsoft.EntityFrameworkCore;

namespace Changelog.Data.Database
{
    public class InMemoryDbContext : AppDbContext
    {
        public InMemoryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
