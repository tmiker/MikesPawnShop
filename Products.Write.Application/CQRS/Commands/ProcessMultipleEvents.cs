namespace Products.Write.Application.CQRS.Commands
{
    public class ProcessMultipleEvents
    {
        public string CorrelationId { get; set; } 

        public ProcessMultipleEvents()
        {
            CorrelationId = Guid.NewGuid().ToString();
        }
    }
}
