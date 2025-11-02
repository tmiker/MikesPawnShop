using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Write.Infrastructure.Data
{
    public class SnapshotRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public string SnapshotType { get; set; }
        public int AggregateVersion { get; set; }
        public string SnapshotJson { get; set; }
        public DateTime RecordedAt { get; set; }

        public SnapshotRecord(Guid aggregateId, string snapshotType, int aggregateVersion, string snapshotJson)
        {
            AggregateId = aggregateId;
            SnapshotType = snapshotType;
            AggregateVersion = aggregateVersion;
            SnapshotJson = snapshotJson;
            RecordedAt = DateTime.UtcNow;
        }
    }
}
