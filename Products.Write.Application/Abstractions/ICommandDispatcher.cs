namespace Products.Write.Application.Abstractions
{
    public interface ICommandDispatcher
    {
        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken);
    }
}
