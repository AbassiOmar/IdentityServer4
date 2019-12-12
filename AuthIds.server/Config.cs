using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthIds.server
{
    public static class Config
    {

        // ressource to be protected
        public static IEnumerable<ApiResource> Apis =>
              new List<ApiResource>
              {
                    new ApiResource("protectedApi", "My API to prtect")
              };

        // clients to w'll be use ressources and to be secure
        public static IEnumerable<Client> Clients =>
          new List<Client>
          {
        // machine to machine client (from quickstart 1)
        new Client
        {
            ClientId = "client",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            // scopes that client has access to
            AllowedScopes = { "protectedApi" }
        },
        // interactive ASP.NET Core MVC client
        new Client
        {
           ClientId = "mvcClient",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,
            RequireConsent = false,
            RequirePkce = false,

            // where to redirect to after login
            RedirectUris = { "http://localhost:5002/signin-oidc" },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "clientApi"
            },
            AllowOfflineAccess=true
        }
          };

        //identity to be protected
        public static IEnumerable<IdentityResource> Ids =>
             new List<IdentityResource>
             {
             new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
             };
    }
}
