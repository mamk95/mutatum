namespace Changelog.Data.Options.Database;

public class SQLiteOptions
{
    /// <summary>
    /// Gets or sets the connection string to connect to the SQLite database file.
    /// <para/>
    /// <example>
    /// Example:
    /// <code>
    /// Filename=path/to/database/mutatum.sqlite
    /// </code>
    /// Example with Windows file path (remember to escape backslashes):
    /// <code>
    /// Filename=C:\\Users\\username\\Documents\\SQLite\\mutatum.sqlite
    /// </code>
    /// </example>
    /// </summary>
    public string ConnectionString { get; set; }
}
