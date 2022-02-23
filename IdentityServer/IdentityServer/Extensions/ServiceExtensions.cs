using System.Reflection;
using Contracts;
using Entities;
using IdentityServer.Test;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace IdentityServer.Extensions;

public static class ServiceExtensions
{
    private static readonly string assembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name ?? throw new InvalidOperationException();
    private const SameSiteMode Unspecified = (SameSiteMode)(-1);

    public static IServiceCollection ConfigureNonBreakingSameSiteCookies(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = Unspecified;
            options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        return services;
    }

    private static bool DisallowsSameSiteNone(string userAgent)
    {
        // Cover all iOS based browsers here. This includes:
        //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
        //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
        //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
        // All of which are broken by SameSite=None, because they use the
        // iOS networking stack.
        // Notes from Thinktecture:
        // Regarding https://caniuse.com/#search=samesite iOS versions lower
        // than 12 are not supporting SameSite at all. Starting with version 13
        // unknown values are NOT treated as strict anymore. Therefore we only
        // need to check version 12.
        if (userAgent.Contains("CPU iPhone OS 12")
           || userAgent.Contains("iPad; CPU OS 12"))
        {
            return true;
        }

        // Cover Mac OS X based browsers that use the Mac OS networking stack.
        // This includes:
        //   - Safari on Mac OS X.
        // This does not include:
        //   - Chrome on Mac OS X
        // because they do not use the Mac OS networking stack.
        // Notes from Thinktecture:
        // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
        // than 10.14 are not supporting SameSite at all. Starting with version
        // 10.15 unknown values are NOT treated as strict anymore. Therefore we
        // only need to check version 10.14.
        if (userAgent.Contains("Safari")
           && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
           && userAgent.Contains("Version/"))
        {
            return true;
        }

        // Cover Chrome 50-69, because some versions are broken by SameSite=None
        // and none in this range require it.
        // Note: this covers some pre-Chromium Edge versions,
        // but pre-Chromium Edge does not require SameSite=None.
        // Notes from Thinktecture:
        // We can not validate this assumption, but we trust Microsofts
        // evaluation. And overall not sending a SameSite value equals to the same
        // behavior as SameSite=None for these old versions anyways.
        if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
        {
            return true;
        }

        return false;
    }

    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None)
        {
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            if (DisallowsSameSiteNone(userAgent))
            {
                options.SameSite = Unspecified;
            }
        }
    }

    // Identity Server
    public static IIdentityServerBuilder ConfigureISOperationalStoreSqlContext(this IIdentityServerBuilder isBuilder, IConfiguration configuration)
    {
        isBuilder.AddOperationalStore(opts =>
            opts.ConfigureDbContext =
                builder => builder.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
            b.MigrationsAssembly(assembly)));
        return isBuilder;
    }

    public static IIdentityServerBuilder ConfigureISClientsAndScopesStoreSqlContext(this IIdentityServerBuilder isBuilder, IConfiguration configuration)
    {
        isBuilder.AddConfigurationStore(options => options.ConfigureDbContext =
            builder => builder.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(assembly)));
        return isBuilder;
    }

    // Identity

    public static void ConfigureDbContextForIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(builder =>
            builder.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(assembly)));
    }

    public static void ConfigureIdentityWithDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<RepositoryContext>();
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddScoped<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    /// <summary>
    /// A small bootstrapping method that will run EF migrations against the database
    /// and create your test data.
    /// </summary>
    public static void InitializeDbTestData(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                foreach (var client in Clients.Get())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                foreach (var client in Clients.Get())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var scope in Resources.GetApiScopes())
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            if (!userManager.Users.Any())
            {
                foreach (var testUser in Users.Get())
                {
                    var identityUser = new User(testUser.Username)
                    {
                        Id = testUser.SubjectId,
                        TestProperty = "This is a customized user"
                    };

                    userManager.CreateAsync(identityUser, "Password123!").Wait();
                    userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                }
            }
        }
    }
}