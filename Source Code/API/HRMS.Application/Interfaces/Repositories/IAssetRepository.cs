using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IAssetRepository
    {
        // Category
        Task<long> CreateCategoryAsync(CreateAssetCategoryRequest r);
        Task<bool> UpdateCategoryAsync(UpdateAssetCategoryRequest r);
        Task<bool> DeleteCategoryAsync(long id, long tenantId, long deletedBy);
        Task<AssetCategoryDto?> GetCategoryByIdAsync(long id, long tenantId);
        Task<IEnumerable<AssetCategoryDto>> SearchCategoriesAsync(long tenantId, string? searchText, int page, int pageSize);

        // Asset Master
        Task<long> CreateAssetAsync(CreateAssetRequest r);
        Task<bool> UpdateAssetAsync(UpdateAssetRequest r);
        Task<bool> DeleteAssetAsync(long id, long tenantId, long deletedBy);
        Task<AssetDto?> GetAssetByIdAsync(long id, long tenantId);
        Task<IEnumerable<AssetDto>> SearchAssetsAsync(long tenantId, string? searchText, string? status, int page, int pageSize);

        // Assignment
        Task<long> CreateAssignmentAsync(CreateAssetAssignmentRequest r);
        Task<bool> UpdateAssignmentAsync(UpdateAssetAssignmentRequest r);
        Task<bool> ReturnAssignmentAsync(ReturnAssetAssignmentRequest r);
        Task<IEnumerable<AssetAssignmentDto>> SearchAssignmentsAsync(long tenantId, long? employeeId, long? assetId, string? status, int page, int pageSize);

        // Transfer
        Task<long> CreateTransferAsync(CreateAssetTransferRequest r);
        Task<bool> ApproveTransferAsync(long id, long tenantId, long approvedBy);
        Task<bool> CompleteTransferAsync(long id, long tenantId, long completedBy);
        Task<IEnumerable<AssetTransferDto>> GetTransfersAsync(long tenantId);

        // Maintenance
        Task<long> CreateMaintenanceAsync(CreateAssetMaintenanceRequest r);
        Task<bool> UpdateMaintenanceAsync(UpdateAssetMaintenanceRequest r);
        Task<bool> CloseMaintenanceAsync(long id, long tenantId, long closedBy, string? remarks);
        Task<IEnumerable<AssetMaintenanceDto>> GetMaintenanceAsync(long tenantId);

        // Repair
        Task<long> CreateRepairAsync(CreateAssetRepairRequest r);
        Task<bool> UpdateRepairAsync(UpdateAssetRepairRequest r);
        Task<bool> CloseRepairAsync(long id, long tenantId, long closedBy, string? remarks);
        Task<IEnumerable<AssetRepairDto>> GetRepairsAsync(long tenantId);

        // Warranty
        Task<long> CreateWarrantyAsync(CreateAssetWarrantyRequest r);
        Task<bool> UpdateWarrantyAsync(UpdateAssetWarrantyRequest r);
        Task<IEnumerable<AssetWarrantyDto>> GetWarrantiesAsync(long tenantId);
        Task<IEnumerable<WarrantyExpiryReportDto>> GetWarrantyExpiryReportAsync(long tenantId, int withinDays);

        // Depreciation
        Task<bool> CalculateDepreciationAsync(long tenantId, long createdBy);
        Task<bool> RecalculateDepreciationAsync(long tenantId, long modifiedBy);
        Task<IEnumerable<AssetDepreciationDto>> GetDepreciationsAsync(long tenantId);
        Task<IEnumerable<AssetDepreciationReportDto>> GetDepreciationReportAsync(long tenantId);

        // Audit
        Task<long> CreateAuditAsync(CreateAssetAuditRequest r);
        Task<bool> CompleteAuditAsync(long id, long tenantId, string status, string? findings, long modifiedBy);
        Task<IEnumerable<AssetAuditDto>> GetAuditsAsync(long tenantId);
        Task<IEnumerable<AssetAuditReportDto>> GetAuditReportAsync(long tenantId);

        // Disposal
        Task<long> CreateDisposalAsync(CreateAssetDisposalRequest r);
        Task<bool> ApproveDisposalAsync(long id, long tenantId, long approvedBy);
        Task<bool> CloseDisposalAsync(long id, long tenantId, long closedBy);
        Task<IEnumerable<AssetDisposalDto>> GetDisposalsAsync(long tenantId);

        // Return
        Task<long> CreateReturnAsync(CreateAssetReturnWorkflowRequest r);
        Task<bool> VerifyReturnAsync(long id, long tenantId, string? condition, long verifiedBy);
        Task<IEnumerable<AssetReturnWorkflowDto>> GetReturnsAsync(long tenantId);

        // Inventory
        Task<long> ReconcileInventoryAsync(long tenantId, long assetId, long locationId, int quantity, string status, long createdBy);
        Task<IEnumerable<AssetInventoryDto>> SearchInventoryAsync(long tenantId, long? locationId, string? status, int page, int pageSize);
    }
}
