using Carts.API.Auth;

namespace Carts.API.Abstractions
{
    public interface ITokenDecoder
    {
        string? GetUserId(string token);
        ApiUserInfoDTO GetTokenData(string? token);
        UserClaimsDTO GetUserClaims(string token);
    }
}
