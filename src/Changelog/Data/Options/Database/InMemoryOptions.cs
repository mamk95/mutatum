using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Changelog.Data.Options.Database
{
    public class InMemoryOptions
    {
        /// <summary>
        /// Totally optional name for the in-memory database. The name does
        /// not matter at all for Mutatum's sake.
        /// <para/>
        /// <example>
        /// Example:
        /// <code>
        /// Changelog
        /// </code>
        /// </example>
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
