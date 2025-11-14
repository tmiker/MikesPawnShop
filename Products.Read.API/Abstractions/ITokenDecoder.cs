using Products.Read.API.Auth;

namespace Products.Read.API.Abstractions
{
    public interface ITokenDecoder
    {
        string? GetUserId(string token);
        ApiUserInfoDTO GetTokenData(string? token);
        UserClaimsDTO GetUserClaims(string token);
    }
}
