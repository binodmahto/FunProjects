using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4APIDemo.IdentityServer4
{
    public class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "123456786",
                Username = "admin",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "binodk.mahto@gmail.com"),
                    new Claim(JwtClaimTypes.Role, "admin"),
                    new Claim(JwtClaimTypes.WebSite, "https://localhost:5001")
                }
            },
            new TestUser
            {
                SubjectId = "123456787",
                Username = "user1",
                Password = "user1",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "binodk.mahto@gmail.com"),
                    new Claim(JwtClaimTypes.Role, "user"),
                    new Claim(JwtClaimTypes.WebSite, "https://localhost:5001")
                }
            },
            new TestUser
            {
                SubjectId = "123456788",
                Username = "user2",
                Password = "user2",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "binodk.mahto@gmail.com"),
                    new Claim(JwtClaimTypes.Role, "user"),
                    new Claim(JwtClaimTypes.WebSite, "https://localhost:5001")
                }
            }
        };
        }
    }
}
