using Products.Write.Domain.Base;

namespace Products.Write.Application.EventManagement
{
    public delegate void DomainEventHandler<T>(T evt) where T : IDomainEvent;
}
