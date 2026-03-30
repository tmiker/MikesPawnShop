using Accounts.API.DTOs;

namespace Accounts.API.Abstractions
{
    public interface IKeyContainerService
    {
        (bool IsSuccess, KeyContainerResponseDTO? KeyContainerResponse, string? ErrorMessage) GetPublicKeyForSpecifiedContainerAsync();
    }
}
