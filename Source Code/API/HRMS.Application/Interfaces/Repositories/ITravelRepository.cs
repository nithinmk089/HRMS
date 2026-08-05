using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ITravelRepository
    {
        // Policies
        Task<IEnumerable<TravelPolicyDto>> GetPoliciesAsync(long tenantId);
        Task<long> CreatePolicyAsync(CreateTravelPolicyRequest r);

        // Requests
        Task<IEnumerable<TravelRequestDto>> GetRequestsAsync(long tenantId, long? employeeId);
        Task<TravelRequestDto?> GetRequestByIdAsync(long id, long tenantId);
        Task<long> CreateRequestAsync(CreateTravelRequest r);
        Task ApproveRequestAsync(ApproveTravelRequestRequest r);
        Task CancelRequestAsync(long id, long tenantId, long modifiedBy);

        // Advances
        Task<IEnumerable<TravelAdvanceDto>> GetAdvancesAsync(long tenantId, long? employeeId);
        Task<long> CreateAdvanceAsync(CreateTravelAdvanceRequest r);
        Task ApproveAdvanceAsync(long id, long tenantId, long approverId, long modifiedBy);
        Task DisburseAdvanceAsync(long id, long tenantId, DateTime date, long modifiedBy);

        // Categories
        Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(long tenantId);
        Task<long> CreateCategoryAsync(CreateExpenseCategoryRequest r);

        // Claims
        Task<IEnumerable<ExpenseClaimDto>> GetClaimsAsync(long tenantId, long? employeeId);
        Task<ExpenseClaimDto?> GetClaimByIdAsync(long id, long tenantId);
        Task<long> CreateClaimAsync(CreateExpenseClaimRequest r);
        Task SubmitClaimAsync(long id, long tenantId, long modifiedBy);
        Task ApproveClaimAsync(ApproveExpenseClaimRequest r);

        // Settlements
        Task<IEnumerable<ExpenseSettlementDto>> GetSettlementsAsync(long tenantId);
        Task ProcessSettlementAsync(ProcessExpenseSettlementRequest r);

        // Receipts
        Task<long> UploadReceiptAsync(long itemId, long tenantId, string fileName, string filePath, long createdBy);
        Task DeleteReceiptAsync(long id, long tenantId, long deletedBy);

        // Corporate Card
        Task<long> ImportTransactionAsync(ImportCorporateCardTransactionRequest r);
        Task ReconcileTransactionAsync(ReconcileCorporateCardRequest r);
        Task<IEnumerable<CorporateCardTransactionDto>> GetTransactionsAsync(long tenantId, long? employeeId);

        // Compliance & Analytics
        Task<IEnumerable<TravelComplianceDto>> GetComplianceAsync(long tenantId, long? travelRequestId);
        Task<IEnumerable<TravelAnalyticsSummaryDto>> GetAnalyticsAsync(long tenantId, long? employeeId);
    }
}
