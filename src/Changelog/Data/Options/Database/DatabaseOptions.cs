using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Options.Database
{
    public class DatabaseOptions
    {
        /// <summary>
        /// The name of the section to read in appsettings.json
        /// </summary>
        public const string AppsettingsSectionName = "Database";

        public string Provider { get; set; }

        public InMemoryOptions InMemory { get; set; }

        public MySQLOptions MySQL { get; set; }

        public MariaDbOptions MariaDB { get; set; }

        public MsSQLOptions MsSQL { get; set; }

        public PostgresOptions Postgres { get; set; }

        public SQLiteOptions SQLite{ get; set; }
    }
}
