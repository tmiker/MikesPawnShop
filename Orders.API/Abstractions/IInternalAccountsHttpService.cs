using Orders.API.DTOs;

namespace Orders.API.Abstractions
{
    public interface IInternalAccountsHttpService
    {
        Task<(bool IsSuccess, AccountDTO? AccountDTO, string? ErrorMessage)> GetUserAccountDataAsync(CancellationToken? cancellationToken = null);
    }
}
