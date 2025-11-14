using System.Security.Claims;

namespace Accounts.API.Auth
{
    public class UserClaimsDTO
    {
        public List<Claim>? UserClaims { get; set; }
    }
}
