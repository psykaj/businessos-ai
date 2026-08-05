using backend.Modules.Transfers.Entities;

namespace backend.Modules.Transfers.DTOs;

public record TransferItemDto(
    string Sku,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record CreateTransferRequestDto(
    Guid SourceWarehouseId,
    Guid DestinationWarehouseId,
    string RequestedByName,
    List<TransferItemDto> Items,
    string? Notes
);

public record TransferApprovalDto(
    bool IsApproved,
    string ApprovedByName,
    string? RejectionReason
);

public record UpdateTransferTrackingDto(
    TransferStatus Status,
    string? TrackingNotes
);

public record ReceiveTransferDto(
    string? ReceiptNotes
);

public record WarehouseTransferResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid SourceWarehouseId,
    Guid DestinationWarehouseId,
    string TransferNumber,
    TransferStatus Status,
    string ApprovalStatus,
    Guid RequestedById,
    string RequestedByName,
    Guid? ApprovedById,
    string? ApprovedByName,
    string TransferredItemsJson,
    int TotalItemsCount,
    decimal TotalTransferValue,
    DateTime RequestedAt,
    DateTime? ApprovedAt,
    DateTime? ShippedAt,
    DateTime? ReceivedAt,
    string? TrackingNotes,
    string? RejectionReason,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
