using Development.Blazor.DTOs.Tests;
using Development.Blazor.DTOs.Write;
using Development.Blazor.Paging;

namespace Development.Blazor.Abstractions
{
    public interface IProductsWriteHttpClient
    {
        Task<(bool IsSuccess, Guid? AggregateId, string? ErrorMessage)> AddProductAsync(AddProductDTO addProductDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateStatusAsync(UpdateStatusDTO updateStatusDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> AddImageAsync(AddImageDTO addImageDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> AddDocumentAsync(AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken);

        // UPDATED METHODS FOR IMAGES AND DOCUMENTS
        Task<(bool IsSuccess, string? ErrorMessage)> AddProductImageAsync(AddImageDTO addImageDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> AddProductDocumentAsync(AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> DeleteProductImageAsync(DeleteImageDTO deleteImageDTO);
        Task<(bool IsSuccess, string? ErrorMessage)> DeleteProductDocumentAsync(DeleteDocumentDTO deleteDocumentDTO);

        // Dev Tests
        Task<(bool IsSuccess, IEnumerable<ProductSnapshotDTO>? ProductSnapshots, PaginationMetadata? PagingData, string? ErrorMessage)> GetProductSnapshotsAsync(
            string? aggregateId,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<EventRecordDTO>? EventRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetEventRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<OutboxRecordDTO>? OutboxRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetOutboxRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, IEnumerable<SnapshotRecordDTO>? SnapshotRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetSnapshotRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10);

        Task<(bool IsSuccess, string? ErrorMessage)> ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? Value, string? ErrorMessage)> GetCloudAmqpSettingsTestingDummyValueAsync(CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? ErrorMessage)> PurgeDataAsync(PurgeDataDTO purgeDataDTO, CancellationToken cancellationToken);
    }
}
