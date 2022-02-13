namespace Changelog.Data.Options.Database;

public class PostgresOptions
{
    /// <summary>
    /// Gets or sets the connection string to connect to the Postgres database server and database.
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
