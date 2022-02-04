using Changelog.Data;
using Changelog.Data.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Changelog
{
    public class AccountRateLimitProtection
    {
        private const string CacheKeySuccessfulLogins = "account.protection.successfullogins";
        private const string CacheKeyFailedLogins = "account.protection.failedlogins";
        private const string CacheKeyRegistrations = "account.protection.registrations";

        private readonly IMemoryCache _memoryCache;

        private readonly IOptions<AccountRateLimitProtectionOptions> _options;

        public AccountRateLimitProtection(IMemoryCache memoryCache, IOptions<AccountRateLimitProtectionOptions> options)
        {
            _memoryCache = memoryCache;
            _options = options;
        }

        public ThreatLevel CurrentThreatLevel
        {
            get
            {
                ThreatLevel level = ThreatLevel.Low;

                if (RecentSuccessfulLogins > _options.Value.RecentSuccessfulLoginsLevel1BumpInt)
                {
                    level = IncreaseThreatLevel(level, increaseBy: 1);
                }

                if (RecentFailedLogins > _options.Value.RecentFailedLoginsLevel2BumpInt)
                {
                    level = IncreaseThreatLevel(level, increaseBy: 2);
                }
                else if (RecentFailedLogins > _options.Value.RecentFailedLoginsLevel1BumpInt)
                {
                    level = IncreaseThreatLevel(level, increaseBy: 1);
                }

                if (RecentAccountRegistrations > _options.Value.RecentAccountRegistrationsLevel2BumpInt)
                {
                    level = IncreaseThreatLevel(level, increaseBy: 2);
                }
                else if (RecentAccountRegistrations > _options.Value.RecentAccountRegistrationsLevel1BumpInt)
                {
                    level = IncreaseThreatLevel(level, increaseBy: 1);
                }

                return level;
            }
        }

        public void AddSuccessfulLogin()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_options.Value.ExpirationInMinutesInt)); // If no new successful logins happen within x minutes, this value is cleared

            if (!_memoryCache.TryGetValue(CacheKeySuccessfulLogins, out int recentSuccessfulLogins))
            {
                recentSuccessfulLogins = 1;
                _memoryCache.Set(CacheKeySuccessfulLogins, recentSuccessfulLogins, cacheEntryOptions);
            }
            else
            {
                _memoryCache.Set(CacheKeySuccessfulLogins, recentSuccessfulLogins + 1, cacheEntryOptions);
            }
        }

        public void AddFailedLogin()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_options.Value.ExpirationInMinutesInt)); // If no new failed logins happen within x minutes, this value is cleared

            if (!_memoryCache.TryGetValue(CacheKeyFailedLogins, out int recentFailedLogins))
            {
                recentFailedLogins = 1;
                _memoryCache.Set(CacheKeyFailedLogins, recentFailedLogins, cacheEntryOptions);
            }
            else
            {
                _memoryCache.Set(CacheKeyFailedLogins, recentFailedLogins + 1, cacheEntryOptions);
            }
        }

        public void AddAccountRegistration()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_options.Value.ExpirationInMinutesInt)); // If no new registrations happen within x minutes, this value is cleared

            if (!_memoryCache.TryGetValue(CacheKeyRegistrations, out int recentRegistrations))
            {
                recentRegistrations = 1;
                _memoryCache.Set(CacheKeyRegistrations, recentRegistrations, cacheEntryOptions);
            }
            else
            {
                _memoryCache.Set(CacheKeyRegistrations, recentRegistrations + 1, cacheEntryOptions);
            }
        }

        private int RecentSuccessfulLogins
        {
            get
            {
                _memoryCache.TryGetValue(CacheKeySuccessfulLogins, out int recentSuccessfulLogins);
                return recentSuccessfulLogins;
            }
        }

        private int RecentFailedLogins
        {
            get
            {
                _memoryCache.TryGetValue(CacheKeyFailedLogins, out int recentFailedLogins);
                return recentFailedLogins;
            }
        }

        private int RecentAccountRegistrations
        {
            get
            {
                _memoryCache.TryGetValue(CacheKeyRegistrations, out int recentRegistrations);
                return recentRegistrations;
            }
        }

        private static ThreatLevel IncreaseThreatLevel(ThreatLevel level, int increaseBy = 1)
        {
            ThreatLevel newLevel = level + increaseBy;

            if (newLevel < ThreatLevel.Low) return ThreatLevel.Low;
            else if (newLevel > ThreatLevel.High) return ThreatLevel.High;
            else return newLevel;
        }
    }
}
