using Entities;
using IdentityServer.Extensions;
using IdentityServer.Test;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Use environment variables with local appsettings fallback.
builder.Configuration.AddJsonFile("appsettings.json").
    AddJsonFile("appsettings.docker.json", true).
    AddEnvironmentVariables();

// Asp Net Core Identity + Identity Server implementation

// Setup database to store users for Identity Server
builder.Services.ConfigureDbContextForIdentity(builder.Configuration);
builder.Services.ConfigureIdentityWithDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Setup for IdentityServer, these IS configurations automatically configures our CORS policy, so no need to touch that.
builder.Services
    .AddIdentityServer()
    //.AddTestUsers(TestUsers.Users)
    .AddDeveloperSigningCredential()
    .ConfigureISOperationalStoreSqlContext(builder.Configuration)
    .ConfigureISClientsAndScopesStoreSqlContext(builder.Configuration)
    .AddAspNetIdentity<User>();

builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureNonBreakingSameSiteCookies();
//builder.Services.ConfigureExternalCookie(options =>
//{
//    options.Cookie.IsEssential = true;
//    options.Cookie.SameSite = (SameSiteMode)(-1); //SameSiteMode.Unspecified in .NET Core 3.1
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.IsEssential = true;
//    options.Cookie.SameSite = (SameSiteMode)(-1); //SameSiteMode.Unspecified in .NET Core 3.1
//});

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseCookiePolicy();
app.UseDeveloperExceptionPage();

ServiceExtensions.InitializeDbTestData(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    //app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
//app.Use((context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", "CSP stuff");
//    return next();
//});

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();