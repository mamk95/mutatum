namespace Changelog.Data.Options
{
    public class FirstRunOptions
    {
        /// <summary>
        /// The name of the section to read in appsettings.json
        /// </summary>
        public const string AppsettingsSectionName = "FirstRun";

        /// <summary>
        /// Email of the admin user created if no other users exist in the database.
        /// Effectively, this means it only runs at the first execution of this project.
        /// Remember to use both <see cref="AdminEmail"/> and <see cref="AdminPassword"/>.
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Password of the admin user created if no other users exist in the database.
        /// Effectively, this means it only runs at the first execution of this project.
        /// Remember to use both <see cref="AdminEmail"/> and <see cref="AdminPassword"/>.
        /// </summary>
        public string AdminPassword { get; set; }

        /// <summary>
        /// If "true", and the database is empty, this will make sure some test data is
        /// written to the database. Use "true" or "1" to turn on seeding, or "false" or "0" to
        /// turn off seeding. Default is off.
        /// </summary>
        [Obsolete("Use SeedWithTestDataBool instead")]
        public string SeedWithTestData { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        public bool SeedWithTestDataBool => SeedWithTestData.ToLower() == "true" || SeedWithTestData == "1";
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
