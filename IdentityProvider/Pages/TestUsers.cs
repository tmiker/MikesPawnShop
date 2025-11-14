using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace IdentityProvider;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address1 = new
            {
                street_address = "4600 Seton Center Pkwy",
                locality = "Austin, TX",
                postal_code = "78759",
                country = "USA"
            };
            var address2 = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = "69118",
                country = "Germany"
            };

            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "wendy",
                        Password = "wendy",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Wendy Davenport"),
                            new Claim(JwtClaimTypes.GivenName, "Wendy"),
                            new Claim(JwtClaimTypes.FamilyName, "Davenport"),
                            new Claim(JwtClaimTypes.Email, "wendy@somemail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://wendy.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address1), IdentityServerConstants.ClaimValueTypes.Json),
                            // roles
                            new Claim("role", "PaidUser"),
                            new Claim("role", "Canine"),
                            new Claim("role", "Consumer"),
                            new Claim("Genus", "Canis"),
                            new Claim("Species", "Familiaris")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "marlow",
                        Password = "marlow",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Marlow Bean"),
                            new Claim(JwtClaimTypes.GivenName, "Marlow"),
                            new Claim(JwtClaimTypes.FamilyName, "Bean"),
                            new Claim(JwtClaimTypes.Email, "marlow@somemail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://marlow.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address1), IdentityServerConstants.ClaimValueTypes.Json),
                            // roles
                            new Claim("role", "PaidUser"),
                            new Claim("role", "Canine"),
                            new Claim("role", "Consumer"),
                            new Claim("Genus", "Canis"),
                            new Claim("Species", "Familiaris")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "3",
                        Username = "mike",
                        Password = "mike",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Mike Riordan"),
                            new Claim(JwtClaimTypes.GivenName, "Mike"),
                            new Claim(JwtClaimTypes.FamilyName, "Riordan"),
                            new Claim(JwtClaimTypes.Email, "mike@somemail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://mike.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address1), IdentityServerConstants.ClaimValueTypes.Json),
                            // roles
                            new Claim("role", "Consumer"),
                            new Claim("role", "Employee"),
                            new Claim("role", "Manager"),
                            new Claim("role", "Admin")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "4",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address2), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim("role", "Consumer")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "5",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address2), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim("role", "Consumer")
                        }
                    }
            };
        }

    }
}

