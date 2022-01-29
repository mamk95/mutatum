using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Database
{
    public class InMemoryDbContext : AppDbContext
    {
        public InMemoryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
