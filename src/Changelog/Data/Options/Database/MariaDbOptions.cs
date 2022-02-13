namespace Changelog.Data.Options.Database;

public class MariaDbOptions
{
    /// <summary>
    /// Gets or sets the connection string to connect to the MariaDB
    /// database server and database.
    /// <para/>
    /// <example>
    /// Example:
    /// <code>
    /// server=localhost;port=3306;user=root;password=1234;database=name;
    /// </code>
    /// </example>
    /// </summary>
    public string ConnectionString { get; set; }
}
