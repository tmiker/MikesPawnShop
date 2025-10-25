namespace Products.Write.Infrastructure.Abstractions
{
    public interface IRegisterableEventHandlers
    {
        void RegisterWithEventAggregator(IEventAggregator eventAggregator);

    }
}
