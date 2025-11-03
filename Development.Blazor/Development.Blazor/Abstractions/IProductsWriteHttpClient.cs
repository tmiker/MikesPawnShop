using Development.Blazor.DTOs.Tests;
using Development.Blazor.DTOs.Write;
using Development.Blazor.Paging;

namespace Development.Blazor.Abstractions
{
    public interface IProductsWriteHttpClient
    {
        Task<(bool IsSuccess, IEnumerable<ProductSnapshotDTO>? ProductSnapshots, PaginationMetadata? PagingData, string? ErrorMessage)> GetProductSnapshotsAsync(
            Guid? aggregateId,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<EventRecordDTO>? EventRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetEventRecordsAsync(
            Guid? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<OutboxRecordDTO>? OutboxRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetOutboxRecordsAsync(
            Guid? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<SnapshotRecordDTO>? SnapshotRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetSnapshotRecordsAsync(
            Guid? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, Guid? AggregateId, string? ErrorMessage)> AddProductAsync(AddProductDTO addProductDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateStatusAsync(UpdateStatusDTO updateStatusDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> AddImageAsync(AddImageDTO addImageDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> AddDocumentAsync(AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken);

        // Dev Tests
        Task<(bool IsSuccess, string? ErrorMessage)> ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? Value, string? ErrorMessage)> GetCloudAmqpSettingsTestingDummyValueAsync(CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> PurgeDataAsync(PurgeDataDTO purgeDataDTO, CancellationToken cancellationToken);
    }
}
