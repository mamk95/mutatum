namespace Changelog.Data.Options;

using System.Globalization;

public class AccountRateLimitProtectionOptions
{
    /// <summary>
    /// The name of the section to read in appsettings.json.
    /// </summary>
    public const string AppsettingsSectionName = "Protection:AccountRateLimit";

    /// <summary>
    /// Gets or sets whether to enable account rate limit protection.
    /// </summary>
    [Obsolete("Use EnabledBool instead")]
    public string Enabled { get; set; }

    /// <summary>
    /// Gets or sets the amount of minutes without a successful login, failed login, or account registration before
    /// the count is cleared and reset to zero. Each of the three measurements (successlful logins, failed logins,
    /// and account registrations) have their own expiration which is not affected by the other two.
    /// </summary>
    [Obsolete("Use ExpirationInMinutesInt instead")]
    public string ExpirationInMinutes { get; set; }

    /// <summary>
    /// Gets or sets how many successful logins (before count expires) are needed before the
    /// threat level isincreased 1 level.
    /// </summary>
    [Obsolete("Use RecentSuccessfulLoginsLevel1BumpInt instead")]
    public string RecentSuccessfulLoginsLevel1Bump { get; set; }

    /// <summary>
    /// Gets or sets how many failed logins (before count expires) are needed before the
    /// threat level is increased 1 level.
    /// </summary>
    [Obsolete("Use RecentFailedLoginsLevel1BumpInt instead")]
    public string RecentFailedLoginsLevel1Bump { get; set; }

    /// <summary>
    /// Gets or sets how many failed logins (before count expires) are needed before the
    /// threat level is increased 2 levels.
    /// </summary>
    [Obsolete("Use RecentFailedLoginsLevel2BumpInt instead")]
    public string RecentFailedLoginsLevel2Bump { get; set; }

    /// <summary>
    /// Gets or sets how many account registrations (before count expires) are needed before
    /// the threat level is increased 1 level.
    /// </summary>
    [Obsolete("Use RecentAccountRegistrationsLevel1BumpInt instead")]
    public string RecentAccountRegistrationsLevel1Bump { get; set; }

    /// <summary>
    /// Gets or sets how many account registrations (before count expires) are needed before
    /// the threat level is increased 2 levels.
    /// </summary>
    [Obsolete("Use RecentAccountRegistrationsLevel2BumpInt instead")]
    public string RecentAccountRegistrationsLevel2Bump { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete

    /// <inheritdoc cref="Enabled"/>
    public bool EnabledBool => Enabled.ToLower(CultureInfo.InvariantCulture) == "true" || Enabled == "1";

    /// <inheritdoc cref="ExpirationInMinutes"/>
    public int ExpirationInMinutesInt
    {
        get
        {
            if (int.TryParse(ExpirationInMinutes, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 10;
        }
    }

    /// <inheritdoc cref="RecentSuccessfulLoginsLevel1Bump"/>
    public int RecentSuccessfulLoginsLevel1BumpInt
    {
        get
        {
            if (int.TryParse(RecentSuccessfulLoginsLevel1Bump, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 20;
        }
    }

    /// <inheritdoc cref="RecentFailedLoginsLevel1Bump"/>
    public int RecentFailedLoginsLevel1BumpInt
    {
        get
        {
            if (int.TryParse(RecentFailedLoginsLevel1Bump, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 7;
        }
    }

    /// <inheritdoc cref="RecentFailedLoginsLevel2Bump"/>
    public int RecentFailedLoginsLevel2BumpInt
    {
        get
        {
            if (int.TryParse(RecentFailedLoginsLevel2Bump, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 15;
        }
    }

    /// <inheritdoc cref="RecentAccountRegistrationsLevel1Bump"/>
    public int RecentAccountRegistrationsLevel1BumpInt
    {
        get
        {
            if (int.TryParse(RecentAccountRegistrationsLevel1Bump, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 4;
        }
    }

    /// <inheritdoc cref="RecentAccountRegistrationsLevel2Bump"/>
    public int RecentAccountRegistrationsLevel2BumpInt
    {
        get
        {
            if (int.TryParse(RecentAccountRegistrationsLevel2Bump, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
                return number;
            else
                return 8;
        }
    }

#pragma warning restore CS0618 // Type or member is obsolete
}
