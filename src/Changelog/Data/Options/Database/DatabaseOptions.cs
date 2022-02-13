namespace Changelog.Data.Options.Database;

public class DatabaseOptions
{
    /// <summary>
    /// The name of the section to read in appsettings.json.
    /// </summary>
    public const string AppsettingsSectionName = "Database";

    public string Provider { get; set; }

    public InMemoryOptions InMemory { get; set; }

    public MySqlOptions MySQL { get; set; }

    public MariaDbOptions MariaDB { get; set; }

    public MsSqlOptions MsSQL { get; set; }

    public PostgresOptions Postgres { get; set; }

    public SQLiteOptions SQLite { get; set; }
}
