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
    public class OffboardingRepository : IOffboardingRepository
    {
        private readonly string _connectionString;
        public OffboardingRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateExitRequestAsync(CreateExitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@ResignationDate", request.ResignationDate);
            p.Add("@LastWorkingDate", request.LastWorkingDate);
            p.Add("@ExitReason", request.ExitReason);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ExitRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_ExitRequest_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ExitRequestID");
        }

        public async Task<bool> UpdateExitRequestAsync(UpdateExitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ExitRequestID", request.ExitRequestID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@LastWorkingDate", request.LastWorkingDate);
            p.Add("@ExitReason", request.ExitReason);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("offboarding.usp_ExitRequest_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> SubmitExitRequestAsync(long requestId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ExitRequestID", requestId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_ExitRequest_Submit", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> ApproveExitRequestAsync(ApproveExitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ExitRequestID", request.ExitRequestID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@ApproverID", request.ApproverID);
            p.Add("@Remarks", request.Remarks);
            p.Add("@ExitApprovalID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_ExitRequest_Approve", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ExitApprovalID");
        }

        public async Task<long> RejectExitRequestAsync(RejectExitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ExitRequestID", request.ExitRequestID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@ApproverID", request.ApproverID);
            p.Add("@Remarks", request.Remarks);
            p.Add("@ExitApprovalID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_ExitRequest_Reject", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ExitApprovalID");
        }

        public async Task<long> CreateClearanceRequestAsync(CreateClearanceRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@InitiatedDate", request.InitiatedDate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ClearanceRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_ClearanceRequest_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ClearanceRequestID");
        }

        public async Task<bool> ApproveClearanceRequestAsync(long clearanceRequestId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ClearanceRequestID", clearanceRequestId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_ClearanceRequest_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CompleteClearanceTaskAsync(long clearanceTaskId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ClearanceTaskID", clearanceTaskId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_ClearanceTask_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> CreateAssetReturnAsync(CreateAssetReturnRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@AssetID", request.AssetID);
            p.Add("@ReturnDate", request.ReturnDate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@AssetReturnID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_AssetReturn_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssetReturnID");
        }

        public async Task<bool> VerifyAssetReturnAsync(VerifyAssetReturnRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AssetReturnID", request.AssetReturnID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@ReturnCondition", request.ReturnCondition);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("offboarding.usp_AssetReturn_Verify", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> CreateKnowledgeTransferAsync(CreateKnowledgeTransferRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@SuccessorEmployeeID", request.SuccessorEmployeeID);
            p.Add("@KTDate", request.KTDate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@KnowledgeTransferID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_KnowledgeTransfer_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@KnowledgeTransferID");
        }

        public async Task<bool> CompleteKnowledgeTransferAsync(long ktId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@KnowledgeTransferID", ktId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_KnowledgeTransfer_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> GenerateExperienceLetterAsync(GenerateExperienceLetterRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ExperienceLetterRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_ExperienceLetter_Generate", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ExperienceLetterRequestID");
        }

        public async Task<long> CalculateFullAndFinalSettlementAsync(CalculateFFSRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@SettlementAmount", request.SettlementAmount);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@FullAndFinalSettlementID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("offboarding.usp_FullAndFinalSettlement_Calculate", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@FullAndFinalSettlementID");
        }

        public async Task<bool> ApproveFullAndFinalSettlementAsync(long ffsId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@FullAndFinalSettlementID", ffsId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_FullAndFinalSettlement_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CloseFullAndFinalSettlementAsync(long ffsId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@FullAndFinalSettlementID", ffsId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("offboarding.usp_FullAndFinalSettlement_Close", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<ExitStatusReportDto>> GetExitStatusReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<ExitStatusReportDto>("offboarding.usp_Report_ExitStatus", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ClearanceStatusReportDto>> GetClearanceStatusReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<ClearanceStatusReportDto>("offboarding.usp_Report_ClearanceStatus", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FullAndFinalSummaryReportDto>> GetFullAndFinalSummaryReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<FullAndFinalSummaryReportDto>("offboarding.usp_Report_FullAndFinalSummary", p, commandType: CommandType.StoredProcedure);
        }
    }
}
