using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Read.API.Domain.Models
{
    [Table("MessageRecords")]
    public class ProductMessageRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = "Product";
        public int AggregateVersion { get; init; }
        public int MessageId { get; init; }
        public string MessageType { get; init; } = default!;
        public string MessageJson { get; init; } = default!;
        public string? CorrelationId { get; init; }

        public ProductMessageRecord(Guid aggregateId, int aggregateVersion, int messageId, string messageType, string messageJson, string? correlationId)
        {
            AggregateId = aggregateId;
            AggregateVersion = aggregateVersion;
            MessageId = messageId;
            MessageType = messageType;
            MessageJson = messageJson;
            CorrelationId = correlationId;
        }
    }
}
