using IdentityServer8;
using IdentityServer8.Models;

namespace QMSWebApplication.BackendServer.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];  

        public static IEnumerable<ApiScope> ApiScopes =>
        [
            new("api.qms", "QMS API")
        ];

        public static IEnumerable<ApiResource> ApiResources =>
        [
            new ApiResource("api.qms", "QMS API")
            {
                Scopes = { "api.qms" }
            }
        ];  

        public static IEnumerable<Client> Clients =>
        [
            new Client
            {
                ClientId = "webportal",
                ClientName = "Web Portal Client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowOfflineAccess = true,

                // Where to redirect after login
                RedirectUris = { "https://localhost:7022/signin-oidc" },

                // Where to redirect after logout   
                PostLogoutRedirectUris = { "https://localhost:7022/signout-callback-oidc" },

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "api.qms" 
                }
            },
            new Client
            {
                ClientId = "swagger",
                ClientName = "Swagger Client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                RequireClientSecret = false,
                RequirePkce = true,

                // Where to redirect after login
                RedirectUris =              { "https://localhost:7022/swagger/oauth2-redirect.html" },
                // Where to redirect after logout   
                PostLogoutRedirectUris =    { "https://localhost:7022/swagger/oauth2-redirect.html" },
                AllowedCorsOrigins =        { "https://localhost:7022" },

                //// secret for authentication
                //ClientSecrets =
                //{
                //    new Secret("secret".Sha256())
                //},

                // scopes that client has                                                                                                                                                                                       access to
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api.qms",
                    IdentityServerConstants.LocalApi.ScopeName
                },
                Enabled = true
            }
        ];

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddIdentityServer()
                .AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients);

            // omitted for brevity
        }
    }
}
