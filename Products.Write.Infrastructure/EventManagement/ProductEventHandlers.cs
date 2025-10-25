using Products.Write.Domain.Events;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Infrastructure.EventManagement
{
    public class ProductEventHandlers : IRegisterableEventHandlers
    {
        public const string AggregateType = "Product";

        public void RegisterWithEventAggregator(IEventAggregator eventAggregator)
        {
            eventAggregator.Register<ProductAdded>(OnProductAdded);
            eventAggregator.Register<StatusUpdated>(OnStatusUpdated);
            eventAggregator.Register<ImageAdded>(OnImageAdded);
            eventAggregator.Register<DocumentAdded>(OnDocumentAdded);
        }

        private void OnProductAdded(ProductAdded @event)
        {
            Console.WriteLine($"Product Added: {@event.Name}, Category: {@event.Category}, Price: {@event.Price} {@event.Currency}");
        }
        private void OnStatusUpdated(StatusUpdated @event)
        {
            Console.WriteLine($"Product Status Updated: {@event.AggregateId}, New Status: {@event.Status}");
        }
        private void OnImageAdded(ImageAdded @event)
        {
            Console.WriteLine($"Image Added to Product: {@event.AggregateId}, Image URL: {@event.ImageUrl}");
        }
        private void OnDocumentAdded(DocumentAdded @event)
        {
            Console.WriteLine($"Document Added to Product: {@event.AggregateId}, Document URL: {@event.DocumentUrl}");
        }
    }
}
