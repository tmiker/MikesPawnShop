
using Orders.API.DTOs;

namespace Orders.API.Abstractions
{
    public interface IKeyContainerService
    {
        (bool IsSuccess, KeyContainerResponseDTO? KeyContainerResponse, string? ErrorMessage) GetPublicKeyForSpecifiedContainerAsync();
    }
}
