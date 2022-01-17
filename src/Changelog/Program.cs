using Changelog;
using Changelog.Areas.Identity;
using Changelog.Data;
using Changelog.Data.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Changelog"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // We use this to block new users access without an existing user/admin confirming the account first
        options.SignIn.RequireConfirmedAccount = true; // true = users without a confirmed account/email cannot login

        // This will enable a temporary lockout if the wrong password is provided too many times
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 4;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

{ // Data services
    builder.Services.AddScoped<ProjectService>();
    builder.Services.AddScoped<ReleaseService>();
    builder.Services.AddScoped<CategoryService>();
}

{ // Reading AppSettings options
    builder.Services.Configure<BrandingOptions>(builder.Configuration.GetSection(BrandingOptions.AppsettingsSectionName));
    builder.Services.Configure<FirstRunOptions>(builder.Configuration.GetSection(FirstRunOptions.AppsettingsSectionName));
    builder.Services.Configure<AccountRateLimitProtectionOptions>(builder.Configuration.GetSection(AccountRateLimitProtectionOptions.AppsettingsSectionName));
}

builder.Services.AddScoped<AccountRateLimitProtection>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAnyOrigin", builder => builder.AllowAnyOrigin());
});

var app = builder.Build();

await SeedTestData.Seed(app.Services.CreateScope().ServiceProvider);

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