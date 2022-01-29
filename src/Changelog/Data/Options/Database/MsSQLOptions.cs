using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Options.Database
{
    public class MsSQLOptions
    {
        /// <summary>
        /// The connection string to connect to the Microsoft SQL database server and database.
        /// <para/>
        /// <example>
        /// Example:
        /// <code>
        /// Server=tcp:localhost,1433;Initial Catalog=databaseName;Persist Security Info=False;User ID=root;Password=1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
        /// </code>
        /// </example>
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
