using System.Security.Claims;

namespace Products.Write.API.Auth
{
    public class UserClaimsDTO
    {
        public List<Claim>? UserClaims { get; set; }
    }
}
