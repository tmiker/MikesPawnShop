using Products.Write.Domain.Base;
using Products.Write.Infrastructure.EventManagement;

namespace Products.Write.Infrastructure.Abstractions
{
    public interface IEventAggregator
    {
        void Register(IRegisterableEventHandlers registerableEventHandlers);
        void Register<T>(DomainEventHandler<T> eventHandler) where T : IDomainEvent;
        void Raise(IDomainEvent domainEvent);
    }
}


// eventaggregator.Register(handlers) => handler.RegisterWithEventAggregator(eventAggregator) ===>>> calls eventAggregator.Register<T>(eventHandler) for each handler