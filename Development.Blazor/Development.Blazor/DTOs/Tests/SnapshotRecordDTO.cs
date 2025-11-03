namespace Development.Blazor.DTOs.Tests
{
    public class SnapshotRecordDTO
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public string? SnapshotType { get; set; }
        public int AggregateVersion { get; set; }
        public string? SnapshotJson { get; set; }
        public DateTime RecordedAt { get; set; }

        //public SnapshotRecordDTO(Guid aggregateId, string snapshotType, int aggregateVersion, string snapshotJson)
        //{
        //    AggregateId = aggregateId;
        //    SnapshotType = snapshotType;
        //    AggregateVersion = aggregateVersion;
        //    SnapshotJson = snapshotJson;
        //    RecordedAt = DateTime.UtcNow;
        //}
    }
}
