using Changelog.Data.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace Changelog.Data;

public class SeedTestData
{
    public static async Task Seed(IServiceProvider services)
    {
        var context = services.GetService<ApplicationDbContext>();

        var firstRunConfig = services.GetService<IOptions<FirstRunOptions>>();
        if (firstRunConfig == null) return;

        await CreateAdminUser(context, firstRunConfig.Value.AdminEmail, firstRunConfig.Value.AdminPassword);

        if (firstRunConfig.Value.SeedWithTestDataBool)
        {
            await CreateCategories(context);
            await CreateProjects(context);
            await CreateReleases(context);
            await CreateChanges(context);
        }
    }

    private static async Task CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminPassword)
    {
        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            return;

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
            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(), // This is used as the "Email input" when logging in. Must be upper case
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = true, // Will teporarily lock the account if the wrong password is given too many times
            };

            user.PasswordHash = hasher.HashPassword(user, adminPassword);

            var userStore = new UserStore<ApplicationUser>(context);
            await userStore.CreateAsync(user);

            await userStore.AddToRoleAsync(user, "admin");
        }

        await context.SaveChangesAsync();
    }

    private static EntityEntry<Category> CategoryNew, CategoryImproved, CategoryFixed;
    private static async Task CreateCategories(ApplicationDbContext context)
    {
        if (!context.Categories.Any())
        {
            CategoryNew = await context.Categories.AddAsync(new Category
            {
                Name = "New",
                BackgroundColor = "#00ca00",
                TextColor = "white",
            });

            CategoryImproved = await context.Categories.AddAsync(new Category
            {
                Name = "Improved",
                BackgroundColor = "#9393ff",
                TextColor = "white",
            });

            CategoryFixed = await context.Categories.AddAsync(new Category
            {
                Name = "Fixed",
                BackgroundColor = "#ea00ea",
                TextColor = "white",
            });
        }

        await context.SaveChangesAsync();
    }

    private static EntityEntry<Project> Project1, Project2, Project3, Project4, Project5, Project6, ProjectHidden;
    private static async Task CreateProjects(ApplicationDbContext context)
    {
        if (!context.Projects.Any())
        {
            Project1 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 1",
                Description = "The first test project",
                SortOrder = 1,
            });

            Project2 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 2",
                Description = "The second test project",
                SortOrder = 2,
            });

            Project3 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 3",
            });

            Project4 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 4",
            });

            Project5 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 5",
            });

            Project6 = await context.Projects.AddAsync(new Project
            {
                Name = "Project 6",
            });

            ProjectHidden = await context.Projects.AddAsync(new Project
            {
                Name = "Hidden project",
                Hidden = true,
            });
        }

        await context.SaveChangesAsync();
    }

    private static EntityEntry<Release> Release1Project1, Release2Project1, Release3Project1, Release1Project2;

    private static async Task CreateReleases(ApplicationDbContext context)
    {
        if (!context.Releases.Any())
        {
            Release1Project1 = await context.Releases.AddAsync(new Release
            {
                ProjectId = Project1.Entity.Id,
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

            Release2Project1 = await context.Releases.AddAsync(new Release
            {
                ProjectId = Project1.Entity.Id,
                ShortDescription = "Quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                LongDescriptionMarkdown = "Sed ut *perspiciatis* unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto **beatae vitae dicta sunt** (!) explicabo 😃 Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt 👏👏👏",
                Major = 1,
                Minor = 0,
                Patch = 1,
                ReleaseYear = 2021,
                ReleaseMonth = 11,
                ReleaseDay = 14,
            });

            Release3Project1 = await context.Releases.AddAsync(new Release
            {
                ProjectId = Project1.Entity.Id,
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

            Release1Project2 = await context.Releases.AddAsync(new Release
            {
                ProjectId = Project2.Entity.Id,
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
            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryFixed.Entity.Id,
                ReleaseId = Release1Project1.Entity.Id,
                Title = "Squashes bugs",
                Markdown = "**bold text**"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryNew.Entity.Id,
                ReleaseId = Release1Project1.Entity.Id,
                Title = "Added changelog",
                Markdown = "Markdown here, *italic*, **bold**\n\n1. List item 1\n2. List item 2\n\n![The San Juan Mountains are beautiful!](https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Whatsapp_chatting_outdoor_20180808.jpg/320px-Whatsapp_chatting_outdoor_20180808.jpg)"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryNew.Entity.Id,
                ReleaseId = Release1Project1.Entity.Id,
                Title = "Added tests",
                Markdown = "*italic text*"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryFixed.Entity.Id,
                ReleaseId = Release2Project1.Entity.Id,
                Title = "Fixed bug"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryImproved.Entity.Id,
                ReleaseId = Release2Project1.Entity.Id,
                Title = "Improved changelog"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryImproved.Entity.Id,
                ReleaseId = Release3Project1.Entity.Id,
                Title = "Easier login"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryFixed.Entity.Id,
                ReleaseId = Release3Project1.Entity.Id,
                Title = "Remove signout bug"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryImproved.Entity.Id,
                ReleaseId = Release3Project1.Entity.Id,
                Title = "Can directly click on button"
            });

            await context.Changes.AddAsync(new Change
            {
                CategoryId = CategoryNew.Entity.Id,
                ReleaseId = Release1Project2.Entity.Id,
                Title = "Init"
            });
        }

        await context.SaveChangesAsync();
    }
}
