using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/assets")]
    [ApiController]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        public AssetsController(IAssetRepository assetRepository) => _assetRepository = assetRepository;

        // --- Categories ---
        [HttpGet("categories")]
        public async Task<IActionResult> SearchCategories([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var res = await _assetRepository.SearchCategoriesAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<IEnumerable<AssetCategoryDto>>.SuccessResult(res));
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(long id, [FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetCategoryByIdAsync(id, tenantId);
            if (res == null) return NotFound(ApiResponse<AssetCategoryDto>.FailureResult("Category not found."));
            return Ok(ApiResponse<AssetCategoryDto>.SuccessResult(res));
        }

        [HttpPost("categories")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateAssetCategoryRequest r)
        {
            r.CreatedBy = 1; // Simulated system user
            var id = await _assetRepository.CreateCategoryAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Category created."));
        }

        [HttpPut("categories/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] UpdateAssetCategoryRequest r)
        {
            r.AssetCategoryID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateCategoryAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Category updated."));
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> DeleteCategory(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.DeleteCategoryAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Category deleted."));
        }

        // --- Asset Master ---
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var res = await _assetRepository.SearchAssetsAsync(tenantId, searchText, status, page, pageSize);
            return Ok(ApiResponse<IEnumerable<AssetDto>>.SuccessResult(res));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetAssetByIdAsync(id, tenantId);
            if (res == null) return NotFound(ApiResponse<AssetDto>.FailureResult("Asset not found."));
            return Ok(ApiResponse<AssetDto>.SuccessResult(res));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateAssetRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateAssetAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Asset registered."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateAssetRequest r)
        {
            r.AssetID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateAssetAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Asset updated."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.DeleteAssetAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Asset deleted."));
        }

        // --- Assignments ---
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] long? assetId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var res = await _assetRepository.SearchAssignmentsAsync(tenantId, employeeId, assetId, status, page, pageSize);
            return Ok(ApiResponse<IEnumerable<AssetAssignmentDto>>.SuccessResult(res));
        }

        [HttpPost("assignments")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HR")]
        public async Task<IActionResult> Assign([FromBody] CreateAssetAssignmentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateAssignmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Asset assigned."));
        }

        [HttpPut("assignments/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HR")]
        public async Task<IActionResult> UpdateAssignment(long id, [FromBody] UpdateAssetAssignmentRequest r)
        {
            r.AssetAssignmentID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateAssignmentAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Assignment updated."));
        }

        [HttpPost("assignments/{id}/return")]
        public async Task<IActionResult> Return(long id, [FromBody] ReturnAssetAssignmentRequest r)
        {
            r.AssetAssignmentID = id;
            r.CreatedBy = 1;
            var ok = await _assetRepository.ReturnAssignmentAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Asset returned successfully."));
        }

        // --- Transfers ---
        [HttpGet("transfers")]
        public async Task<IActionResult> GetTransfers([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetTransfersAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetTransferDto>>.SuccessResult(res));
        }

        [HttpPost("transfers")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HR")]
        public async Task<IActionResult> Transfer([FromBody] CreateAssetTransferRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateTransferAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Transfer request submitted."));
        }

        [HttpPost("transfers/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveTransfer(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.ApproveTransferAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Transfer approved."));
        }

        [HttpPost("transfers/{id}/complete")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CompleteTransfer(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.CompleteTransferAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Transfer completed."));
        }

        // --- Maintenance ---
        [HttpGet("maintenance")]
        public async Task<IActionResult> GetMaintenance([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetMaintenanceAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetMaintenanceDto>>.SuccessResult(res));
        }

        [HttpPost("maintenance")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateMaintenance([FromBody] CreateAssetMaintenanceRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateMaintenanceAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Maintenance scheduled."));
        }

        [HttpPut("maintenance/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateMaintenance(long id, [FromBody] UpdateAssetMaintenanceRequest r)
        {
            r.AssetMaintenanceID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateMaintenanceAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Maintenance updated."));
        }

        [HttpPost("maintenance/{id}/close")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CloseMaintenance(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var ok = await _assetRepository.CloseMaintenanceAsync(id, tenantId, 1, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Maintenance closed."));
        }

        // --- Repairs ---
        [HttpGet("repairs")]
        public async Task<IActionResult> GetRepairs([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetRepairsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetRepairDto>>.SuccessResult(res));
        }

        [HttpPost("repairs")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateRepair([FromBody] CreateAssetRepairRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateRepairAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Repair request submitted."));
        }

        [HttpPut("repairs/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateRepair(long id, [FromBody] UpdateAssetRepairRequest r)
        {
            r.AssetRepairID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateRepairAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Repair details updated."));
        }

        [HttpPost("repairs/{id}/close")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CloseRepair(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var ok = await _assetRepository.CloseRepairAsync(id, tenantId, 1, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Repair status closed."));
        }

        // --- Warranties ---
        [HttpGet("warranties")]
        public async Task<IActionResult> GetWarranties([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetWarrantiesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetWarrantyDto>>.SuccessResult(res));
        }

        [HttpPost("warranties")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateWarranty([FromBody] CreateAssetWarrantyRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateWarrantyAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Warranty registered."));
        }

        [HttpPut("warranties/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateWarranty(long id, [FromBody] UpdateAssetWarrantyRequest r)
        {
            r.AssetWarrantyID = id;
            r.ModifiedBy = 1;
            var ok = await _assetRepository.UpdateWarrantyAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Warranty updated."));
        }

        [HttpGet("warranties/expiry-report")]
        public async Task<IActionResult> GetWarrantyExpiryReport([FromQuery] long tenantId, [FromQuery] int withinDays = 30)
        {
            var res = await _assetRepository.GetWarrantyExpiryReportAsync(tenantId, withinDays);
            return Ok(ApiResponse<IEnumerable<WarrantyExpiryReportDto>>.SuccessResult(res));
        }

        // --- Depreciation ---
        [HttpGet("depreciation")]
        public async Task<IActionResult> GetDepreciations([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetDepreciationsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetDepreciationDto>>.SuccessResult(res));
        }

        [HttpPost("depreciation/recalculate")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> RecalculateDepreciation([FromQuery] long tenantId)
        {
            var ok = await _assetRepository.RecalculateDepreciationAsync(tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Depreciation recalculated."));
        }

        [HttpGet("depreciation/report")]
        public async Task<IActionResult> GetDepreciationReport([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetDepreciationReportAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetDepreciationReportDto>>.SuccessResult(res));
        }

        // --- Audits ---
        [HttpGet("audits")]
        public async Task<IActionResult> GetAudits([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetAuditsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetAuditDto>>.SuccessResult(res));
        }

        [HttpPost("audits")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAssetAuditRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateAuditAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Audit scheduled."));
        }

        [HttpPost("audits/{id}/complete")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CompleteAudit(long id, [FromQuery] long tenantId, [FromQuery] string status, [FromQuery] string? findings)
        {
            var ok = await _assetRepository.CompleteAuditAsync(id, tenantId, status, findings, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Audit status finalized."));
        }

        // --- Disposals ---
        [HttpGet("disposals")]
        public async Task<IActionResult> GetDisposals([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetDisposalsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetDisposalDto>>.SuccessResult(res));
        }

        [HttpPost("disposals")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateDisposal([FromBody] CreateAssetDisposalRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateDisposalAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Disposal request submitted."));
        }

        [HttpPost("disposals/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveDisposal(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.ApproveDisposalAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Disposal approved."));
        }

        [HttpPost("disposals/{id}/close")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CloseDisposal(long id, [FromQuery] long tenantId)
        {
            var ok = await _assetRepository.CloseDisposalAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Disposal status closed."));
        }

        // --- Returns ---
        [HttpGet("returns")]
        public async Task<IActionResult> GetReturns([FromQuery] long tenantId)
        {
            var res = await _assetRepository.GetReturnsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AssetReturnWorkflowDto>>.SuccessResult(res));
        }

        [HttpPost("returns")]
        public async Task<IActionResult> CreateReturn([FromBody] CreateAssetReturnWorkflowRequest r)
        {
            r.CreatedBy = 1;
            var id = await _assetRepository.CreateReturnAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Return logged."));
        }

        [HttpPost("returns/{id}/verify")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> VerifyReturn(long id, [FromQuery] long tenantId, [FromQuery] string? condition)
        {
            var ok = await _assetRepository.VerifyReturnAsync(id, tenantId, condition, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Return verified."));
        }

        // --- Inventory ---
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory([FromQuery] long tenantId, [FromQuery] long? locationId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var res = await _assetRepository.SearchInventoryAsync(tenantId, locationId, status, page, pageSize);
            return Ok(ApiResponse<IEnumerable<AssetInventoryDto>>.SuccessResult(res));
        }

        [HttpPost("inventory/reconcile")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ReconcileInventory([FromQuery] long tenantId, [FromQuery] long assetId, [FromQuery] long locationId, [FromQuery] int quantity, [FromQuery] string status)
        {
            var id = await _assetRepository.ReconcileInventoryAsync(tenantId, assetId, locationId, quantity, status, 1);
            return Ok(ApiResponse<long>.SuccessResult(id, "Inventory reconciled."));
        }
    }
}
