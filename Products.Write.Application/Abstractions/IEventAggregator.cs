﻿using Products.Write.Application.EventManagement;
using Products.Write.Domain.Base;

namespace Products.Write.Application.Abstractions
{
    public interface IEventAggregator
    {
        void Register(IRegisterableEventHandlers registerableEventHandlers);
        void Register<T>(DomainEventHandler<T> eventHandler) where T : IDomainEvent;
        void Raise(IDomainEvent domainEvent);
    }
}


// eventaggregator.Register(handlers) => handler.RegisterWithEventAggregator(eventAggregator) ===>>> calls eventAggregator.Register<T>(eventHandler) for each handler