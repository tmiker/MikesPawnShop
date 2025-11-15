using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Your role(s)", new [] { "role" })    // name, display name, list of claims
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("cartsapi.fullaccess"),
                new ApiScope("productsreadapi.fullaccess"),
                new ApiScope("productswriteapi.fullaccess"),
                new ApiScope("accountsapi.fullaccess"),
                new ApiScope("ordersapi.fullaccess")
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("cartsapi", "Shopping Carts API", new [] { "role" })
                {
                    Scopes = { "cartsapi.fullaccess" }
                },
            new ApiResource("productsreadapi", "Products Read API", new[] { "role" })
                {
                    Scopes = { "productsreadapi.fullaccess" }
                },
            new ApiResource("productswriteapi", "Products Write API", new[] { "role" })
                {
                    Scopes = { "productswriteapi.fullaccess" }
                },
            new ApiResource("accountsapi", "Accounts API", new[] { "role" })
                {
                    Scopes = { "accountsapi.fullaccess" }
                },
            new ApiResource("ordersapi", "Orders API", new[] { "role" })
                {
                    Scopes = { "ordersapi.fullaccess" }
                }
            
        };
    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client()
                    {
                        ClientName = "DevTestBlazorServer",
                        ClientId = "devTestBlazorServer",
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireConsent = true,   // default = false
                        RedirectUris = { "https://localhost:7030/signin-oidc" },        //  where client will receive tokens, default = base uri + /signin-oidc
                        PostLogoutRedirectUris = { "https://localhost:7030/signout-callback-oidc" },       
                        // FrontChannelLogoutUri =    "https://localhost:7030/signout-callback-oidc",      
                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "roles",
                            "cartsapi.fullaccess",
                            "productsreadapi.fullaccess",
                            "productswriteapi.fullaccess",
                            "accountsapi.fullaccess",
                            "ordersapi.fullaccess"
                        },
                        ClientSecrets = { new Secret("wendyandmarlowFSD".Sha256()) }    // *** move to secrets 
                        //ClientSecrets = { new Secret("wendyandmarlowarethebestoffriendsforclientblazor".Sha256()) }
                    }

            };
}
