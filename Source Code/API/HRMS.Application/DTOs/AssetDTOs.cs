using System;

namespace HRMS.Application.DTOs
{
    // CATEGORIES
    public class AssetCategoryDto
    {
        public long AssetCategoryID { get; set; }
        public long TenantID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public long? ParentCategoryID { get; set; }
        public string? Description { get; set; }
        public bool IsDepreciable { get; set; }
    }

    public class CreateAssetCategoryRequest
    {
        public long TenantId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public long? ParentCategoryID { get; set; }
        public string? Description { get; set; }
        public bool IsDepreciable { get; set; } = true;
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetCategoryRequest
    {
        public long AssetCategoryID { get; set; }
        public long TenantId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public long? ParentCategoryID { get; set; }
        public string? Description { get; set; }
        public bool IsDepreciable { get; set; }
        public long ModifiedBy { get; set; }
    }

    // ASSET MASTER
    public class AssetDto
    {
        public long AssetID { get; set; }
        public long TenantID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetTag { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public long AssetCategoryID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal CurrentBookValue { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? CurrentHolder { get; set; }
        public string? WarrantyStatus { get; set; }
    }

    public class CreateAssetRequest
    {
        public long TenantId { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetTag { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public long AssetCategoryID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal CurrentBookValue { get; set; }
        public string Status { get; set; } = "Available";
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetRequest
    {
        public long AssetID { get; set; }
        public long TenantId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public long AssetCategoryID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal CurrentBookValue { get; set; }
        public string Status { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // ASSIGNMENTS
    public class AssetAssignmentDto
    {
        public long AssetAssignmentID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string AssignmentStatus { get; set; } = string.Empty;
    }

    public class CreateAssetAssignmentRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetAssignmentRequest
    {
        public long AssetAssignmentID { get; set; }
        public long TenantId { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public string AssignmentStatus { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    public class ReturnAssetAssignmentRequest
    {
        public long AssetAssignmentID { get; set; }
        public long TenantId { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string? ReturnCondition { get; set; }
        public long CreatedBy { get; set; }
    }

    // TRANSFERS
    public class AssetTransferDto
    {
        public long AssetTransferID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public long? FromEmployeeID { get; set; }
        public string? FromEmployeeName { get; set; }
        public long ToEmployeeID { get; set; }
        public string? ToEmployeeName { get; set; }
        public DateTime TransferDate { get; set; }
        public string? TransferReason { get; set; }
        public string TransferStatus { get; set; } = string.Empty;
    }

    public class CreateAssetTransferRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public long? FromEmployeeID { get; set; }
        public long ToEmployeeID { get; set; }
        public DateTime TransferDate { get; set; }
        public string? TransferReason { get; set; }
        public long CreatedBy { get; set; }
    }

    // MAINTENANCE
    public class AssetMaintenanceDto
    {
        public long AssetMaintenanceID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public string? VendorName { get; set; }
        public decimal Cost { get; set; }
        public string MaintenanceStatus { get; set; } = string.Empty;
    }

    public class CreateAssetMaintenanceRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public string? VendorName { get; set; }
        public decimal Cost { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetMaintenanceRequest
    {
        public long AssetMaintenanceID { get; set; }
        public long TenantId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public string? VendorName { get; set; }
        public decimal Cost { get; set; }
        public string MaintenanceStatus { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // REPAIR
    public class AssetRepairDto
    {
        public long AssetRepairID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public DateTime RepairDate { get; set; }
        public string? RepairReason { get; set; }
        public decimal RepairCost { get; set; }
        public string RepairStatus { get; set; } = string.Empty;
    }

    public class CreateAssetRepairRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public DateTime RepairDate { get; set; }
        public string? RepairReason { get; set; }
        public decimal RepairCost { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetRepairRequest
    {
        public long AssetRepairID { get; set; }
        public long TenantId { get; set; }
        public DateTime RepairDate { get; set; }
        public string? RepairReason { get; set; }
        public decimal RepairCost { get; set; }
        public string RepairStatus { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // WARRANTY
    public class AssetWarrantyDto
    {
        public long AssetWarrantyID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public string WarrantyProvider { get; set; } = string.Empty;
        public string? WarrantyStatus { get; set; }
    }

    public class CreateAssetWarrantyRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public string WarrantyProvider { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateAssetWarrantyRequest
    {
        public long AssetWarrantyID { get; set; }
        public long TenantId { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public string WarrantyProvider { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    public class WarrantyExpiryReportDto
    {
        public long AssetWarrantyID { get; set; }
        public long AssetID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public DateTime WarrantyEndDate { get; set; }
        public string WarrantyProvider { get; set; } = string.Empty;
    }

    // DEPRECIATION
    public class AssetDepreciationDto
    {
        public long AssetDepreciationID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string DepreciationMethod { get; set; } = string.Empty;
        public decimal DepreciationRate { get; set; }
        public decimal BookValue { get; set; }
    }

    public class AssetDepreciationReportDto
    {
        public long AssetDepreciationID { get; set; }
        public long AssetID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string DepreciationMethod { get; set; } = string.Empty;
        public decimal DepreciationRate { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal BookValue { get; set; }
    }

    // AUDIT
    public class AssetAuditDto
    {
        public long AssetAuditID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public DateTime AuditDate { get; set; }
        public long AuditorID { get; set; }
        public string AuditStatus { get; set; } = string.Empty;
        public string? Findings { get; set; }
    }

    public class CreateAssetAuditRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public DateTime AuditDate { get; set; }
        public long AuditorID { get; set; }
        public long CreatedBy { get; set; }
    }

    public class AssetAuditReportDto
    {
        public long AssetAuditID { get; set; }
        public long AssetID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; }
        public string Auditor { get; set; } = string.Empty;
        public string AuditStatus { get; set; } = string.Empty;
        public string? Findings { get; set; }
    }

    // DISPOSAL
    public class AssetDisposalDto
    {
        public long AssetDisposalID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public DateTime DisposalDate { get; set; }
        public string DisposalMethod { get; set; } = string.Empty;
        public decimal DisposalValue { get; set; }
        public string DisposalStatus { get; set; } = string.Empty;
    }

    public class CreateAssetDisposalRequest
    {
        public long TenantId { get; set; }
        public long AssetID { get; set; }
        public DateTime DisposalDate { get; set; }
        public string DisposalMethod { get; set; } = string.Empty;
        public decimal DisposalValue { get; set; }
        public long CreatedBy { get; set; }
    }

    // RETURN
    public class AssetReturnWorkflowDto
    {
        public long AssetReturnID { get; set; }
        public long TenantID { get; set; }
        public long AssetAssignmentID { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? ReturnCondition { get; set; }
        public string ReturnStatus { get; set; } = string.Empty;
    }

    public class CreateAssetReturnWorkflowRequest
    {
        public long TenantId { get; set; }
        public long AssetAssignmentID { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? ReturnCondition { get; set; }
        public long CreatedBy { get; set; }
    }

    // INVENTORY
    public class AssetInventoryDto
    {
        public long AssetInventoryID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public long LocationID { get; set; }
        public string? LocationName { get; set; }
        public int Quantity { get; set; }
        public string InventoryStatus { get; set; } = string.Empty;
    }
}
