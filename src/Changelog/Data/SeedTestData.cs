namespace Changelog.Data;
using System.Globalization;
using Changelog.Data.Database;
using Changelog.Data.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

public static class SeedTestData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        AppDbContext context = services.GetService<AppDbContext>();

        IOptions<FirstRunOptions> firstRunConfig = services.GetService<IOptions<FirstRunOptions>>();
        if (firstRunConfig == null) return;

        await CreateAdminUserAsync(context, firstRunConfig.Value.AdminEmail, firstRunConfig.Value.AdminPassword);

        if (firstRunConfig.Value.SeedWithTestDataBool)
        {
            CreateCategories(context);
            CreateProjects(context);
            CreateReleases(context);
            CreateChanges(context);
        }
    }

    private static async Task CreateAdminUserAsync(AppDbContext context, string adminEmail, string adminPassword)
    {
        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            return;

        if (!context.Users.Any())
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(CultureInfo.InvariantCulture), // This is used as the "Email input" when logging in. Must be upper case
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(CultureInfo.InvariantCulture),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = true, // Will teporarily lock the account if the wrong password is given too many times
            };

            user.PasswordHash = hasher.HashPassword(user, adminPassword);

            var userStore = new UserStore<ApplicationUser>(context);
            await userStore.CreateAsync(user);
        }

        context.SaveChanges();
    }

    private static EntityEntry<Category> _categoryNew;
    private static EntityEntry<Category> _categoryImproved;
    private static EntityEntry<Category> _categoryFixed;

    private static void CreateCategories(AppDbContext context)
    {
        if (!context.Categories.Any())
        {
            _categoryNew = context.Categories.Add(new Category
            {
                Name = "New",
                BackgroundColor = "#00ca00",
                TextColor = "white",
            });

            _categoryImproved = context.Categories.Add(new Category
            {
                Name = "Improved",
                BackgroundColor = "#9393ff",
                TextColor = "white",
            });

            _categoryFixed = context.Categories.Add(new Category
            {
                Name = "Fixed",
                BackgroundColor = "#ea00ea",
                TextColor = "white",
            });
        }

        context.SaveChanges();
    }

    private static EntityEntry<Project> _project1;
    private static EntityEntry<Project> _project2;

    private static void CreateProjects(AppDbContext context)
    {
        if (!context.Projects.Any())
        {
            _project1 = context.Projects.Add(new Project
            {
                Name = "Project 1",
                UrlSlug = "proj1",
                Description = "The first test project",
                SortOrder = 1,
            });

            _project2 = context.Projects.Add(new Project
            {
                Name = "Project 2",
                UrlSlug = "proj2",
                Description = "The second test project",
                SortOrder = 2,
            });

            context.Projects.Add(new Project
            {
                Name = "Project 3",
                UrlSlug = "cool-url",
            });

            context.Projects.Add(new Project
            {
                Name = "Project 4",
                UrlSlug = "proj4",
            });

            context.Projects.Add(new Project
            {
                Name = "Project 5",
                UrlSlug = "proj5",
            });

            context.Projects.Add(new Project
            {
                Name = "Project 6",
                UrlSlug = "proj6",
            });

            context.Projects.Add(new Project
            {
                Name = "Hidden project",
                UrlSlug = "hidden",
                Hidden = true,
            });
        }

        context.SaveChanges();
    }

    private static EntityEntry<Release> _release1Project1;
    private static EntityEntry<Release> _release2Project1;
    private static EntityEntry<Release> _release3Project1;
    private static EntityEntry<Release> _release1Project2;

    private static void CreateReleases(AppDbContext context)
    {
        if (!context.Releases.Any())
        {
            _release1Project1 = context.Releases.Add(new Release
            {
                ProjectId = _project1.Entity.Id,
                Title = "First release",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam.",
                LongDescriptionMarkdown = "*Markdown* **here**",
                Major = 1,
                Minor = 0,
                Patch = 0,
                ReleaseYear = 2021,
                ReleaseMonth = 11,
                ReleaseDay = 13,
            });

            _release2Project1 = context.Releases.Add(new Release
            {
                ProjectId = _project1.Entity.Id,
                ShortDescription = "Quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                LongDescriptionMarkdown = "Sed ut *perspiciatis* unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto **beatae vitae dicta sunt** (!) explicabo 😃 Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt 👏👏👏",
                Major = 1,
                Minor = 0,
                Patch = 1,
                ReleaseYear = 2021,
                ReleaseMonth = 11,
                ReleaseDay = 14,
            });

            _release3Project1 = context.Releases.Add(new Release
            {
                ProjectId = _project1.Entity.Id,
                Title = "Added users",
                ShortDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                LongDescriptionMarkdown = "*Markdown* **here**",
                Major = 1,
                Minor = 1,
                Patch = 0,
                ReleaseYear = 2021,
                ReleaseMonth = 12,
                ReleaseDay = 23,
            });

            _release1Project2 = context.Releases.Add(new Release
            {
                ProjectId = _project2.Entity.Id,
                Title = "Init",
                Major = 0,
                Minor = 0,
                Patch = 1,
                ReleaseYear = 2021,
                ReleaseMonth = 1,
                ReleaseDay = 2,
            });
        }

        context.SaveChanges();
    }

    private static void CreateChanges(AppDbContext context)
    {
        if (!context.Changes.Any())
        {
            context.Changes.Add(new Change
            {
                CategoryId = _categoryFixed.Entity.Id,
                ReleaseId = _release1Project1.Entity.Id,
                Title = "Squashes bugs",
                Markdown = "**bold text**",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryNew.Entity.Id,
                ReleaseId = _release1Project1.Entity.Id,
                Title = "Added changelog",
                Markdown = "Markdown here, *italic*, **bold**\n\n1. List item 1\n2. List item 2\n\n![The San Juan Mountains are beautiful!](https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Whatsapp_chatting_outdoor_20180808.jpg/320px-Whatsapp_chatting_outdoor_20180808.jpg)",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryNew.Entity.Id,
                ReleaseId = _release1Project1.Entity.Id,
                Title = "Added tests",
                Markdown = "*italic text*",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryFixed.Entity.Id,
                ReleaseId = _release2Project1.Entity.Id,
                Title = "Fixed bug",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryImproved.Entity.Id,
                ReleaseId = _release2Project1.Entity.Id,
                Title = "Improved changelog",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryImproved.Entity.Id,
                ReleaseId = _release3Project1.Entity.Id,
                Title = "Easier login",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryFixed.Entity.Id,
                ReleaseId = _release3Project1.Entity.Id,
                Title = "Remove signout bug",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryImproved.Entity.Id,
                ReleaseId = _release3Project1.Entity.Id,
                Title = "Can directly click on button",
            });

            context.Changes.Add(new Change
            {
                CategoryId = _categoryNew.Entity.Id,
                ReleaseId = _release1Project2.Entity.Id,
                Title = "Init",
            });
        }

        context.SaveChanges();
    }
}
