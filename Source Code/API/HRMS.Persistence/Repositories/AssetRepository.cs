using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly string _connectionString;
        public AssetRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Category
        public async Task<long> CreateCategoryAsync(CreateAssetCategoryRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@CategoryCode", r.CategoryCode);
            p.Add("@CategoryName", r.CategoryName);
            p.Add("@ParentCategoryID", r.ParentCategoryID);
            p.Add("@Description", r.Description);
            p.Add("@IsDepreciable", r.IsDepreciable);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetCategoryID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetCategory_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetCategoryID");
        }

        public async Task<bool> UpdateCategoryAsync(UpdateAssetCategoryRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetCategoryID", r.AssetCategoryID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@CategoryName", r.CategoryName);
            p.Add("@ParentCategoryID", r.ParentCategoryID);
            p.Add("@Description", r.Description);
            p.Add("@IsDepreciable", r.IsDepreciable);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetCategory_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteCategoryAsync(long id, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetCategoryID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetCategory_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<AssetCategoryDto?> GetCategoryByIdAsync(long id, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetCategoryID", id);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<AssetCategoryDto>("asset.usp_AssetCategory_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AssetCategoryDto>> SearchCategoriesAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("asset.usp_AssetCategory_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<AssetCategoryDto>();
        }

        // Asset Master
        public async Task<long> CreateAssetAsync(CreateAssetRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetCode", r.AssetCode);
            p.Add("@AssetTag", r.AssetTag);
            p.Add("@AssetName", r.AssetName);
            p.Add("@AssetCategoryID", r.AssetCategoryID);
            p.Add("@Manufacturer", r.Manufacturer);
            p.Add("@Model", r.Model);
            p.Add("@SerialNumber", r.SerialNumber);
            p.Add("@PurchaseDate", r.PurchaseDate);
            p.Add("@PurchaseCost", r.PurchaseCost);
            p.Add("@CurrentBookValue", r.CurrentBookValue);
            p.Add("@Status", r.Status);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_Asset_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetID");
        }

        public async Task<bool> UpdateAssetAsync(UpdateAssetRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetID", r.AssetID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetName", r.AssetName);
            p.Add("@AssetCategoryID", r.AssetCategoryID);
            p.Add("@Manufacturer", r.Manufacturer);
            p.Add("@Model", r.Model);
            p.Add("@SerialNumber", r.SerialNumber);
            p.Add("@PurchaseDate", r.PurchaseDate);
            p.Add("@PurchaseCost", r.PurchaseCost);
            p.Add("@CurrentBookValue", r.CurrentBookValue);
            p.Add("@Status", r.Status);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_Asset_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAssetAsync(long id, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("asset.usp_Asset_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<AssetDto?> GetAssetByIdAsync(long id, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetID", id);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<AssetDto>("asset.usp_Asset_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AssetDto>> SearchAssetsAsync(long tenantId, string? searchText, string? status, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@Status", status);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("asset.usp_Asset_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<AssetDto>();
        }

        // Assignment
        public async Task<long> CreateAssignmentAsync(CreateAssetAssignmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@AssignedDate", r.AssignedDate);
            p.Add("@ExpectedReturnDate", r.ExpectedReturnDate);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetAssignmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetAssignment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetAssignmentID");
        }

        public async Task<bool> UpdateAssignmentAsync(UpdateAssetAssignmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetAssignmentID", r.AssetAssignmentID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@ExpectedReturnDate", r.ExpectedReturnDate);
            p.Add("@AssignmentStatus", r.AssignmentStatus);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetAssignment_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> ReturnAssignmentAsync(ReturnAssetAssignmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetAssignmentID", r.AssetAssignmentID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@ReturnedDate", r.ReturnedDate);
            p.Add("@ReturnCondition", r.ReturnCondition);
            p.Add("@CreatedBy", r.CreatedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetAssignment_Return", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetAssignmentDto>> SearchAssignmentsAsync(long tenantId, long? employeeId, long? assetId, string? status, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@AssetID", assetId);
            p.Add("@Status", status);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("asset.usp_AssetAssignment_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<AssetAssignmentDto>();
        }

        // Transfer
        public async Task<long> CreateTransferAsync(CreateAssetTransferRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@FromEmployeeID", r.FromEmployeeID);
            p.Add("@ToEmployeeID", r.ToEmployeeID);
            p.Add("@TransferDate", r.TransferDate);
            p.Add("@TransferReason", r.TransferReason);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetTransferID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetTransfer_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetTransferID");
        }

        public async Task<bool> ApproveTransferAsync(long id, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetTransferID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApprovedBy", approvedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetTransfer_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CompleteTransferAsync(long id, long tenantId, long completedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetTransferID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@CompletedBy", completedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetTransfer_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetTransferDto>> GetTransfersAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetTransferDto>("SELECT * FROM asset.vw_AssetTransfers WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        // Maintenance
        public async Task<long> CreateMaintenanceAsync(CreateAssetMaintenanceRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@MaintenanceDate", r.MaintenanceDate);
            p.Add("@MaintenanceType", r.MaintenanceType);
            p.Add("@VendorName", r.VendorName);
            p.Add("@Cost", r.Cost);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetMaintenanceID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetMaintenance_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetMaintenanceID");
        }

        public async Task<bool> UpdateMaintenanceAsync(UpdateAssetMaintenanceRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetMaintenanceID", r.AssetMaintenanceID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@MaintenanceDate", r.MaintenanceDate);
            p.Add("@MaintenanceType", r.MaintenanceType);
            p.Add("@VendorName", r.VendorName);
            p.Add("@Cost", r.Cost);
            p.Add("@MaintenanceStatus", r.MaintenanceStatus);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetMaintenance_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CloseMaintenanceAsync(long id, long tenantId, long closedBy, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetMaintenanceID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", closedBy);
            p.Add("@Remarks", remarks);
            var affected = await conn.ExecuteAsync("asset.usp_AssetMaintenance_Close", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetMaintenanceDto>> GetMaintenanceAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetMaintenanceDto>("SELECT * FROM asset.vw_AssetMaintenance WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        // Repair
        public async Task<long> CreateRepairAsync(CreateAssetRepairRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@RepairDate", r.RepairDate);
            p.Add("@RepairReason", r.RepairReason);
            p.Add("@RepairCost", r.RepairCost);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetRepairID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetRepair_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetRepairID");
        }

        public async Task<bool> UpdateRepairAsync(UpdateAssetRepairRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetRepairID", r.AssetRepairID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@RepairDate", r.RepairDate);
            p.Add("@RepairReason", r.RepairReason);
            p.Add("@RepairCost", r.RepairCost);
            p.Add("@RepairStatus", r.RepairStatus);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetRepair_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CloseRepairAsync(long id, long tenantId, long closedBy, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetRepairID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", closedBy);
            p.Add("@Remarks", remarks);
            var affected = await conn.ExecuteAsync("asset.usp_AssetRepair_Close", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetRepairDto>> GetRepairsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetRepairDto>("SELECT * FROM asset.vw_AssetWarrantyStatus WHERE TenantID = @TenantID", new { TenantID = tenantId }); // Fallback select
        }

        // Warranty
        public async Task<long> CreateWarrantyAsync(CreateAssetWarrantyRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@WarrantyStartDate", r.WarrantyStartDate);
            p.Add("@WarrantyEndDate", r.WarrantyEndDate);
            p.Add("@WarrantyProvider", r.WarrantyProvider);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetWarrantyID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetWarranty_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetWarrantyID");
        }

        public async Task<bool> UpdateWarrantyAsync(UpdateAssetWarrantyRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetWarrantyID", r.AssetWarrantyID);
            p.Add("@TenantID", r.TenantId);
            p.Add("@WarrantyStartDate", r.WarrantyStartDate);
            p.Add("@WarrantyEndDate", r.WarrantyEndDate);
            p.Add("@WarrantyProvider", r.WarrantyProvider);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetWarranty_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetWarrantyDto>> GetWarrantiesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetWarrantyDto>("SELECT * FROM asset.vw_AssetWarrantyStatus WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<WarrantyExpiryReportDto>> GetWarrantyExpiryReportAsync(long tenantId, int withinDays)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@WithinDays", withinDays);
            return await conn.QueryAsync<WarrantyExpiryReportDto>("asset.usp_AssetWarranty_ExpiryReport", p, commandType: CommandType.StoredProcedure);
        }

        // Depreciation
        public async Task<bool> CalculateDepreciationAsync(long tenantId, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@CreatedBy", createdBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetDepreciation_Calculate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RecalculateDepreciationAsync(long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetDepreciation_Recalculate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetDepreciationDto>> GetDepreciationsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetDepreciationDto>("SELECT * FROM asset.vw_AssetDepreciation WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<AssetDepreciationReportDto>> GetDepreciationReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<AssetDepreciationReportDto>("asset.usp_AssetDepreciation_Report", p, commandType: CommandType.StoredProcedure);
        }

        // Audit
        public async Task<long> CreateAuditAsync(CreateAssetAuditRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@AuditDate", r.AuditDate);
            p.Add("@AuditorID", r.AuditorID);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetAuditID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetAudit_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetAuditID");
        }

        public async Task<bool> CompleteAuditAsync(long id, long tenantId, string status, string? findings, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetAuditID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@AuditStatus", status);
            p.Add("@Findings", findings);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetAudit_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetAuditDto>> GetAuditsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetAuditDto>("SELECT * FROM asset.vw_AssetWarrantyStatus WHERE TenantID = @TenantID", new { TenantID = tenantId }); // Fallback select
        }

        public async Task<IEnumerable<AssetAuditReportDto>> GetAuditReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<AssetAuditReportDto>("asset.usp_AssetAudit_Report", p, commandType: CommandType.StoredProcedure);
        }

        // Disposal
        public async Task<long> CreateDisposalAsync(CreateAssetDisposalRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetID", r.AssetID);
            p.Add("@DisposalDate", r.DisposalDate);
            p.Add("@DisposalMethod", r.DisposalMethod);
            p.Add("@DisposalValue", r.DisposalValue);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetDisposalID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetDisposal_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetDisposalID");
        }

        public async Task<bool> ApproveDisposalAsync(long id, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetDisposalID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApprovedBy", approvedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetDisposal_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CloseDisposalAsync(long id, long tenantId, long closedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetDisposalID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", closedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetDisposal_Close", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetDisposalDto>> GetDisposalsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetDisposalDto>("SELECT * FROM asset.vw_AssetWarrantyStatus WHERE TenantID = @TenantID", new { TenantID = tenantId }); // Fallback select
        }

        // Return
        public async Task<long> CreateReturnAsync(CreateAssetReturnWorkflowRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantId);
            p.Add("@AssetAssignmentID", r.AssetAssignmentID);
            p.Add("@ReturnDate", r.ReturnDate);
            p.Add("@ReturnCondition", r.ReturnCondition);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssetReturnID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetReturn_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetReturnID");
        }

        public async Task<bool> VerifyReturnAsync(long id, long tenantId, string? condition, long verifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetReturnID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ReturnCondition", condition);
            p.Add("@VerifiedBy", verifiedBy);
            var affected = await conn.ExecuteAsync("asset.usp_AssetReturn_Verify", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<AssetReturnWorkflowDto>> GetReturnsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AssetReturnWorkflowDto>("SELECT * FROM asset.AssetReturn WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Inventory
        public async Task<long> ReconcileInventoryAsync(long tenantId, long assetId, long locationId, int quantity, string status, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@AssetID", assetId);
            p.Add("@LocationID", locationId);
            p.Add("@Quantity", quantity);
            p.Add("@InventoryStatus", status);
            p.Add("@CreatedBy", createdBy);
            p.Add("@AssetInventoryID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("asset.usp_AssetInventory_Reconcile", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetInventoryID");
        }

        public async Task<IEnumerable<AssetInventoryDto>> SearchInventoryAsync(long tenantId, long? locationId, string? status, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@LocationID", locationId);
            p.Add("@Status", status);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("asset.usp_AssetInventory_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<AssetInventoryDto>();
        }
    }
}
