using System.Security.Claims;

namespace Products.Read.API.Auth
{
    public class UserClaimsDTO
    {
        public List<Claim>? UserClaims { get; set; }
    }
}
