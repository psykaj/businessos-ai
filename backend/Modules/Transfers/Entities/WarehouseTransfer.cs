using backend.Common;

namespace backend.Modules.Transfers.Entities;

public enum TransferStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    InTransit = 3,
    Received = 4,
    Cancelled = 5
}

public class WarehouseTransfer : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid SourceWarehouseId { get; set; }
    public Guid DestinationWarehouseId { get; set; }
    
    public string TransferNumber { get; set; } = string.Empty; // e.g., "TRF-2026-0001"
    public TransferStatus Status { get; set; } = TransferStatus.Pending;
    public string ApprovalStatus { get; set; } = "Pending"; // Approved, Rejected, Pending, AutoApproved
    
    public Guid RequestedById { get; set; }
    public string RequestedByName { get; set; } = string.Empty;
    public Guid? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    
    // JSON serialized list of transfer items (SKU, Quantity, UnitPrice, ProductName)
    public string TransferredItemsJson { get; set; } = "[]";
    public int TotalItemsCount { get; set; }
    public decimal TotalTransferValue { get; set; }
    
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
    
    public string? TrackingNotes { get; set; }
    public string? RejectionReason { get; set; }
}
