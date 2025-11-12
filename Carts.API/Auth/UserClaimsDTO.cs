using System.Security.Claims;

namespace Carts.API.Auth
{
    public class UserClaimsDTO
    {
        public List<Claim>? UserClaims { get; set; }
    }
}
