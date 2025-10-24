using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Write.Infrastructure.Data
{
    [Table("Product Event Records", Schema = "dbo")]
    public class EventRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public int AggregateVersion { get; set; }
        public string EventType { get; set; }
        public string EventJson { get; set; }
        public DateTime OccurredAt { get; set; }
        public string CorrelationId { get; set; }

        public EventRecord(Guid aggregateId, string aggregateType, int aggregateVersion, string eventType, string eventJson, DateTime occurredAt, string correlationId)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            EventType = eventType;
            EventJson = eventJson;
            OccurredAt = occurredAt;
            CorrelationId = correlationId;
        }
    }
}
