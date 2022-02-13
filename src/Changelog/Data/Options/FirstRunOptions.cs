namespace Changelog.Data.Options;

using System.Globalization;

public class FirstRunOptions
{
    /// <summary>
    /// The name of the section to read in appsettings.json.
    /// </summary>
    public const string AppsettingsSectionName = "FirstRun";

    /// <summary>
    /// Gets or sets the email of the admin user created if no other users exist in the database.
    /// Effectively, this means it only runs at the first execution of this project.
    /// Remember to use both <see cref="AdminEmail"/> and <see cref="AdminPassword"/>.
    /// </summary>
    public string AdminEmail { get; set; }

    /// <summary>
    /// Gets or sets the password of the admin user created if no other users exist in the database.
    /// Effectively, this means it only runs at the first execution of this project.
    /// Remember to use both <see cref="AdminEmail"/> and <see cref="AdminPassword"/>.
    /// </summary>
    public string AdminPassword { get; set; }

    /// <summary>
    /// Gets or sets whether to seed the database with test data.
    /// If "true", and the database is empty, this will make sure some test data is
    /// written to the database. Use "true" or "1" to turn on seeding, or "false" or "0" to
    /// turn off seeding. Default is off.
    /// </summary>
    [Obsolete("Use SeedWithTestDataBool instead")]
    public string SeedWithTestData { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete

    /// <inheritdoc cref="SeedWithTestData"/>
    public bool SeedWithTestDataBool => SeedWithTestData.ToLower(CultureInfo.InvariantCulture) == "true" || SeedWithTestData == "1";

#pragma warning restore CS0618 // Type or member is obsolete
}
