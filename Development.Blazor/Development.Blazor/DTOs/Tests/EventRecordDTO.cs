namespace Development.Blazor.DTOs.Tests
{
    public class EventRecordDTO
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public string? AggregateType { get; set; }
        public int AggregateVersion { get; set; }
        public string? EventType { get; set; }
        public string? EventJson { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? CorrelationId { get; set; }

        //public EventRecordDTO(Guid aggregateId, string aggregateType, int aggregateVersion, string eventType, string eventJson, DateTime occurredAt, string correlationId)
        //{
        //    AggregateId = aggregateId;
        //    AggregateType = aggregateType;
        //    AggregateVersion = aggregateVersion;
        //    EventType = eventType;
        //    EventJson = eventJson;
        //    OccurredAt = occurredAt;
        //    CorrelationId = correlationId;
        //}
    }
}
