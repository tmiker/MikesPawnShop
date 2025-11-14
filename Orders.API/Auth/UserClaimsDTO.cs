using System.Security.Claims;

namespace Orders.API.Auth
{
    public class UserClaimsDTO
    {
        public List<Claim>? UserClaims { get; set; }
    }
}
