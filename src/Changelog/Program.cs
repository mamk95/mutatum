#pragma warning disable SA1200 // Using directives should be placed correctly. Reason: There is no namespace in Program.cs

using Changelog;
using Changelog.Areas.Identity;
using Changelog.Data;
using Changelog.Data.Database;
using Changelog.Data.Options;
using Changelog.Data.Options.Database;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

#pragma warning restore SA1200 // Using directives should be placed correctly

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

DatabaseOptions dbOptions = builder.Configuration.GetSection(DatabaseOptions.AppsettingsSectionName).Get<DatabaseOptions>();

if (dbOptions.Provider == "InMemory")
{
    builder.Services.AddDbContext<AppDbContext, InMemoryDbContext>(options =>
    {
        options.UseInMemoryDatabase(dbOptions.InMemory.DatabaseName);
#if DEBUG
        options.EnableSensitiveDataLogging();
#endif
    });
}
else if (dbOptions.Provider == "MySQL")
{
    var serverVersion = ServerVersion.AutoDetect(dbOptions.MySQL.ConnectionString);
    if (serverVersion.Type == ServerType.MySql && serverVersion.Version.Major < 8)
        throw new NotSupportedException($"Only MySQL v8.0.0 or newer is supported. Please see the Mutatum docs. Your server seems to be running v{serverVersion.Version}.");

    builder.Services.AddDbContext<AppDbContext, MySqlDbContext>(options =>
            options.UseMySql(
                        dbOptions.MySQL.ConnectionString,
                        serverVersion));
}
else if (dbOptions.Provider == "MariaDB")
{
    var serverVersion = ServerVersion.AutoDetect(dbOptions.MariaDB.ConnectionString);

    builder.Services.AddDbContext<AppDbContext, MariaDbContext>(options =>
            options.UseMySql(
                        dbOptions.MariaDB.ConnectionString,
                        serverVersion));
}
else if (dbOptions.Provider == "MsSQL")
{
    builder.Services.AddDbContext<AppDbContext, MsSqlDbContext>(options => options.UseSqlServer(dbOptions.MsSQL.ConnectionString));
}
else if (dbOptions.Provider == "Postgres")
{
    builder.Services.AddDbContext<AppDbContext, PostgresDbContext>(options => options.UseNpgsql(dbOptions.Postgres.ConnectionString));
}
else if (dbOptions.Provider == "SQLite")
{
    builder.Services.AddDbContext<AppDbContext, SQLiteDbContext>(options => options.UseSqlite(dbOptions.SQLite.ConnectionString));
}
else
{
    throw new NotSupportedException($"Unknown database provider '{dbOptions.Provider}'. Please read the Mutatum docs.");
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        // We use this to block new users access without an existing user/admin confirming the account first
        options.SignIn.RequireConfirmedAccount = true; // true = users without a confirmed account/email cannot login

        // This will enable a temporary lockout if the wrong password is provided too many times
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 4;

        options.Stores.MaxLengthForKeys = 128; // Helps avoid issues in MsSQL and MySQL regarding index length. Both MsSQL and MySQL need additional config, see MsSqlDbContext and MySqlDbContext
    })
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

// Data services
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ReleaseService>();
builder.Services.AddScoped<CategoryService>();

// Reading AppSettings options
builder.Services.Configure<BrandingOptions>(builder.Configuration.GetSection(BrandingOptions.AppsettingsSectionName));
builder.Services.Configure<FirstRunOptions>(builder.Configuration.GetSection(FirstRunOptions.AppsettingsSectionName));
builder.Services.Configure<AccountRateLimitProtectionOptions>(builder.Configuration.GetSection(AccountRateLimitProtectionOptions.AppsettingsSectionName));
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.AppsettingsSectionName));

builder.Services.AddScoped<AccountRateLimitProtection>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAnyOrigin", builder => builder.AllowAnyOrigin());
});

WebApplication app = builder.Build();

await SeedTestData.SeedAsync(app.Services.CreateScope().ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("AllowAnyOrigin");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
