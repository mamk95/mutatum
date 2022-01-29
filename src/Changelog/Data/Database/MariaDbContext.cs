using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Database
{
    public class MariaDbContext : AppDbContext
    {
        public MariaDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
