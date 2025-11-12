using Carts.API.Abstractions;
using Carts.API.Auth;
using Carts.API.DTOs;
using Carts.API.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using System.Text;

namespace Carts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ITokenDecoder _tokenDecoder;
        private readonly ILogger<CartsController> _logger;

        public CartsController(ICartService cartService, ITokenDecoder tokenDecoder, ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _tokenDecoder = tokenDecoder;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO?>> GetShoppingCart()
        {
            await LogIdentityInformation();
            string? ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null) throw new InvalidUserCredentitalsException($"User identity information unavailable. Unauthorized access to restricted resource.");
            //return BadRequest($"User identity information unavailable. Unauthorized access to restricted resource.");
            ShoppingCartDTO? cartDTO = await _cartService.GetCartAsync(ownerId);
            return Ok(cartDTO);
        }

        [HttpPut]
        public async Task<IActionResult> SaveShoppingCart(ShoppingCartDTO cartDTO)
        {
            //await LogIdentityInformation();
            string? ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null) throw new InvalidUserCredentitalsException($"User identity information unavailable. Unauthorized access to restricted resource.");
            //return BadRequest($"User identity information unavailable. Unauthorized access to restricted resource.");
            bool success = await _cartService.UpdateCartAsync(cartDTO, ownerId);
            if (success) return NoContent();
            else return BadRequest("Error saving shopping cart.");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShoppingCart()
        {
            //await LogIdentityInformation();
            string? ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null) throw new InvalidUserCredentitalsException($"User identity information unavailable. Unauthorized access to restricted resource.");
            //return BadRequest($"User identity information unavailable. Unauthorized access to restricted resource.");
            bool success = await _cartService.RemoveCartAsync(ownerId);
            if (success) return NoContent();
            else return BadRequest("Error removing shopping cart.");
        }

        [HttpGet("[action]")]
        public ActionResult<ApiUserInfoDTO> GetApiUserInfo()
        {
            _logger.LogInformation($"External Carts API method GetApiUserInfo was called.");

            string authorizationHeaderValue = Request.Headers.Authorization.ToString();
            string accessToken = authorizationHeaderValue.Substring("Bearer ".Length);
            ApiUserInfoDTO apiUserInfoDTO = _tokenDecoder.GetTokenData(accessToken);

            List<Claim> userClaims = User.Claims.ToList();
            if (userClaims.Any())
            {
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "role") apiUserInfoDTO.ApiUserClaimsRolesList.Add(claim.Value);
                    apiUserInfoDTO.ApiUserClaimsClaimsList.Add($"{claim.Type} : {claim.Value}");
                }
            }
            else
            {
                apiUserInfoDTO.ApiUserClaimsClaimsList.Add("User.Claims did not contain any claims.");
            }
            return Ok(apiUserInfoDTO);
        }

        private async Task LogIdentityInformation()
        {
            // get saved identity token
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var userClaimsStringBuilder = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                userClaimsStringBuilder.AppendLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            _logger.LogInformation($"Identity Token and User Claims: \n  {identityToken}\n    {userClaimsStringBuilder}");
            _logger.LogInformation($"Access token: \n{accessToken}");
        }

        private string? GetCurrentOwnerId()
        {
            string? ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null) throw new InvalidUserCredentitalsException($"User identity information unavailable. Unauthorized access to restricted resource.");
            return ownerId;
        }

        [HttpGet("testApplicationException")]
        public IActionResult TestApplicationException()
        {
            string testResult = $"External Carts TestApplicationException Endpoint called. Throwing Application Exception.";
            if (testResult != "success") throw new CartsDomainApplicationException($"{testResult} || Carts domain application exception thrown.");
            return Ok(testResult);
        }

        [HttpGet("testException")]
        public IActionResult TestException()
        {
            string testResult = $"External Carts TestException Endpoint called. Throwing Exception.";
            if (testResult != "success") throw new CartsDomainException($"{testResult} || Carts domain exception thrown.");
            return Ok(testResult);
        }

    }
}
