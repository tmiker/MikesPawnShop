namespace Products.Write.Application.Abstractions
{
    public interface IProductCommandManagementService
    {
        Task<TResult> ExecuteCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken);
    }
}
