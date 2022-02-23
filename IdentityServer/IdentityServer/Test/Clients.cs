using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Test;

internal class Clients
{
    public static IEnumerable<Client> Get()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "oauthClient",
                ClientName = "Example client application using client credentials",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                AllowedScopes = new List<string> {"api1.read"}
            },
            new Client
            {
                ClientName = "Angular-Client",
                ClientId = "angular-client",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string>{ "https://localhost:4200/signin-callback", "https://localhost:4200/assets/silent-callback.html" },
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "schoolApi",
                    "role"
                },
                AllowedCorsOrigins = { "https://localhost:4200" },
                RequireClientSecret = false,
                PostLogoutRedirectUris = new List<string> { "https://localhost:4200/signout-callback" },
                RequireConsent = false,
                AccessTokenLifetime = 600
            }
        };
    }
}