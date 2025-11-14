using Development.Blazor.Client.Abstractions;
using Development.Blazor.Client.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Development.Blazor.HttpProxyServices
{
    [Route("localapi/[controller]")]
    [ApiController]
    public class CartsProxyController : ControllerBase
    {
        private readonly ICartsHttpService _cartHttpService;
        private readonly ILogger<CartsProxyController> _logger;

        public CartsProxyController(ICartsHttpService cartHttpService, ILogger<CartsProxyController> logger)
        {
            _cartHttpService = cartHttpService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("getApiUserInfo")]
        public async Task<ActionResult<ApiUserInfoDTO>> GetProductsApiUserInfo()
        {
            string uri = Request.GetDisplayUrl();
            _logger.LogInformation("Blazor Dev Server Proxy Services: GetProductsApiUserInfo was called. URI: {uri}", uri);
            var result = await _cartHttpService.GetCartsApiUserInfoAsync();
            if (result.IsSuccess) return Ok(result.ApiUserInfo);
            else return BadRequest(result.ErrorMessage);
        }
    }
}
