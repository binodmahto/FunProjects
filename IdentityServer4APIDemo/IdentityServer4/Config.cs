using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer4APIDemo.IdentityServer4
{
    public class Config
    {
        private IConfiguration _configuration;
        public Config(IConfiguration configuration) => _configuration = configuration;

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
                 new IdentityResources.Email(),
                 new IdentityResource
                 {
                     Name = "role",
                     UserClaims = new List<string> {"admin"}
                 }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("demoapi.read", "Read Access to Weather API"),
                new ApiScope("demoapi.write", "Write Access to Weather API"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
             {
                  new ApiResource
                  {
                      Name = "demoapi",
                      DisplayName = "Demo API",
                      Scopes = new List<string> { "demoapi.read", "demoapi.write"},
                      ApiSecrets = new List<Secret> {new Secret("1234567890123456789".Sha256())},
                      UserClaims = new List<string> {"admin"}
                  }
             };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                 new Client
                 {
                     ClientId = "demoapi",
                     ClientSecrets = { new Secret("1234567890123456789".Sha256()) },
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     // scopes that client has access to
                     AllowedScopes = { "demoapi.read", "demoapi.write"},
                    // RequirePkce =false
                 }
            };
    }
}
