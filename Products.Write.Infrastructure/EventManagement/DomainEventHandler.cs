using Products.Write.Domain.Base;

namespace Products.Write.Infrastructure.EventManagement
{
    public delegate void DomainEventHandler<T>(T evt) where T : IDomainEvent;
}
