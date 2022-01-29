using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Options.Database
{
    public class PostgresOptions
    {
        /// <summary>
        /// The connection string to connect to the Postgres database server and database.
        /// <para/>
        /// <example>
        /// Example:
        /// <code>
        /// Host=myserver;port=123;Username=mylogin;Password=mypass;Database=mydatabase;
        /// </code>
        /// </example>
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
