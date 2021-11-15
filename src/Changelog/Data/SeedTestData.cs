using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Changelog.Data;

public class SeedTestData
{
    public static async Task Seed(IServiceProvider services)
    {
        var context = services.GetService<ApplicationDbContext>();

        await CreateAdminUser(context);
        await CreateChangeTypes(context);
        await CreateProjects(context);
        await CreateReleases(context);
        await CreateChanges(context);
    }

    private static async Task CreateAdminUser(ApplicationDbContext context)
    {
        // any unique string id
        const string ADMIN_ID = "5d1c46d4-79f1-4972-906d-b4d1140ca64a";
        const string ROLE_ID = "67f34fe4-22b8-4a4f-a641-8aa13a81ad1f";

        var roleStore = new RoleStore<IdentityRole>(context);

        if (!context.Roles.Any(r => r.Name == "admin"))
        {
            await roleStore.CreateAsync(new IdentityRole()
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin"
            });
        }

        if (!context.Users.Any(u => u.UserName == "admin"))
        {
            var hasher = new PasswordHasher<IdentityUser>();
            var user = new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "admin@admin.com",
                NormalizedUserName = "admin@admin.com".ToUpper(), // This is used as the "Email input" when logging in. Must be upper case
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            user.PasswordHash = hasher.HashPassword(user, "Admin123!");

            var userStore = new UserStore<IdentityUser>(context);
            await userStore.CreateAsync(user);

            await userStore.AddToRoleAsync(user, "admin");
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateChangeTypes(ApplicationDbContext context)
    {
        if (!context.ChangeTypes.Any())
        {
            await context.ChangeTypes.AddAsync(new ChangeType
            {
                Id = 1,
                Name = "New",
                Color = "#00ca00",
            });

            await context.ChangeTypes.AddAsync(new ChangeType
            {
                Id = 2,
                Name = "Improved",
                Color = "#9393ff",
            });

            await context.ChangeTypes.AddAsync(new ChangeType
            {
                Id = 3,
                Name = "Fixed",
                Color = "#ea00ea",
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateProjects(ApplicationDbContext context)
    {
        if (!context.Projects.Any())
        {
            int id = 1;

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = "Project 1",
                Description = "The first test project",
                SortOrder = 1,
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = "Project 2",
                Description = "The second test project",
                SortOrder = 2,
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = $"Project {id - 1}",
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = $"Project {id - 1}",
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = $"Project {id - 1}",
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = $"Project {id - 1}",
            });

            await context.Projects.AddAsync(new Project
            {
                Id = id++,
                Name = "Hidden project",
                Hidden = true,
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateReleases(ApplicationDbContext context)
    {
        if (!context.Releases.Any())
        {
            await context.Releases.AddAsync(new Release
            {
                Id = 1,
                ProjectId = 1,
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

            await context.Releases.AddAsync(new Release
            {
                Id = 2,
                ProjectId = 1,
                ShortDescription = "Quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                LongDescriptionMarkdown = "Sed ut *perspiciatis* unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto **beatae vitae dicta sunt** (!) explicabo 😃 Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt 👏👏👏",
                Major = 1,
                Minor = 0,
                Patch = 1,
                ReleaseYear = 2021,
                ReleaseMonth = 12,
                ReleaseDay = 14,
            });

            await context.Releases.AddAsync(new Release
            {
                Id = 3,
                ProjectId = 1,
                Title = "Added users",
                ShortDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                LongDescriptionMarkdown = "*Markdown* **here**",
                Major = 1,
                Minor = 1,
                Patch = 0,
                ReleaseYear = 2021,
                ReleaseMonth = 11,
                ReleaseDay = 23,
            });

            await context.Releases.AddAsync(new Release
            {
                Id = 4,
                ProjectId = 2,
                Title = "Init",
                Major = 0,
                Minor = 0,
                Patch = 1,
                ReleaseYear = 2021,
                ReleaseMonth = 1,
                ReleaseDay = 2,
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateChanges(ApplicationDbContext context)
    {
        if (!context.Changes.Any())
        {
            var id = 1;

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 3,
                ReleaseId = 1,
                Title = "Squashes bugs"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 1,
                ReleaseId = 1,
                Title = "Added changelog",
                Markdown = "Markdown here, *italic*, **bold**\n\n1. List item 1\n2. List item 2\n\n![The San Juan Mountains are beautiful!](https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Whatsapp_chatting_outdoor_20180808.jpg/320px-Whatsapp_chatting_outdoor_20180808.jpg)"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 1,
                ReleaseId = 1,
                Title = "Added tests"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 3,
                ReleaseId = 2,
                Title = "Fixed bug"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 2,
                ReleaseId = 2,
                Title = "Improved changelog"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 3,
                ReleaseId = 3,
                Title = "Added users"
            });

            await context.Changes.AddAsync(new Change
            {
                Id = id++,
                ChangeTypeId = 1,
                ReleaseId = 4,
                Title = "Init"
            });
        }

        await context.SaveChangesAsync();
    }
}
