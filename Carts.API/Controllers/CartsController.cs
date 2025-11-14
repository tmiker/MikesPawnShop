using Carts.API.Abstractions;
using Carts.API.Auth;
using Carts.API.DTOs;
using Carts.API.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Carts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ITokenDecoder _tokenDecoder;
        private readonly ILogger<CartsController> _logger;

        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

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
            _logger.LogInformation($"LOG INDENTITY INFORMATION METHOD RESULT: ");
            _logger.LogInformation($"Identity Token: \n{identityToken}\n   ");
            _logger.LogInformation($"Access Token: \n{accessToken}\n");
            _logger.LogInformation($"User Claims: \n{userClaimsStringBuilder}\n");
        }

        private string? GetCurrentOwnerId()
        {
            string? ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null) throw new InvalidUserCredentitalsException($"User identity information unavailable. Unauthorized access to restricted resource.");
            return ownerId;
        }
    }
}
