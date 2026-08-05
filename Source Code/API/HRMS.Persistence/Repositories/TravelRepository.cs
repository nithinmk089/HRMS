using System;
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
    public class TravelRepository : ITravelRepository
    {
        private readonly string _connectionString;
        public TravelRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // --- Policies ---
        public async Task<IEnumerable<TravelPolicyDto>> GetPoliciesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<TravelPolicyDto>("SELECT * FROM travel.TravelPolicy WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> CreatePolicyAsync(CreateTravelPolicyRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "INSERT INTO travel.TravelPolicy (TenantID, PolicyCode, PolicyName, EffectiveFrom, EffectiveTo, CreatedBy) VALUES (@TenantID, @PolicyCode, @PolicyName, @EffectiveFrom, @EffectiveTo, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);";
            return await conn.ExecuteScalarAsync<long>(sql, r);
        }

        // --- Requests ---
        public async Task<IEnumerable<TravelRequestDto>> GetRequestsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.vw_TravelRequests WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<TravelRequestDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        public async Task<TravelRequestDto?> GetRequestByIdAsync(long id, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<TravelRequestDto>("SELECT * FROM travel.vw_TravelRequests WHERE TravelRequestID = @ID AND TenantID = @TenantID", new { ID = id, TenantID = tenantId });
        }

        public async Task<long> CreateRequestAsync(CreateTravelRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@TravelType", r.TravelType);
            p.Add("@TravelStartDate", r.TravelStartDate);
            p.Add("@TravelEndDate", r.TravelEndDate);
            p.Add("@Destination", r.Destination);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@TravelRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("travel.usp_TravelRequest_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@TravelRequestID");
        }

        public async Task ApproveRequestAsync(ApproveTravelRequestRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync("travel.usp_TravelRequest_Approve", r, commandType: CommandType.StoredProcedure);
        }

        public async Task CancelRequestAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE travel.TravelRequest SET RequestStatus = 'Cancelled', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE() WHERE TravelRequestID = @ID AND TenantID = @TenantID AND IsDeleted = 0";
            await conn.ExecuteAsync(sql, new { ID = id, TenantID = tenantId, ModifiedBy = modifiedBy });
        }

        // --- Advances ---
        public async Task<IEnumerable<TravelAdvanceDto>> GetAdvancesAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.vw_AdvanceOutstanding WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<TravelAdvanceDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        public async Task<long> CreateAdvanceAsync(CreateTravelAdvanceRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "INSERT INTO travel.TravelAdvance (TenantID, TravelRequestID, AdvanceAmount, CreatedBy) VALUES (@TenantID, @TravelRequestID, @AdvanceAmount, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);";
            return await conn.ExecuteScalarAsync<long>(sql, r);
        }

        public async Task ApproveAdvanceAsync(long id, long tenantId, long approverId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE travel.TravelAdvance SET AdvanceStatus = 'Approved', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE() WHERE TravelAdvanceID = @ID AND TenantID = @TenantID AND IsDeleted = 0";
            await conn.ExecuteAsync(sql, new { ID = id, TenantID = tenantId, ModifiedBy = modifiedBy });
        }

        public async Task DisburseAdvanceAsync(long id, long tenantId, DateTime date, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE travel.TravelAdvance SET AdvanceStatus = 'Disbursed', DisbursementDate = @Date, ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE() WHERE TravelAdvanceID = @ID AND TenantID = @TenantID AND IsDeleted = 0";
            await conn.ExecuteAsync(sql, new { ID = id, TenantID = tenantId, Date = date, ModifiedBy = modifiedBy });
        }

        // --- Categories ---
        public async Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ExpenseCategoryDto>("SELECT * FROM travel.ExpenseCategory WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> CreateCategoryAsync(CreateExpenseCategoryRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "INSERT INTO travel.ExpenseCategory (TenantID, CategoryCode, CategoryName, MaximumLimit, CreatedBy) VALUES (@TenantID, @CategoryCode, @CategoryName, @MaximumLimit, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);";
            return await conn.ExecuteScalarAsync<long>(sql, r);
        }

        // --- Claims ---
        public async Task<IEnumerable<ExpenseClaimDto>> GetClaimsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.vw_ExpenseClaims WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<ExpenseClaimDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        public async Task<ExpenseClaimDto?> GetClaimByIdAsync(long id, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<ExpenseClaimDto>("SELECT * FROM travel.vw_ExpenseClaims WHERE ExpenseClaimID = @ID AND TenantID = @TenantID", new { ID = id, TenantID = tenantId });
        }

        public async Task<long> CreateClaimAsync(CreateExpenseClaimRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@TravelRequestID", r.TravelRequestID);
            p.Add("@ClaimDate", r.ClaimDate);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@ExpenseClaimID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("travel.usp_ExpenseClaim_Create", p, commandType: CommandType.StoredProcedure);
            var claimId = p.Get<long>("@ExpenseClaimID");

            foreach (var item in r.Items)
            {
                var itemSql = "INSERT INTO travel.ExpenseClaimItem (TenantID, ExpenseClaimID, ExpenseCategoryID, ExpenseAmount, ExpenseDate, CreatedBy) VALUES (@TenantID, @ClaimID, @CategoryID, @Amount, @Date, @CreatedBy)";
                await conn.ExecuteAsync(itemSql, new { TenantID = r.TenantID, ClaimID = claimId, CategoryID = item.ExpenseCategoryID, Amount = item.ExpenseAmount, Date = item.ExpenseDate, CreatedBy = r.CreatedBy });
            }

            return claimId;
        }

        public async Task SubmitClaimAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync("travel.usp_ExpenseClaim_Submit", new { ExpenseClaimID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
        }

        public async Task ApproveClaimAsync(ApproveExpenseClaimRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync("travel.usp_ExpenseApproval_Approve", r, commandType: CommandType.StoredProcedure);
        }

        // --- Settlements ---
        public async Task<IEnumerable<ExpenseSettlementDto>> GetSettlementsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ExpenseSettlementDto>("SELECT * FROM travel.ExpenseSettlement WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task ProcessSettlementAsync(ProcessExpenseSettlementRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync("travel.usp_ExpenseSettlement_Process", r, commandType: CommandType.StoredProcedure);
        }

        // --- Receipts ---
        public async Task<long> UploadReceiptAsync(long itemId, long tenantId, string fileName, string filePath, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "INSERT INTO travel.ReceiptAttachment (TenantID, ExpenseClaimItemID, FileName, FilePath, CreatedBy) VALUES (@TenantID, @ItemID, @FileName, @FilePath, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);";
            return await conn.ExecuteScalarAsync<long>(sql, new { TenantID = tenantId, ItemID = itemId, FileName = fileName, FilePath = filePath, CreatedBy = createdBy });
        }

        public async Task DeleteReceiptAsync(long id, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE travel.ReceiptAttachment SET IsDeleted = 1, DeletedBy = @DeletedBy, DeletedDate = GETUTCDATE() WHERE ReceiptAttachmentID = @ID AND TenantID = @TenantID";
            await conn.ExecuteAsync(sql, new { ID = id, TenantID = tenantId, DeletedBy = deletedBy });
        }

        // --- Corporate Card ---
        public async Task<long> ImportTransactionAsync(ImportCorporateCardTransactionRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@TransactionDate", r.TransactionDate);
            p.Add("@MerchantName", r.MerchantName);
            p.Add("@TransactionAmount", r.TransactionAmount);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@CorporateCardTransactionID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("travel.usp_CorporateCardTransaction_Import", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@CorporateCardTransactionID");
        }

        public async Task ReconcileTransactionAsync(ReconcileCorporateCardRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync("travel.usp_CorporateCardReconciliation_Process", r, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CorporateCardTransactionDto>> GetTransactionsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.CorporateCardTransaction WHERE TenantID = @TenantID AND IsDeleted = 0";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<CorporateCardTransactionDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // --- Compliance & Analytics ---
        public async Task<IEnumerable<TravelComplianceDto>> GetComplianceAsync(long tenantId, long? travelRequestId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.vw_TravelCompliance WHERE TenantID = @TenantID";
            if (travelRequestId.HasValue) sql += " AND TravelRequestID = @TravelRequestID";
            return await conn.QueryAsync<TravelComplianceDto>(sql, new { TenantID = tenantId, TravelRequestID = travelRequestId });
        }

        public async Task<IEnumerable<TravelAnalyticsSummaryDto>> GetAnalyticsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM travel.vw_TravelAnalytics WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<TravelAnalyticsSummaryDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }
    }
}
