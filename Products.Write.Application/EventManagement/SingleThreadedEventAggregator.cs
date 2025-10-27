using Products.Write.Application.Abstractions;
using Products.Write.Domain.Base;

namespace Products.Write.Application.EventManagement
{
    public class SingleThreadedEventAggregator : IEventAggregator
    {
        private readonly IDictionary<Type, IList<DomainEventHandler<IDomainEvent>>> _handlerDictionary;

        public SingleThreadedEventAggregator()
        {
            _handlerDictionary = new Dictionary<Type, IList<DomainEventHandler<IDomainEvent>>>();
        }

        public void Register(IRegisterableEventHandlers registerableEventHandlers)
        {
            registerableEventHandlers.RegisterWithEventAggregator(this);
        }

        public void Register<T>(DomainEventHandler<T> eventHandler) where T : IDomainEvent
        {
            if (!_handlerDictionary.ContainsKey(typeof(T)))
            {
                _handlerDictionary[typeof(T)] = new List<DomainEventHandler<IDomainEvent>>();
            }
            IList<DomainEventHandler<IDomainEvent>> handlerList = _handlerDictionary[typeof(T)];
            handlerList.Add(evt => eventHandler((T)evt));
        }

        public void Raise(IDomainEvent domainEvent)
        {
            if (_handlerDictionary.ContainsKey(domainEvent.GetType()))
            {
                IList<DomainEventHandler<IDomainEvent>> handlerList = _handlerDictionary[domainEvent.GetType()];
                foreach (DomainEventHandler<IDomainEvent> handler in handlerList)
                {
                    handler.Invoke(domainEvent);
                }
            }
        }
    }
}
